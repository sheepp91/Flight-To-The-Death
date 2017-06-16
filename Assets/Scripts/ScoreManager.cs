using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

    public int pointsPot;
    int playerPoints;
    public bool player1Scored = false;
    public bool player2Scored = false;
    private bool lastGoalScored = false;
    public bool timeUp = false;
    private bool gameWin = false;

    private float timer = 0f;
    public int gameResetTime = 10;
    public int roundResetTime = 3;

    public int potScore;
    public static int player1Score = 0;
    public static int player2Score = 0;
    //public static int winScore = 5;

    public Text potText;
    public Text playerOneText;
    public Text playerTwoText;
    public Text goalScoreText;
    public Text winText;
    public Text timerResetText;
    public Text pointsText;

    public Animator anim;
    //public Animator animp1;
    //public Animator animp2;


    public bool timedMode = false;
    static float gameTimer = 0;
    public int maxTime;

    public bool animDone;

    void Awake() {
        GameObject menuController = GameObject.FindGameObjectWithTag("MenuController");
        if (menuController != null) {
            maxTime = menuController.GetComponent<MenuController>().gameTimeNum; // If there is an error here, ignore it.
        } else {
            maxTime = 2;
        }

        anim = pointsText.GetComponent<Animator>();
    }

    void Start () {
        pointsPot = 0;
        playerOneText.text = player1Score + "";
        playerTwoText.text = player2Score + "";
        //if (!timedMode) {
        //    goalScoreText.text = "Goal: " + winScore;
        //} else {
        goalScoreText.text = ((int)((maxTime * 60) - gameTimer)).ToString();
        //}

        animDone = false;
    }

    void Update()
    {

        if (!gameWin && !player1Scored && !player2Scored && !timeUp)
        {  ////////////////// TIMES UP AND GOAL SCORED MODE /////////////////////////
           //if (!gameWin && !player1Scored && !player2Scored) {
            gameTimer += Time.deltaTime;
            goalScoreText.text = ((int)((maxTime * 60) - gameTimer)).ToString();
        }
        if ((maxTime * 60) < gameTimer)
        {
            timeUp = true;                                             //////////////////                               /////////////////////////
            //print("Overtime! Last Chance to score points!");
            winText.text = "Overtime! Last Chance to score points!";
            if (lastGoalScored && animDone == true) {                  ////////////////// TIMES UP AND GOAL SCORED MODE /////////////////////////
                //print("LAST GOAL SCORED");                           //////////////////                               /////////////////////////
                gameWin = true;
                if (player1Score > player2Score)
                {
                    winText.text = "Player 1 wins!!";
                }
                else if (player1Score < player2Score)
                {
                    winText.text = "Player 2 wins!!";
                }
                else
                {
                    winText.text = "Tie!!";
                }
            }
        }

        if (gameWin) {
            timer += Time.deltaTime;
            timerResetText.text = "You will be returned to the menu in " + (gameResetTime - (int)timer);
            if (timer > gameResetTime)
            {
                player1Score = 0;
                player2Score = 0;

                GameObject menuController = GameObject.FindGameObjectWithTag("MenuController");
                Destroy(menuController);

                //Destroy audio controller
                GameObject am = GameObject.FindGameObjectWithTag("AudioController");
                am.GetComponent<AudioController>().setCreated(false);
                Destroy(am);

                resetGameTimer();
                SceneManager.LoadScene(0);
            }
        }

        if (player1Scored && !gameWin && !timeUp)
        {
            //winText.text = "Player 1 scored!";
            timer += Time.deltaTime;
            winText.text = "Next round will start in " + (roundResetTime - (int)timer);
            if (timer > roundResetTime)
            {
                winText.text = "";
                timer -= timer;
                player1Scored = false;
                //Debug.Log(player1Score + " p1 score check");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (player2Scored && !gameWin && !timeUp)
        {
            //winText.text = "Player 2 scored!";
            timer += Time.deltaTime;
            winText.text = "Next round will start in " + (roundResetTime - (int)timer);
            if (timer > roundResetTime)
            {
                winText.text = "";
                timer -= timer;
                player2Scored = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (((int)((maxTime * 60) - gameTimer) <= 10) && !timeUp && !player1Scored && !player2Scored) {
            winText.text = (int)((maxTime * 60) - gameTimer) + " seconds left!";
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("pointsAnim") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animDone == false) {
            player1Score += pointsPot++;
            pointsText.text = " ";
            //pointsPot = 0;
            playerOneText.text = player1Score + "";
            animDone = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("pointsAnimp2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animDone == false) {
            player2Score += pointsPot++;
            pointsText.text = " ";
            //pointsPot = 0;
            playerTwoText.text = player2Score + "";
            animDone = true;
        }
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animDone == false) {
        //    pointsText.text = " ";
        //}
    }

    public void incrementPointsPot () {
        pointsPot++;
        potText.text = "Pot: " + pointsPot;
    }

    public void addToPlayer1Total() {
        pointsText.text = pointsPot.ToString();
        //player1Score += pointsPot++;
        //pointsPot = 0;

        playerOneText.text = player1Score + "";
        potText.text = "Pot: " + pointsPot;

        player1Scored = true;

        //pointsText.gameObject.SetActive(true);

        // score animation
        anim.SetBool("p1Scored", true);
        potText.text = "Pot: ";
    }

    public void addToPlayer2Total () {
        pointsText.text = pointsPot.ToString();
        //player2Score += pointsPot++;
        //pointsPot = 0;

        playerTwoText.text = player2Score + "";
        potText.text = "Pot: " + pointsPot;

        player2Scored = true;

        // score animation
        anim.SetBool("p2Scored", true);
        potText.text = "Pot: ";
    }

    public void setScoresToZero() {
        player1Score = 0;
        player2Score = 0;
    }

    public void resetGameTimer() {
        gameTimer = 0f;
    }

    public bool gameFinished() {
        return gameWin;
    }

    public void setLastGoalScored(bool lastGoalScored) {
        this.lastGoalScored = lastGoalScored;
    }

    public bool someoneHasScored() {
        return player1Score > 0 || player2Score > 0;
    }
}
