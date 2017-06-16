using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {

    public Spawner spawnerScript;

    //void Start() {
    //    spawnerScript = GameObject.Find("Game Controller").GetComponent<Spawner>();
    //}

    void OnTriggerEnter (Collider c) {
        if (c.gameObject.CompareTag("Pickupable")) {
            Transform packageTransform = c.gameObject.transform;
            Rigidbody packageRigidbody = packageTransform.GetComponent<Rigidbody>();
            packageTransform.GetChild(0).gameObject.SetActive(true);

            packageTransform.parent = this.transform;
            packageTransform.position = this.transform.position;
            packageTransform.GetComponent<ParticleSystem>().Stop();

            packageRigidbody.useGravity = false;
            packageRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

            spawnerScript.Spawn("location");
        }
    }
}
