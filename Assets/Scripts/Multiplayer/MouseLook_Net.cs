using UnityEngine;
using System.Collections;

public class MouseLook_Net : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    private GameObject drone;
    private GameObject cameraAnchor;

    void Start () {
        cameraAnchor = this.transform.parent.gameObject;
        drone = cameraAnchor.transform.parent.gameObject;
    }

    void Update () {
        Vector2 md = Vector2.zero;
        
        md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -45f, 70f);

        cameraAnchor.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        drone.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
    }
}
