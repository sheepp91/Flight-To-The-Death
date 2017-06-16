using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    AudioSource[] music;
    bool loopStarted = false;

    private static bool created = false;

	void Start () {
        if (!created) {
            // this is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            created = true;
        } else {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
        music = GetComponents<AudioSource>();
    }
	
	void Update () {
        if (!music[0].isPlaying && !loopStarted) {
            loopStarted = true;
            music[1].Play();
        }

    }

    public void setCreated(bool setVal) {
        created = setVal;
    }
}
