using UnityEngine;
using System.Collections;

public class PowerupRespawn : MonoBehaviour {

    public GameObject powerup;

    float timer;
    public float respawnTime = 10.0f;
	
    void Start() {
        spawnPowerup();
    }

	void Update () {
	    if (transform.childCount < 1) {
            if (timer < respawnTime) {
                timer += Time.deltaTime;
            } else {
                timer = 0f;
                spawnPowerup();
            }
        }
	}

    void spawnPowerup() {
        GameObject spawnedPowerup = Instantiate(powerup, this.transform) as GameObject;
        spawnedPowerup.transform.parent = this.transform;
        spawnedPowerup.transform.localPosition = Vector3.zero;
    }
}
