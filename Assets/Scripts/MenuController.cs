using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    // Main

    public Slider slider;
    public Text gameTime;

    public Slider sensSlider;
    public Text sens;

    public Slider masterVolSlider;
    public Text vol;

    // Pause

    //public Slider pauseSlider;
    //public Text pausePoints;

    //public Slider pauseSensSlider;
    //public Text pauseSens;

    [HideInInspector]
    public int gameTimeNum;
    [HideInInspector]
    public float sensitivity;
    public float defaultSensitivity = 2.0f;
    [HideInInspector]
    public float masterVolume = 100.00f;
    [HideInInspector]
    public float defaultMasterVolume = 100.00f;
    [HideInInspector]
    public float muteMasterVolume = 0.00f;

    void Awake() {
        DontDestroyOnLoad(this);
        Cursor.lockState = CursorLockMode.None;    
    }

    public void exit() {
        Application.Quit();
    }

    public void startGame() {
        gameTimeNum = (int)slider.value;
        sensitivity = sensSlider.value;
        masterVolume = masterVolSlider.value;
        GameObject ac = GameObject.FindGameObjectWithTag("AudioController");
        ac.GetComponent<AudioController>().setCreated(false);
        Destroy(ac);
        SceneManager.LoadScene(1);
    }

    public void startTutorial() {
        gameTimeNum = (int)slider.value;
        sensitivity = sensSlider.value;
        masterVolume = masterVolSlider.value;
        GameObject ac = GameObject.FindGameObjectWithTag("AudioController");
        ac.GetComponent<AudioController>().setCreated(false);
        Destroy(ac);
        SceneManager.LoadScene(2);
    }

    // update points (not needed anymore maybe?)
    public void updatePoints() {
        gameTime.text = slider.value.ToString();
    }

    // update sensitivity
    public void updateSensitivity() {
        sens.text = sensSlider.value.ToString();
        //pauseSens.text = pauseSensSlider.value.ToString();
    }

    // reset to default sensitivity
    public void resetSensitivity() {
        sens.text = defaultSensitivity.ToString();
        //pauseSens.text = defaultSensitivity.ToString();
        sensSlider.value = defaultSensitivity;
        //pauseSensSlider.value = defaultSensitivity;
    }

    public void updateMasterVolume() {
        vol.text = masterVolSlider.value.ToString() + "%";
        masterVolume = masterVolSlider.value;
        AudioListener.volume = masterVolume / 100;
    }

    // reset to default master volume
    public void resetMasterVolume() {
        vol.text = defaultMasterVolume.ToString() + "%";
        masterVolSlider.value = defaultMasterVolume;
        AudioListener.volume = masterVolume / 100;
    }

    // Mute Master Volume
    public void mutingMasterVolume() {
        vol.text = muteMasterVolume.ToString() + "%";
        masterVolSlider.value = muteMasterVolume;
        masterVolume = muteMasterVolume;
        AudioListener.volume = masterVolume / 100;
    }

    void Update () {
        //print(masterVolume);
        //AudioListener.volume = masterVolume / 100;
        //AudioListener.volume = 1;
    }
}
