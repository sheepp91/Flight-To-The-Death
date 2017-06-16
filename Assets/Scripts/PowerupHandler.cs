using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerupHandler : MonoBehaviour {

    enum Attacks {
        INVERT,
        SHRINK,
        SLOW
    };

    public float shinkSize;
    public float enlargeSize;

    private Transform p1_transform;
    private Transform p2_transform;
    private Transform p1_modelTransform;
    private Transform p2_modelTransform;
    private Camera p1_camera;
    private Camera p2_camera;
    private Image p1_powerup;
    private Image p2_powerup;
    private GameObject p1_systemOverride;
    private GameObject p2_systemOverride;

    public bool hasShinkOpponent = false;
    public bool hasStealScreen = false;
    public bool hasInvertOpponentControls = false;
    public bool hasEnlargeSelf = false;
    public bool hasReduceOpponentSpeed = false;
    
    [HideInInspector]
    public bool p1_sizeChanged = false;
    [HideInInspector]
    public bool p2_sizeChanged = false;
    [HideInInspector]
    public bool p1_reduceSpeed = false;
    [HideInInspector]
    public bool p2_reduceSpeed = false;
    bool stoleScreenTimer = false;

    [HideInInspector]
    public bool p1_attack = false;
    [HideInInspector]
    public bool p2_attack = false;
    bool p1_stoleScreen = false;
    bool p2_stoleScreen = false;
    bool screenBackToNormal = false;
    float lerpSpeed = 5f;

    public float reduceSpeedAmount = 25;

    [HideInInspector]
    public float timer = 0;
    public float screenStealTime;
    public float sizeChangedTime;
    public float slowDownOpponentTime;
    public float invertControlsTime;

    Vector3 p1_originalScale;
    Vector3 p2_originalScale;

    private Rect p1_originalCamRect = new Rect(0f, 0f, 0.5f, 1f);
    private Rect p2_originalCamRect = new Rect(0.5f, 0f, 0.5f, 1f);
    private Rect p1_largeCamRect = new Rect(0f, 0f, 0.75f, 1f);
    private Rect p2_largeCamRect = new Rect(0.25f, 0f, 0.75f, 1f);
    private Rect p1_smallCamRect = new Rect(0f, 0f, 0.25f, 1f);
    private Rect p2_smallCamRect = new Rect(0.75f, 0f, 0.25f, 1f);

    private AudioSource[] sfx;

    void Start () {
        p1_transform = GameObject.FindGameObjectWithTag("Player1").transform;
        p2_transform = GameObject.FindGameObjectWithTag("Player2").transform;

        p1_modelTransform = p1_transform.GetChild(0);
        p2_modelTransform = p2_transform.GetChild(0);
        
        p1_camera = p1_transform.GetChild(1).GetChild(0).GetComponent<Camera>();
        p2_camera = p2_transform.GetChild(1).GetChild(0).GetComponent<Camera>();

        p1_originalScale = p1_modelTransform.localScale;
        p2_originalScale = p2_modelTransform.localScale;

        p1_powerup = GameObject.FindGameObjectWithTag("P1PowerupHud").GetComponent<Image>();
        p2_powerup = GameObject.FindGameObjectWithTag("P2PowerupHud").GetComponent<Image>();

        p1_systemOverride = GameObject.FindGameObjectWithTag("P1SystemOverride");
        p2_systemOverride = GameObject.FindGameObjectWithTag("P2SystemOverride");

        sfx = this.GetComponents<AudioSource>();
    }

    void Update () {
        checkPlayer1();
        checkPlayer2();

        checkStoleScreen();
    }

    void checkPlayer1() {
        if (this.CompareTag("Player1")) {
            if (Input.GetKeyDown("f") || Input.GetKeyDown("joystick 1 button 1")) {
                checkPowerups(1);
            }
            if (p1_sizeChanged) {
                if (timer < sizeChangedTime) {
                    timer += Time.deltaTime;
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.fillAmount = ((sizeChangedTime - timer) / sizeChangedTime);
                    } else {
                        p1_powerup.fillAmount = ((sizeChangedTime - timer) / sizeChangedTime);
                    }
                } else {
                    timer = 0f;
                    p1_modelTransform.localScale = p1_originalScale;
                    turnOffP1SystemOverride();
                    p1_sizeChanged = false;
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.enabled = false;
                        p2_transform.GetComponent<PowerupHandler>().p2_attack = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p1_powerup.enabled = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            if (p1_transform.GetComponent<DroneMove>().p1Inverted) {
                if (timer < invertControlsTime) {
                    timer += Time.deltaTime;
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.fillAmount = ((invertControlsTime - timer) / invertControlsTime);
                    } else {
                        p1_powerup.fillAmount = ((invertControlsTime - timer) / invertControlsTime);
                    }
                } else {
                    timer = 0f;
                    p1_transform.GetComponent<DroneMove>().p1Inverted = false;
                    p1_camera.GetComponent<MouseLook>().p1Inverted = false;
                    turnOffP1SystemOverride();
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.enabled = false;
                        p2_transform.GetComponent<PowerupHandler>().p2_attack = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p1_powerup.enabled = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            if (p1_reduceSpeed) {
                if (timer < slowDownOpponentTime) {
                    timer += Time.deltaTime;
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.fillAmount = ((slowDownOpponentTime - timer) / slowDownOpponentTime);
                    } else {
                        p1_powerup.fillAmount = ((slowDownOpponentTime - timer) / slowDownOpponentTime);
                    }
                } else {
                    timer = 0f;
                    p1_reduceSpeed = false;
                    p2_transform.GetComponent<DroneMove>().speed += reduceSpeedAmount;
                    turnOffP1SystemOverride();
                    if (p2_transform.GetComponent<PowerupHandler>().p2_attack) {
                        p2_powerup.enabled = false;
                        p2_transform.GetComponent<PowerupHandler>().p2_attack = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p1_powerup.enabled = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void checkPlayer2() {
        if (this.CompareTag("Player2")) {
            if (Input.GetKeyDown("joystick 2 button 1")) {
                checkPowerups(2);
            }
            if (p2_sizeChanged) {
                if (timer < sizeChangedTime) {
                    timer += Time.deltaTime;
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.fillAmount = ((sizeChangedTime - timer) / sizeChangedTime);
                    } else {
                        p2_powerup.fillAmount = ((sizeChangedTime - timer) / sizeChangedTime);
                    }
                } else {
                    timer = 0f;
                    p2_modelTransform.localScale = p2_originalScale;
                    p2_sizeChanged = false;
                    turnOffP2SystemOverride();
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.enabled = false;
                        p1_transform.GetComponent<PowerupHandler>().p1_attack = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p2_powerup.enabled = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            if (p2_transform.GetComponent<DroneMove>().p2Inverted) {
                if (timer < invertControlsTime) {
                    timer += Time.deltaTime;
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.fillAmount = ((invertControlsTime - timer) / invertControlsTime);
                    } else {
                        p2_powerup.fillAmount = ((invertControlsTime - timer) / invertControlsTime);
                    }
                } else {
                    timer = 0f;
                    p2_transform.GetComponent<DroneMove>().p2Inverted = false;
                    p2_camera.GetComponent<MouseLook>().p2Inverted = false;
                    turnOffP2SystemOverride();
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.enabled = false;
                        p1_transform.GetComponent<PowerupHandler>().p1_attack = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p2_powerup.enabled = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            if (p2_reduceSpeed) {
                if (timer < slowDownOpponentTime) {
                    timer += Time.deltaTime;
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.fillAmount = ((slowDownOpponentTime - timer) / slowDownOpponentTime);
                    } else {
                        p2_powerup.fillAmount = ((slowDownOpponentTime - timer) / slowDownOpponentTime);
                    }
                } else {
                    timer = 0f;
                    p2_reduceSpeed = false;
                    p2_transform.GetComponent<DroneMove>().speed += reduceSpeedAmount;
                    turnOffP2SystemOverride();
                    if (p1_transform.GetComponent<PowerupHandler>().p1_attack) {
                        p1_powerup.enabled = false;
                        p1_transform.GetComponent<PowerupHandler>().p1_attack = false;
                        p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    } else {
                        p2_powerup.enabled = false;
                        p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void checkStoleScreen() {
        if (!stoleScreenTimer) {
            if (p1_stoleScreen) {
                stealingScreen(p1_largeCamRect, p2_smallCamRect);
            } else if (p2_stoleScreen) {
                stealingScreen(p1_smallCamRect, p2_largeCamRect);
            } else if (screenBackToNormal) {
                stealingScreen(p1_originalCamRect, p2_originalCamRect);
            }
        }

        if (stoleScreenTimer) {
            if (timer < screenStealTime) {
                timer += Time.deltaTime;
                if (this.CompareTag("Player1") && p1_stoleScreen) {
                    p1_powerup.fillAmount = ((screenStealTime - timer) / screenStealTime);
                } else if (this.CompareTag("Player2") && p2_stoleScreen) {
                    p2_powerup.fillAmount = ((screenStealTime - timer) / screenStealTime);
                }
            } else {
                timer = 0f;
                screenBackToNormal = true;

                if (p1_stoleScreen) {
                    p1_powerup.enabled = false;
                    p1_powerup.transform.GetChild(0).gameObject.SetActive(false);
                } else if (p2_stoleScreen) {
                    p2_powerup.enabled = false;
                    p2_powerup.transform.GetChild(0).gameObject.SetActive(false);
                }

                p1_stoleScreen = false;
                p2_stoleScreen = false;
                stoleScreenTimer = false;
            }
        }
    }

    void checkPowerups (int player) {
        if (hasShinkOpponent) {                                 // Shrink Opponent
            hasShinkOpponent = false;
            sfx[2].Play(); // attack alarm
            if (player == 1) {
                resizePlayer(p2_modelTransform, shinkSize);
                p2_transform.GetComponent<PowerupHandler>().p2_sizeChanged = true;
                p1_attack = true;
                turnOnP2SystemOverride(Attacks.SHRINK);
            } else if (player == 2) {
                resizePlayer(p1_modelTransform, shinkSize);
                p1_transform.GetComponent<PowerupHandler>().p1_sizeChanged = true;
                p2_attack = true;
                turnOnP1SystemOverride(Attacks.SHRINK);
            }
        } else if (hasEnlargeSelf) {                            // Enlarge Self
            hasEnlargeSelf = false;
            if (player == 1) {
                resizePlayer(p1_modelTransform, enlargeSize);
                p1_sizeChanged = true;
            } else if (player == 2) {
                resizePlayer(p2_modelTransform, enlargeSize);
                p2_sizeChanged = true;
            }
        } else if (hasStealScreen) {                            // Steal Screen
            hasStealScreen = false;
            sfx[2].Play(); // attack alarm
            if (player == 1) {
                if (p2_stoleScreen) {
                    p2_stoleScreen = false;
                }
                p1_stoleScreen = true;
            } else if (player == 2) {
                if (p1_stoleScreen) {
                    p1_stoleScreen = false;
                }
                p2_stoleScreen = true;
            }
        } else if (hasInvertOpponentControls) {                 // Invert Opponent Controls
            hasInvertOpponentControls = false;
            sfx[2].Play(); // attack alarm
            if (player == 1) {
                p2_transform.GetComponent<DroneMove>().p2Inverted = true;
                p2_camera.GetComponent<MouseLook>().p2Inverted = true;
                p1_attack = true;
                turnOnP2SystemOverride(Attacks.INVERT);
            } else if (player == 2) {
                p1_transform.GetComponent<DroneMove>().p1Inverted = true;
                p1_camera.GetComponent<MouseLook>().p1Inverted = true;
                p2_attack = true;
                turnOnP1SystemOverride(Attacks.INVERT);
            }
        } else if (hasReduceOpponentSpeed) {                    // Steal Boost
            hasReduceOpponentSpeed = false;
            sfx[2].Play(); // attack alarm
            if (player == 1) {
                p2_transform.GetComponent<PowerupHandler>().p2_reduceSpeed = true;
                p2_transform.GetComponent<DroneMove>().speed -= reduceSpeedAmount;
                p1_attack = true;
                turnOnP2SystemOverride(Attacks.SLOW);
            } else if (player == 2) {
                p1_transform.GetComponent<PowerupHandler>().p1_reduceSpeed = true;
                p1_transform.GetComponent<DroneMove>().speed -= reduceSpeedAmount;
                p2_attack = true;
                turnOnP1SystemOverride(Attacks.SLOW);
            }
        } else {
            Debug.Log("No boosts");
        }
    }

    void resizePlayer (Transform player, float resizeFactor) {
        Vector3 newScale = player.localScale;
        newScale.x *= resizeFactor;
        newScale.y *= resizeFactor;
        newScale.z *= resizeFactor;
        player.localScale = newScale;
    }

    void stealingScreen(Rect p1NewScreenSize, Rect p2NewScreenSize) {
        p1_camera.rect = new Rect(0f, 0f, Mathf.Lerp(p1_camera.rect.width, p1NewScreenSize.width, Time.deltaTime * lerpSpeed), 1f); //p1NewScreenSize;
        p2_camera.rect = new Rect(Mathf.Lerp(p2_camera.rect.x, p2NewScreenSize.x, Time.deltaTime * lerpSpeed), 0f, Mathf.Lerp(p2_camera.rect.width, p2NewScreenSize.width, Time.deltaTime * lerpSpeed), 1f); //p2NewScreenSize;

        if (((p1_camera.rect.width >= p1NewScreenSize.width - 0.0005f) && (p1_camera.rect.width <= p1NewScreenSize.width + 0.0005f)) &&
            ((p2_camera.rect.width >= p2NewScreenSize.width - 0.0005f) && (p2_camera.rect.width <= p2NewScreenSize.width + 0.0005f))) {
            if (p1_stoleScreen || p2_stoleScreen) {
                stoleScreenTimer = true;
            }
            
            screenBackToNormal = false;
        }
    }

    void turnOnP1SystemOverride(Attacks type) {
        p1_systemOverride.transform.GetChild(0).gameObject.SetActive(true);
        p1_systemOverride.transform.GetChild(1).gameObject.SetActive(true);
        p1_systemOverride.transform.GetChild(2).gameObject.SetActive(true);
        switch (type) {
            case Attacks.INVERT:
                p1_systemOverride.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case Attacks.SHRINK:
                p1_systemOverride.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case Attacks.SLOW:
                p1_systemOverride.transform.GetChild(5).gameObject.SetActive(true);
                break;
        }
    }

    void turnOnP2SystemOverride (Attacks type) {
        p2_systemOverride.transform.GetChild(0).gameObject.SetActive(true);
        p2_systemOverride.transform.GetChild(1).gameObject.SetActive(true);
        p2_systemOverride.transform.GetChild(2).gameObject.SetActive(true);
        switch (type) {
            case Attacks.INVERT:
                p2_systemOverride.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case Attacks.SHRINK:
                p2_systemOverride.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case Attacks.SLOW:
                p2_systemOverride.transform.GetChild(5).gameObject.SetActive(true);
                break;
        }
    }

    void turnOffP1SystemOverride() {
        p1_systemOverride.transform.GetChild(0).gameObject.SetActive(false);
        p1_systemOverride.transform.GetChild(1).gameObject.SetActive(false);
        p1_systemOverride.transform.GetChild(2).gameObject.SetActive(false);
        p1_systemOverride.transform.GetChild(3).gameObject.SetActive(false);
        p1_systemOverride.transform.GetChild(4).gameObject.SetActive(false);
        p1_systemOverride.transform.GetChild(5).gameObject.SetActive(false);
    }

    void turnOffP2SystemOverride() {
        p2_systemOverride.transform.GetChild(0).gameObject.SetActive(false);
        p2_systemOverride.transform.GetChild(1).gameObject.SetActive(false);
        p2_systemOverride.transform.GetChild(2).gameObject.SetActive(false);
        p2_systemOverride.transform.GetChild(3).gameObject.SetActive(false);
        p2_systemOverride.transform.GetChild(4).gameObject.SetActive(false);
        p2_systemOverride.transform.GetChild(5).gameObject.SetActive(false);
    }

    public bool hasPowerup() {
        return hasShinkOpponent || hasStealScreen || hasInvertOpponentControls || hasEnlargeSelf || hasReduceOpponentSpeed || p1_attack || p2_attack || p1_stoleScreen || p2_stoleScreen;
    }
}
