using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour {

    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player1")) {
            Debug.Log("Pickup collected by player 1");
            this.gameObject.SetActive(false);
        }

        else if (other.CompareTag("Player2")) {
            Debug.Log("Pickup collected by player 2");
            this.gameObject.SetActive(false);
        }
    }
}
