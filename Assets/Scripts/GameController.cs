using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public bool tutorialMode = false;
    float tutorialTimer = 0f;
    public float tutorialMoveMessageLength = 10f;
    string tutorialMoveMessage = "Use Left Stick to move drone\nUse Right Stick to move camera";
    public float tutorialUpAndDownMessageLength = 10f;
    string tutorialUpAndDownMessage = "Press RB to move up\nPress LB to move down";
    public float tutorialBoostMessageLength = 10f;
    string tutorialBoostMessage = "\nPress RT to boost";
    public float tutorialHitTheBallMessageLength = 10f;
    string tutorialHitTheBallMessage = "\nHit The Ball";
    GameObject ball;
    GameObject p1Arrow;
    GameObject p2Arrow;
    public GameObject ballLocator;
    public GameObject ballCrosshair;
    public GameObject pickups;

    int playerTurn = 1;
    public float gravity;
    public Text playerTurnText;
    public Transform pauseMenu;
    public Transform pauseSettingsMenu;
    public Transform pauseControlsMenu;

    public Text tutorialText;

    public GameObject winPanel;
    public Text pointsText;

    // Menu Controller Reference:
    private GameObject pauseController;
    private PauseControllerMenuUI pauseScript;

    public Animator anim;
    public ScoreManager sm;


    //private bool isPaused = false;

    void Awake() {
        GameObject pauseController = GameObject.FindGameObjectWithTag("GameController");
        if (pauseController != null) {
            pauseScript = pauseController.GetComponent<PauseControllerMenuUI>();
        }
        //anim = pointsText.GetComponent<Animator>();
    }

    void Start () {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        Physics.gravity = new Vector3(0, gravity, 0);

        //playerTurnText.text = "Player 1 Turn";

        ball = GameObject.FindGameObjectWithTag("Ball");

        if (sm.someoneHasScored()) {
            tutorialMode = false;
        }

        if (tutorialMode) {
            p1Arrow = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(1).GetChild(1).gameObject;
            p2Arrow = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(1).GetChild(1).gameObject;
            p1Arrow.SetActive(false);
            p2Arrow.SetActive(false);
            ballLocator.SetActive(false);
            ballCrosshair.SetActive(false);
            ball.SetActive(false);
            pickups.SetActive(false);
            tutorialText.text = tutorialMoveMessage;
        } else {
            playerTurnText.text = "Hit the ball!";
            tutorialText.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update () {
        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    Cursor.lockState = CursorLockMode.None;
        //}
        if (tutorialMode) {
            tutorialUpdate();
        } else {
            pickups.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick 1 button 7") || Input.GetKeyDown("joystick 2 button 7")) {
            if (pauseMenu.gameObject.activeInHierarchy || pauseSettingsMenu.gameObject.activeInHierarchy || pauseControlsMenu.gameObject.activeInHierarchy) {
                PauseMenuOff();
            } else {
                PauseMenuOn();
            }
        }
    }

    void tutorialUpdate() {
        tutorialTimer += Time.deltaTime;
        if (tutorialTimer > tutorialMoveMessageLength && tutorialTimer < (tutorialMoveMessageLength + tutorialHitTheBallMessageLength)) {
            tutorialText.text = tutorialUpAndDownMessage;
        } else if (tutorialTimer > (tutorialMoveMessageLength + tutorialBoostMessageLength) && tutorialTimer < (tutorialMoveMessageLength + tutorialBoostMessageLength + tutorialHitTheBallMessageLength)) {
            tutorialText.text = tutorialBoostMessage;
        } else if (tutorialTimer > (tutorialMoveMessageLength + tutorialBoostMessageLength + tutorialHitTheBallMessageLength)) {
            ball.SetActive(true);
            p1Arrow.SetActive(true);
            p2Arrow.SetActive(true);
            ballLocator.SetActive(true);
            ballCrosshair.SetActive(true);
            tutorialText.text = tutorialHitTheBallMessage;
        }
    }

    public void PauseMenuOff () {
        // Pause animation - not requied anymore
        //anim.speed = 1;
        //anim.enabled = false;
        //anim.SetBool("isPaused", false);

        //if (sm.player1Scored == true) {
        //    anim.SetBool("p1Scored", true);
        //    //anim.Play("pointsAnim");
        //}
        //else if (sm.player2Scored == true) {
        //    anim.SetBool("p2Scored", true);
        //}

        if (tutorialMode) {
            tutorialText.gameObject.SetActive(true);
        }

        // Time back
        if (!ball.GetComponent<BallCollisions>().tutorialMessage1Triggered && !ball.GetComponent<BallCollisions>().tutorialMessage2Triggered && !ball.GetComponent<BallCollisions>().tutorialMessage3Triggered) {
            Time.timeScale = 1;
        }
        // Menu off
        pauseMenu.gameObject.SetActive(false);
        pauseSettingsMenu.gameObject.SetActive(false);
        pauseControlsMenu.gameObject.SetActive(false);

        // Turn on win text panel stuff
        winPanel.gameObject.SetActive(true);
        //pointsText.gameObject.SetActive(true);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && sm.animDone == false) {
            pointsText.text = "";
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("pointsAnim") || anim.GetCurrentAnimatorStateInfo(0).IsName("pointsAnimp2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && sm.animDone == false) {
            pointsText.text = sm.pointsPot.ToString();
        }

        pauseScript.updatePauseSensitivity();
    }

    public void PauseMenuOn () {
        // Pause animation (not needed anymore)
        //anim.speed = 0;
        //anim.enabled = true;
        //anim.SetBool("isPaused", true);

        if (tutorialMode) {
            tutorialText.gameObject.SetActive(false);
        }

        // Time Stop
        Time.timeScale = 0;
        
        // Menu On
        pauseMenu.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        pointsText.text = "";

        // Turn off win text panel stuff (NOTE: not turning off animated points text anymore)
        winPanel.gameObject.SetActive(false);
        //pointsText.gameObject.SetActive(false);
    }

    public int getPlayerTurn() {
        return playerTurn;
    }

    public void setPlayerTurn(int player) {
        playerTurn = player;
        //playerTurnText.text = "Player " + player + "'s Turn";
    }

    public bool isPaused() {
        if (pauseMenu.gameObject.activeInHierarchy) {
            return true;
        }
        else if (pauseSettingsMenu.gameObject.activeInHierarchy) {
            return true;
        }
        else if (pauseControlsMenu.gameObject.activeInHierarchy) {
            return true;
        }
        else
            return false;
    }
}
