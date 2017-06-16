using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//dgkshda

public class DroneMove_Net : NetworkBehaviour {

    public float speed;
    public float boostSpeed;
    public float tiltFactor;
    public bool magic;

    private Rigidbody rb;
    private Transform droneBodyTransform;

    private bool isBoosting = false;

    private float initialDroneYRotation;

    void Start () {
        if (!isLocalPlayer) {
            Destroy(this.transform.GetChild(1).GetChild(0).gameObject); // destroy other player's camera
            Destroy(this); // destroy other players control script
            return;
        }
        rb = GetComponent<Rigidbody>();
        droneBodyTransform = this.transform.GetChild(0);

        initialDroneYRotation = droneBodyTransform.rotation.eulerAngles.y;
    }

    void FixedUpdate () {

        float moveHorizontal = 0;
        float moveForwardBack = 0;
        float moveUpDown = 0;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveForwardBack = Input.GetAxis("Vertical");
        moveUpDown = Input.GetAxis("ClimbFall_P1");

        // Move Drone
        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveForwardBack);
        rb.AddRelativeForce(movement * speed);

        // Tilt Drone
        Vector3 tilt = new Vector3(moveHorizontal * tiltFactor, initialDroneYRotation, moveForwardBack * tiltFactor);
        droneBodyTransform.localRotation = Quaternion.Euler(tilt);

        //CmdMove();
    }

    [Command]
    void CmdMove () {
        float moveHorizontal = 0;
        float moveForwardBack = 0;
        float moveUpDown = 0;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveForwardBack = Input.GetAxis("Vertical");
        moveUpDown = Input.GetAxis("ClimbFall_P1");

        // Move Drone
        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveForwardBack);
        rb.AddRelativeForce(movement * speed);

        // Tilt Drone
        Vector3 tilt = new Vector3(moveHorizontal * tiltFactor, initialDroneYRotation, moveForwardBack * tiltFactor);
        droneBodyTransform.localRotation = Quaternion.Euler(tilt);
    }

    void Update () {
        if (Input.GetAxis("Boost_P1") < -0.5f && !isBoosting) {
            speed += boostSpeed;
            isBoosting = true;
        } else if (Input.GetAxis("Boost_P1") > -0.5f && isBoosting) {
            speed -= boostSpeed;
            isBoosting = false;
        }
    }
}
