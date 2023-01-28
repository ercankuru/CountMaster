using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    // ***Deðiþkenler***

    public TextMeshPro gateNo;
    public int randomNumber;
    public bool multiplier;
    void Start()
    {
        if (multiplier)
        {
            randomNumber = Random.Range(2, 4);
            gateNo.text = "X" + randomNumber;
        }
        else
        {
            randomNumber = Random.Range(10, 100);
            if (randomNumber % 2 != 0)
                randomNumber += 1;

            gateNo.text = randomNumber.ToString();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
