using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 2.0f;
    public float smoothing = 2.0f;

    [HideInInspector]
    public bool p1Inverted = false;
    [HideInInspector]
    public bool p2Inverted = false;

    private GameController gcScript;
    private GameObject drone;
    private GameObject cameraAnchor;
    private BallCollisions ballCollisionScript;

    private GameObject menuController;
    private MenuController mScript;

    void Start() {
        gcScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cameraAnchor = this.transform.parent.gameObject;
        drone = cameraAnchor.transform.parent.gameObject;
        ballCollisionScript = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallCollisions>();

        menuController = GameObject.FindGameObjectWithTag("MenuController");
        if (menuController != null) {
            mScript = menuController.GetComponent<MenuController>();
            if (mScript.sensitivity != 2) {
                sensitivity = mScript.sensitivity;
            }
            sensitivity = mScript.GetComponent<MenuController>().sensitivity; // If there is an error here, ignore it.
            Debug.Log("Mouse look sens: " + sensitivity);
            Debug.Log("menu sens: " + mScript.sensitivity);
        }
        else {
            sensitivity = 2.0f;
        }
    }

    void Update() {
        if (!gcScript.isPaused() && !ballCollisionScript.tutorialMessage1Triggered && !ballCollisionScript.tutorialMessage2Triggered && !ballCollisionScript.tutorialMessage3Triggered) {
            Vector2 md = Vector2.zero;

            if (drone.CompareTag("Player1")) {
                if (!p1Inverted) {
                    md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                } else {
                    md = new Vector2(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
                }
            } else if (drone.CompareTag("Player2")) {
                if (!p1Inverted) {
                    md = new Vector2(Input.GetAxisRaw("Mouse X_P2"), Input.GetAxisRaw("Mouse Y_P2"));
                } else {
                    md = new Vector2(-Input.GetAxisRaw("Mouse X_P2"), -Input.GetAxisRaw("Mouse Y_P2"));
                }
            }

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -45f, 70f);

            cameraAnchor.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

            if (drone.CompareTag("Player1")) {
                drone.transform.localRotation = Quaternion.AngleAxis(mouseLook.x + 180, Vector3.up);
            } else if (drone.CompareTag("Player2")) {
                drone.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
            }

        } else if (gcScript.isPaused()) {
            if (mScript != null) {
                //if (mScript.sensitivity != 2)  {
                    sensitivity = mScript.sensitivity;
                //}
            }
        }
    }
}