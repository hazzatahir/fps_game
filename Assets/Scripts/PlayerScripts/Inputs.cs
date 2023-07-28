using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    Rigidbody rb;

    Vector3 previousPos , currentPos;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Invoke("SetPos", 1f);
    }

    void Start()
    {
        InvokeRepeating("CheckPosition", 1f, 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameplayController.instance.GivePlayerInfo();
        }

    }

    void CheckPosition()
    {
        currentPos = transform.position;

        if(Vector3.Distance(previousPos , currentPos) >= 10)
        {
            Debug.Log("YOUR HEALTH IS REDUCED");
            GameplayController.instance.DecreaseHealthPoint();
            previousPos = transform.position;
        }
    }

    void SetPos()
    {
        previousPos = transform.position;
    }


} //class













