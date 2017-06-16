using UnityEngine;
using System.Collections;

public class ArrowToDestination : MonoBehaviour {

    public Transform dest;

    float xRot;
    float zRot;

	void Start () {
        //xRot = this.transform.localRotation.eulerAngles.x;
        zRot = this.transform.localRotation.eulerAngles.z;
    }
	
	void Update () {
        this.transform.LookAt(dest);
        Vector3 temp = this.transform.localRotation.eulerAngles;
        //temp.x = xRot;
        temp.z = zRot;
        this.transform.localRotation = Quaternion.Euler(temp.x + 95, temp.y, temp.z);
    }

    public void SetDestination(Transform objTrans) {
        dest = objTrans;
    }
}
