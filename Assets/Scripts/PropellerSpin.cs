using UnityEngine;
using System.Collections;

public class PropellerSpin : MonoBehaviour {

    public float speed = 10;

	void Start () {
	
	}
	
	void Update () {
        this.transform.Rotate(new Vector3(0, speed, 0));
	}
}
