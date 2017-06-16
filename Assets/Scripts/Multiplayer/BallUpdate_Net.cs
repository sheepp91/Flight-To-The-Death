using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BallUpdate_Net : NetworkBehaviour {

    private Transform myTransform;

// Object rotation vars
    [SyncVar] private Quaternion syncObjectRotation;
    public float rotationLerpRate = 15;

    private Quaternion lastRot;
    private float thresholdRot = 5f;

    //private Transform myTransform;
    [SerializeField]
    float lerpRate = 15;
    [SyncVar]
    private Vector3 syncPos;

    private Vector3 lastPos;
    private float thresholdPos = 0.5f;




    // Use this for initialization
    void Start () {
        myTransform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        // Pos
        TransmitPosition();
        LerpPosition();
        // Rot
        //LerpRotation();
        //TransmitRotation();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    private void LerpRotation()
    {

        if (!isLocalPlayer)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncObjectRotation, Time.deltaTime * rotationLerpRate);
        }

    }

    [Command]
    void Cmd_ProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [Command]
    private void Cmd_ProvideRotationToServer(Quaternion objectRotation)
    {
        syncObjectRotation = objectRotation;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > thresholdPos)
        {
            Cmd_ProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }


    [ClientCallback]
    private void TransmitRotation()
    { // Send rotation to server
        if (isLocalPlayer && Quaternion.Angle(myTransform.rotation, lastRot) > thresholdRot)
        {
            Cmd_ProvideRotationToServer(myTransform.rotation);
            lastRot = myTransform.rotation;
        }
    }
}
