using UnityEngine;
using System.Collections;

public class DropObject : MonoBehaviour {

    public ArrowToDestination destArrowScript;

    void Update () {
	    if (Input.GetButtonDown("DropObject")) {
            if (this.transform.childCount > 0) {
                destArrowScript.SetDestination(this.transform.GetChild(0));

                this.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // turn off camera

                this.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                this.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                this.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                this.transform.GetChild(0).parent = null;
            }
        }
	}
}
