using UnityEngine;
using System.Collections;

public class PropSpinAntiClockwise : MonoBehaviour {

    public float idleSpeed = 20;
    [Header("\\/ \\/ High Number = Slower Speed")]
    public float movingSpeed = 5;
    private Rigidbody droneRB;

    void Start () {
        droneRB = this.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        this.transform.Rotate(new Vector3(0, -(idleSpeed + (droneRB.velocity.magnitude / movingSpeed)), 0));
        if (this.GetComponent<AudioSource>()) {
            this.GetComponent<AudioSource>().pitch = 1 + (droneRB.velocity.magnitude / ((movingSpeed) * 20));
        }
    }
}
