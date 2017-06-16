using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour {

    float timer;
    float timeAlive = 3f;

	void Update () {
        if (timer < timeAlive) {
            timer += Time.deltaTime;
        } else {
            timer = 0f;
            Destroy(this.gameObject);
        }
	}
}
