using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControllerMenuUI : MonoBehaviour {

    // MENU

        // Top element:
    public GameObject gameSetupButton;  // game setup
    public GameObject modeButton;       // game mode 
    public GameObject settingsButton;   // settings
    public GameObject mainMenuButton;   // main menu
    public GameObject controlsButton;   // controls map

        // Panels:
    public GameObject mainPanel;
    public GameObject modePanel;
    public GameObject gameSetupPanel;
    public GameObject settingsPanel;
    public GameObject controlsMapPanel;

    private AudioSource[] sfx;

    void Start() {
        sfx = this.GetComponents<AudioSource>();
    }

    // Event System
    public void EnterEvent () {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameSetupButton);
    }
    public void ModeEvent() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(modeButton);
    }

    public void SettingsEvent () {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(settingsButton);
    }

    public void backEvent () {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(mainMenuButton);
    }

    public void ControlsMapEvent() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(controlsButton);
    }


    // Updating Event System if changed by mouse (auto select top element) (fix mouse clicking messing with controller input event system)
    void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject == null) {
                if (mainPanel.activeSelf == true) {
                    backEvent();
                }
                else if(modePanel.activeSelf == true) {
                    ModeEvent();
                }
                else if (gameSetupPanel.activeSelf == true) {
                    EnterEvent();
                }
                else if (settingsPanel.activeSelf == true) {
                    SettingsEvent();
                }
                else if (controlsMapPanel.activeSelf == true) {
                    ControlsMapEvent();
                }
            }
        }
    }
}



