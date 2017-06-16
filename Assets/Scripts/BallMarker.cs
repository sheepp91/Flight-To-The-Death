using UnityEngine;
using System.Collections;

public class BallMarker : MonoBehaviour {

    public Transform ballTransform;
	
	void Update () {
        Vector3 newPos = ballTransform.position;
        newPos.y = this.transform.position.y;
        this.transform.position = newPos;
    }
}
