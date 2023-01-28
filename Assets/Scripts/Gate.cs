using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform player;
    private int numberOfStickmans;
    [SerializeField] private TextMeshPro skorText;
    [SerializeField] private GameObject stickman;

    [Range(0f, 1f)] [SerializeField] private float distance, radius;
    void Start()
    {
        player = transform;
        numberOfStickmans = transform.childCount - 1;
        skorText.text = numberOfStickmans.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FormatStickman()
    {
        for (int i = 0; i < player.childCount; i++)
        {
            var x = distance * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distance * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var NewPos = new Vector3(x, 0f, z);

            player.transform.GetChild(i).DOLocalMove(NewPos, 1f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(stickman, transform.position, Quaternion.identity, transform);

        }
        numberOfStickmans = transform.childCount - 1;
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
                MakeStickMan(numberOfStickmans * gatemanager.randomNumber);
            }
            else
            {
                MakeStickMan(numberOfStickmans - gatemanager.randomNumber);
            }


        


        }


    }

}
