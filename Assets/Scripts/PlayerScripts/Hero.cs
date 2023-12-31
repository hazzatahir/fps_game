﻿using UnityEngine;
using AFPC;

/// <summary>
/// Example of setup AFPC with Lifecycle, Movement and Overview classes.
/// </summary>
public class Hero : MonoBehaviour {

    public Movement movement;

    public Overview overview;

    /* Some classes need to initizlize */
    private void Start () {

        /* a few apllication settings for more smooth. This is Optional. */
        QualitySettings.vSyncCount = 0;
        overview.camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        movement.Initialize();
        movement.AssignLandingAction (()=> overview.Shake(0.5f));
    }

    private void Update () {

        /* Read player input before check availability */
        ReadInput();

        /* Mouse look state */
        overview.Looking();

        /* Change camera FOV state */
        //overview.Aiming();

        /* Shake camera state. Required "physical camera" mode on */
        overview.Shaking();

        /* Control the speed */
        movement.Running();

        /* Control the jumping, ground search... */
        movement.Jumping();

    }

    private void FixedUpdate () {

       
        /* Physical movement */
        movement.Accelerate();

        /* Physical rotation with camera */
        overview.RotateRigigbodyToLookDirection (movement.rb);
    }

    private void LateUpdate () {

       /* Camera following */
        overview.Follow (transform.position);
    }

    private void ReadInput () {
        overview.lookingInputValues.x = Input.GetAxis("Mouse X");
        overview.lookingInputValues.y = Input.GetAxis("Mouse Y");
        overview.aimingInputValue = Input.GetMouseButton(1);
        movement.movementInputValues.x = Input.GetAxis("Horizontal");
        movement.movementInputValues.y = Input.GetAxis("Vertical");
        movement.jumpingInputValue = Input.GetButtonDown("Jump");
        movement.runningInputValue = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
            }
            else if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
            }
        }
    }

}
