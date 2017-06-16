using UnityEngine;
using System.Collections;

public class PropSpinClockwise : MonoBehaviour {

    public float idleSpeed = 20;
    [Header("\\/ \\/ High Number = Slower Speed")]
    public float movingSpeed = 5;
    private Rigidbody droneRB;

    void Start () {
        droneRB = this.transform.parent.parent.parent.GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        this.transform.Rotate(new Vector3(0, idleSpeed + (droneRB.velocity.magnitude / movingSpeed), 0));
    }
}
