using UnityEngine;
using System.Collections;

public class ExplodeOnLose : MonoBehaviour {

    public GameObject explosion;

    private GameObject droneBody;
    private Transform playerCamera;
    private GameObject playerArrow;

    private AudioSource[] sfx;

    void Start() {
        droneBody = transform.GetChild(0).gameObject;
        playerCamera = transform.GetChild(1).GetChild(0);
        playerArrow = transform.GetChild(1).GetChild(1).gameObject;

        sfx = this.GetComponents<AudioSource>();
    }

    public void explode() {
        //Destroy(transform.GetChild(0));
        if (droneBody != null) {
            Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
            sfx[1].Play(); // Explode sfx
            sfx[3].Play(); // Cheering sfx
            Destroy(droneBody);
            this.GetComponent<DroneMove>().enabled = false;
            playerCamera.GetComponent<MouseLook>().enabled = false;
            playerArrow.SetActive(false);
        }
    }
}
