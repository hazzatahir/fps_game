using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    Rigidbody rb;

    Vector3 previousPos , currentPos;

    public bool canInteract;

    float shootTime = 0.15f;
    float shootTimer;
    public bool canShoot = false;

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
        shootTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && GameplayController.instance.gameMode != GameMode.END)
        {
            GameplayController.instance.GivePlayerInfo();
        }

        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                GameplayController.instance.GiveNPCInfo();
                
            }
        }

        if (Input.GetMouseButtonDown(1) && GameplayController.instance.gameMode == GameMode.WAR)
        {
            canShoot = true;

            Vector2 camPos = transform.GetChild(0).localPosition;

            camPos.x = 0.2f;

            transform.GetChild(0).localPosition = camPos;
        }

        if (Input.GetMouseButtonUp(1))
        {
            canShoot = false;

            Vector2 camPos = transform.GetChild(0).localPosition;

            camPos.x = 0;

            transform.GetChild(0).localPosition = camPos;
        }

        if (Input.GetMouseButton(0) && canShoot && GameplayController.instance.gameMode == GameMode.WAR)
        {
            if(shootTimer >= shootTime)
            {
                //Debug.Log("SHOOT");
                GetComponent<PlayerScript>().Shooting();
                AudioManager.instance.PlayPlayerGunshot();
                shootTimer = 0;
            }
        }

    }

    void CheckPosition()
    {
        currentPos = transform.position;

        if(Vector3.Distance(previousPos , currentPos) >= 10)
        {
            //Debug.Log("YOUR HEALTH IS REDUCED");
            //GameplayController.instance.DecreaseHealthPoint(1);
            previousPos = transform.position;
        }
    }

    void SetPos()
    {
        previousPos = transform.position;
    }


} //class













