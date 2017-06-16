using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallCollisions : MonoBehaviour {

    [HideInInspector]
    public bool tutorialMessage1Triggered = false;
    [HideInInspector]
    public bool tutorialMessage2Triggered = false;
    [HideInInspector]
    public bool tutorialMessage3Triggered = false;
    bool tutorialPlayer1Turn = false;
    bool tutorialPlayer2Turn = false;

    public GameObject collisionPuff;

    public Material p1TurnMaterial;
    public Material p2TurnMaterial;

    public Material greenGround;
    public Material purpleGround;

    public Material greenFlower;
    public Material purpleFlower;
    public Material greenMushroom;
    public Material purpleMushroom;

    GameObject[] flowers;
    GameObject[] mushrooms;

    public GameObject ground1;
    public GameObject ground2;

    public ExplodeOnLose player1;
    public ExplodeOnLose player2;

    public float winSlowDown = 0.6f;

    private GameObject gameController;

    private GameController gcScript;
    private ScoreManager smScript;
    private bool firstHit = true;

    // To undo "Hit the Ball" on wintext after first hit
    public Text playerTurnText;

    private AudioSource[] sfx;


    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gcScript = gameController.GetComponent<GameController>();
        smScript = gameController.GetComponent<ScoreManager>();

        sfx = this.GetComponents<AudioSource>();

        flowers = GameObject.FindGameObjectsWithTag("Flower");
        mushrooms = GameObject.FindGameObjectsWithTag("Mushroom");
    }

    //void Update() {
    //    Vector3 down = Vector3.down * 5f;
    //    Debug.DrawRay(transform.position, down, Color.green);
    //
    //    //RaycastHit hit;
    //    //
    //    //if (Physics.Raycast(transform.position, down, out hit, 5f)) {
    //    //    if (hit.transform.CompareTag("Ground")) {
    //    //        Time.timeScale = 0.5f;
    //    //        print("hit ground");
    //    //    }
    //    //} else {
    //    //    Time.timeScale = 1f;
    //    //}
    //}

    void Update() {
        if (tutorialMessage1Triggered) {
            if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.Return)) {
                tutorialMessage1Triggered = false;
                tutorialMessage2Triggered = true;
            }
        } else if (tutorialMessage2Triggered) {
            gcScript.tutorialText.text = "If the ball is your color when it hits the ground your opponent gets the pot!\nPress A to continue";
            if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.Return)) {
                tutorialMessage2Triggered = false;
                tutorialMessage3Triggered = true;
            }
        } else if (tutorialMessage3Triggered) {
            gcScript.tutorialText.text = "The flaming orbs around the arena are powerups.\nTry them out!\nPress A to continue";
            if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.Return)) {
                gcScript.tutorialText.text = "";
                gcScript.tutorialText.transform.parent.gameObject.SetActive(false);
                Time.timeScale = 1f;
                gcScript.tutorialMode = false;
                tutorialMessage3Triggered = false;
            }
        }
    }

	void OnCollisionEnter (Collision c) {
        sfx[0].Play(); //Knock sound effect

        if (firstHit) {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.useGravity = true;
            firstHit = false;
            smScript.incrementPointsPot();
            playerTurnText.text = " ";
            if (c.gameObject.CompareTag("Player1")) {
                playerOneHit();
                tutorialPlayer2Turn = true;
            } else if (c.gameObject.CompareTag("Player2")) {
                playerTwoHit();
                tutorialPlayer1Turn = true;
            }

            if (gcScript.tutorialMode && (!tutorialMessage1Triggered || !tutorialMessage2Triggered)) {
                tutorialMessage1Triggered = true;
                gcScript.tutorialMode = false;
                gcScript.tutorialText.text = "The Ball and Ground will change color to the drone that needs to hit it next!\nPress A to Continue";
                Time.timeScale = 0f;
            }
        }
        if (c.gameObject.tag.Contains("Player") && (!smScript.player1Scored && !smScript.player2Scored) && !smScript.gameFinished()) {
            if ((c.gameObject.CompareTag("Player1") && (gcScript.getPlayerTurn() == 1)) || (c.gameObject.CompareTag("Player2") && (gcScript.getPlayerTurn() == 2))) {
                smScript.incrementPointsPot();
                if (c.gameObject.CompareTag("Player1")) {
                    playerOneHit();
                } else if (c.gameObject.CompareTag("Player2")) {
                    playerTwoHit();
                }
            }
        } else if (c.gameObject.CompareTag("Ground") && !smScript.gameFinished()) {
            if (smScript.timeUp) {                                              //////////////////                               /////////////////////////
                smScript.setLastGoalScored(true);                               ////////////////// TIMES UP AND GOAL SCORED MODE /////////////////////////
            }                                                                   //////////////////                               /////////////////////////
            if (gcScript.getPlayerTurn() == 1) {
                smScript.addToPlayer2Total();
                player1.GetComponent<ExplodeOnLose>().explode();
            } else {
                smScript.addToPlayer1Total();
                player2.GetComponent<ExplodeOnLose>().explode();
            }
            Time.timeScale = winSlowDown;
        }
        foreach (ContactPoint contact in c.contacts) {
            Instantiate(collisionPuff, contact.point, new Quaternion(0, 0, 0, 0));
        }
    }

    void playerOneHit() {
        //Player 2 turn
        gcScript.setPlayerTurn(2);
        //Change material
        this.GetComponent<Renderer>().material = p2TurnMaterial;
        ground1.GetComponent<Renderer>().material = purpleGround;
        ground2.GetComponent<Renderer>().material = purpleGround;

        foreach (GameObject mushroom in mushrooms) {
            mushroom.GetComponent<Renderer>().material = purpleMushroom;
        }
        foreach (GameObject flower in flowers) {
            flower.GetComponent<Renderer>().material = purpleFlower;
        }
    }

    void playerTwoHit() {
        //Player 1 turn
        gcScript.setPlayerTurn(1);
        //Change material
        this.GetComponent<Renderer>().material = p1TurnMaterial;
        ground1.GetComponent<Renderer>().material = greenGround;
        ground2.GetComponent<Renderer>().material = greenGround;

        foreach (GameObject mushroom in mushrooms) {
            mushroom.GetComponent<Renderer>().material = greenMushroom;
        }
        foreach (GameObject flower in flowers) {
            flower.GetComponent<Renderer>().material = greenFlower;
        }
    }
}
