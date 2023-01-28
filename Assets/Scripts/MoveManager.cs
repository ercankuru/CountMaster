using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    // DEÐÝÞKENLER
    private Vector3 move;
    [SerializeField] private float x = 0;
    [SerializeField] private float z = 1;
    [SerializeField] private float speed = 10f;
    

    void Start()
    {
        move = new Vector3(1f, 0f, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        move = new Vector3(x, 0f, z);
        transform.position += move;
        
    }
}
