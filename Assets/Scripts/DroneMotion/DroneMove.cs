using UnityEngine;
//using XInputDotNetPure; // Required in C#
using System.Collections;

public class DroneMove : MonoBehaviour {
    
    public float speed;
    public float boostSpeed;
    public float tiltFactor;
    public float sensitivity;
    public bool magic;

    [HideInInspector]
    public bool p1Inverted = false;
    [HideInInspector]
    public bool p2Inverted = false;

    private Rigidbody rb;
    private Transform droneBodyTransform;

    private bool isBoosting = false;

    private float initialDroneYRotation;

    void Awake() {
        GameObject menuController = GameObject.FindGameObjectWithTag("MenuController");
        if (menuController != null) {
            sensitivity = menuController.GetComponent<MenuController>().sensitivity; // If there is an error here, ignore it.
        }
        else {
            sensitivity = 1;
        }
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        droneBodyTransform = this.transform.GetChild(0);

        initialDroneYRotation = droneBodyTransform.rotation.eulerAngles.y;
    }
    
    void FixedUpdate () {
        float moveHorizontal = 0;
        float moveForwardBack = 0;
        float moveUpDown = 0;

        if (this.CompareTag("Player1")) {
            if (!p1Inverted) {
                moveHorizontal = Input.GetAxis("Horizontal")/* * sensitivity*/;
                moveForwardBack = Input.GetAxis("Vertical")/* * sensitivity*/;
                moveUpDown = Input.GetAxis("ClimbFall_P1");
            } else {
                moveHorizontal = -Input.GetAxis("Horizontal");
                moveForwardBack = -Input.GetAxis("Vertical");
                moveUpDown = -Input.GetAxis("ClimbFall_P1");
            }
        } else if (this.CompareTag("Player2")) {
            if (!p2Inverted) {
                moveHorizontal = Input.GetAxis("Horizontal_P2")/* * sensitivity*/;
                moveForwardBack = Input.GetAxis("Vertical_P2")/* * sensitivity*/;
                moveUpDown = Input.GetAxis("ClimbFall_P2");
            } else {
                moveHorizontal = -Input.GetAxis("Horizontal_P2");
                moveForwardBack = -Input.GetAxis("Vertical_P2");
                moveUpDown = -Input.GetAxis("ClimbFall_P2");
            }
        }

        // Move Drone
        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveForwardBack);
        rb.AddRelativeForce(movement * speed);

        // Tilt Drone
        Vector3 tilt = new Vector3(moveHorizontal * tiltFactor, initialDroneYRotation, moveForwardBack * tiltFactor);
        if (droneBodyTransform != null) {
            droneBodyTransform.localRotation = Quaternion.Euler(tilt);
        }
    }

    void Update() {
        if (this.CompareTag("Player1")) {
            if (Input.GetAxis("Boost_P1") < -0.5f && !isBoosting) {
                speed += boostSpeed;
                isBoosting = true;
            } else if (Input.GetAxis("Boost_P1") > -0.5f && isBoosting) {
                speed -= boostSpeed;
                isBoosting = false;
            }
        } else if (this.CompareTag("Player2")) {
            if (Input.GetAxis("Boost_P2") < -0.5f && !isBoosting) {
                speed += boostSpeed;
                isBoosting = true;
            } else if (Input.GetAxis("Boost_P2") > -0.5f && isBoosting) {
                speed -= boostSpeed;
                isBoosting = false;
            }
        }
    }
}
