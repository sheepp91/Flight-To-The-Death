using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject package;
    public GameObject destination;
    public GameObject locations;
    public ArrowToDestination destArrowScript;

    private int numChild;

    void Start () {
        numChild = locations.transform.childCount;
    }

    public void Spawn (string objToSpawn) {
        //int rand = Random.Range(0, numChild);
        //Debug.Log(rand);
        Transform loc = locations.transform.GetChild(Random.Range(0, numChild));
        GameObject obj;
        
        if (objToSpawn == "package") {
            obj = Instantiate(package, loc) as GameObject;
        } else { //if (objToSpawn == "destination") {
            obj = Instantiate(destination, loc) as GameObject;
        }

        destArrowScript.SetDestination(obj.transform);
    }
}
