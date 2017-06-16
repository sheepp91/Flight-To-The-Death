using UnityEngine;
using System.Collections;

public class BallGravity : MonoBehaviour {

    public float gravity;

    void Start () {
        //this.GetComponent<Rigidbody>().AddForce(-Vector3.up * gravity);
    }

    void FixedUpdate () {
        
    }
}

//http://answers.unity3d.com/questions/1162421/how-to-make-object-fall-slowly-but-bounce-normally.html

