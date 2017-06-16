using UnityEngine;
using System.Collections;

public class DeliverObject : MonoBehaviour {

    public Spawner spawnerScript;

    bool packageDelivered = false;

    void Start() {
        spawnerScript = GameObject.Find("Game Controller").GetComponent<Spawner>();
    }

    void OnTriggerEnter (Collider c) {
        if (c.gameObject.CompareTag("Pickupable") && !packageDelivered) {
            packageDelivered = true;

            Transform packageTransform = c.gameObject.transform;
            packageTransform.parent = this.transform;
            packageTransform.GetChild(0).gameObject.SetActive(false);

            //Destroy(packageTransform.gameObject);

            spawnerScript.Spawn("package");

            Destroy(this.gameObject);
        }
    }
}
