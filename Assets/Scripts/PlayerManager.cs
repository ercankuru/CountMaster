using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;



public class PlayerManager : MonoBehaviour
{
    // ***DEÐÝÞKENLER

    public Transform player;
    private int numberOfStickmans, numberOfEnemyStickmans;
    [SerializeField] private TextMeshPro skorText;
    [SerializeField] private GameObject stickman;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject win;

    // dictance: her çöp adam arasýndaki mesafeyi belirler. radius: stikman kalabalýðýnýn þeklini deðiþtirecek
    [Range(0f, 1f)] [SerializeField] private float distance, radius;

    //**** hareket 
    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    private Camera camera1;

    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    private bool attack;
    public static PlayerManager PlayerManagerInstance;



    void Start()

    {
        player = transform;
        numberOfStickmans = transform.childCount - 1;
        skorText.text = numberOfStickmans.ToString();
        PlayerManagerInstance = this;
        camera1 = Camera.main;
        PlayerManagerInstance = this;
        //gameState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
            }

            if (enemy.GetChild(1).childCount > 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var Distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;

                    if (Distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y,
                                enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1f);
                    }
                }
            }

            else
            {
                attack = false;
                roadSpeed = 4f;

                FormatStickman();

                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity; 
               
                enemy.gameObject.SetActive(false);


            }
            if (transform.childCount == 1)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                gameObject.SetActive(false);
            }
        }
        else
        {
            MoveThePlayer();

            

            

        }




        if (gameState)
        {
            road.Translate(road.forward * Time.deltaTime * roadSpeed);

            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Animator>() != null)
                    transform.GetChild(i).GetComponent<Animator>().SetBool("run", true);

            }
        }

    }
    //*** METOTLAR



    public void FormatStickman()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = distance * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distance * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var NewPos = new Vector3(x, 0.15f, z);

            player.transform.GetChild(i).DOLocalMove(NewPos, 1f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(stickman, transform.position, Quaternion.identity, transform);

        }
        numberOfStickmans = transform.childCount;
        skorText.text = numberOfStickmans.ToString();

        FormatStickman();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<Collider>().enabled = false;

            var gatemanager = other.GetComponent<GateManager>();

            if (gatemanager.multiplier)
            {
              //  MakeStickMan(numberOfStickmans * gatemanager.randomNumber);
               MakeStickMan( gatemanager.randomNumber);
            }
            else
            {
                MakeStickMan(gatemanager.randomNumber);
            }



        }
        if (other.CompareTag("gate1"))
        {
            other.transform.parent.GetChild(1).GetComponent<Collider>().enabled = true;

            var gatemanager1 = other.GetComponent<GateManager1>();

            if (gatemanager1.multiplier)
            {
                MakeStickMan(gatemanager1.randomNumber);
            }
            else
            {
                MakeStickMan(gatemanager1.randomNumber);
            }



        }
        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            attack = true;

            roadSpeed = 0.5f;


            other.transform.GetChild(1).GetComponent<EnemyManager>().Attack(transform);
           
            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());

            
        }

        if (other.CompareTag("wall"))
        {
            GetComponent<BoxCollider>().isTrigger = false;
            win.gameObject.SetActive(true);
            gameState = false;
            
        }
       
           
        




        // print("game over");

    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true;

            var plane = new Plane(Vector3.up, 0f);

            var ray = camera1.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;

        }

        if (moveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            var ray = camera1.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);

                var move = mousePos - mouseStartPos;

                var control = playerStartPos + move;


                if (numberOfStickmans > 50)
                {

                    control.x = Mathf.Clamp(control.x, -1f, 1f);
                }
                else
                    control.x = Mathf.Clamp(control.x, -3f, 2.5f); //sað sol alaný

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed)
                    , transform.position.y, transform.position.z);


            }
        }


    }
    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {

        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount - 1;
        numberOfStickmans = transform.childCount - 1;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<EnemyManager>().CounterTxt.text = numberOfEnemyStickmans.ToString();
            skorText.text = numberOfStickmans.ToString();

            yield return null;
        }


        if (numberOfStickmans == 0)
        {
            roadSpeed = 0f;
            gameOver.gameObject.SetActive(true);
        }
    
      

    }
}

