using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseControllerMenuUI : MonoBehaviour
{

    public ScoreManager sm;

    // GAME PAUSE MENU

    // Top element:
    public GameObject topButton;
    public GameObject settingsTopButton;
    public GameObject controlsTopButton;

    // Panels:
    public GameObject pausePanel;
    public GameObject pauseSettingsPanel;
    public GameObject pauseControlsPanel;

    // Menu Controller Reference:
    private GameObject menuController;
    private MenuController mScript;

    // Pause: 
    public Slider pauseSensSlider;
    public Text pauseSens;

    public Slider pauseMasterVolSlider;
    public Text pauseVol;

    void Awake() {
        GameObject menuController = GameObject.FindGameObjectWithTag("MenuController");
        if (menuController != null) {
            mScript = menuController.GetComponent<MenuController>();
        }
    }

    void Start () {
        if (menuController != null) {
            pauseSensSlider.value = mScript.sensitivity;
            pauseMasterVolSlider.value = mScript.masterVolume;
        }
    }

    // Event System Changes
    public void pauseEvent() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(topButton);
    }

    public void pauseSettingsEvent() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(settingsTopButton);
    }

    public void pauseControlsEvent() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(controlsTopButton);
    }

    // Returning to menu
    public void returnToMenu() {
        // Destroy old menu controller
        GameObject menuController = GameObject.FindGameObjectWithTag("MenuController");
        Destroy(menuController);

        // Reset both players scores
        sm.setScoresToZero();

        // Reset pot score
        sm.potScore = 0;

        // Reset Timer
        sm.resetGameTimer();

        GameObject ac = GameObject.FindGameObjectWithTag("AudioController");
        ac.GetComponent<AudioController>().setCreated(false);
        Destroy(ac);

        // Load Menu (0)
        SceneManager.LoadScene(0);
    }

    void Update() {
        // Fix - If click with mouse off highlighted/selectable button, auto flicker to top element of active panel
        if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject == null) {
            // Pause Panel on -> select top element (ContinueButton)
            if (pausePanel.activeSelf == true) {
                pauseEvent();
            }
            // Pause Settings Panel on -> select top element (SensitivitySlider)
            else if (pauseSettingsPanel.activeSelf == true) {
                pauseSettingsEvent();
                //if (mScript.sensitivity != 2)  {
                updatePauseSensitivity();
                updatePauseMasterVolume();
                //}
            }
            // Controls Map Menu on -> select top element (backButton)
            else if (pauseControlsPanel.activeSelf == true) {
                pauseControlsEvent();
            }
        }
    }

    public void updatePauseSensitivity() {
        pauseSens.text = pauseSensSlider.value.ToString();
        //print("You nearly got there");
        //if (menuController != null) {
            mScript.sensitivity = pauseSensSlider.value;
            //print("You got there");
        //}
    }

    public void resetPauseSensitivity() {
        pauseSens.text = mScript.defaultSensitivity.ToString();
        pauseSensSlider.value = mScript.defaultSensitivity;
        mScript.sensitivity = pauseSensSlider.value;
    }

    public void updatePauseMasterVolume() {
        pauseVol.text = pauseMasterVolSlider.value.ToString() + "%";
        mScript.masterVolume = pauseMasterVolSlider.value;
        AudioListener.volume = mScript.masterVolume / 100;
    }

    public void mutingPauseMasterVolume() {
        pauseVol.text = mScript.muteMasterVolume.ToString() + "%";
        pauseMasterVolSlider.value = mScript.muteMasterVolume;
        mScript.masterVolume = mScript.muteMasterVolume;
        AudioListener.volume = mScript.masterVolume / 100;
    }
    public void resetPauseMasterVolume() {
        pauseVol.text = mScript.defaultMasterVolume.ToString() + "%";
        pauseMasterVolSlider.value = mScript.defaultMasterVolume;
        AudioListener.volume = mScript.masterVolume / 100;
    }
}
