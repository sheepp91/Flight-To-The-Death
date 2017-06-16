using UnityEngine;
using System.Collections;

public class BallCrosshair : MonoBehaviour {

    public Transform ballCrosshair;

    void Update () {
        Vector3 down = Vector3.down * 100f;
        //Debug.DrawRay(transform.position, down, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, down, out hit, 100f)) {
            ballCrosshair.localPosition = hit.point;
        }
    }
}
