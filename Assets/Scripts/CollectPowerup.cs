using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollectPowerup : MonoBehaviour {

    Image p1_powerup;
    Image p2_powerup;

    public Sprite shrink;
    public Sprite enlarge;
    public Sprite invert;
    public Sprite steal;
    public Sprite slow;

    private AudioSource[] sfx;

    void Start() {
        p1_powerup = GameObject.FindGameObjectWithTag("P1PowerupHud").GetComponent<Image>();
        p2_powerup = GameObject.FindGameObjectWithTag("P2PowerupHud").GetComponent<Image>();

        sfx = this.GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Pickupable")) {
            if (!this.GetComponent<PowerupHandler>().hasPowerup() && !(this.GetComponent<PowerupHandler>().timer > 0)) {
                choosePowerup();
            }
            sfx[4].Play();
            Destroy(c.gameObject);
        }
    }

	void choosePowerup() {
        int powerup = (int) Random.Range(0, 5);

        switch (powerup) {
            case 0:
                this.GetComponent<PowerupHandler>().hasShinkOpponent = true;
                setSprite(shrink);
                break;
            case 1:
                this.GetComponent<PowerupHandler>().hasEnlargeSelf = true;
                setSprite(enlarge);
                break;
            case 2:
                this.GetComponent<PowerupHandler>().hasInvertOpponentControls = true;
                setSprite(invert);
                break;
            case 3:
                this.GetComponent<PowerupHandler>().hasStealScreen = true;
                setSprite(steal);
                break;
            case 4:
                this.GetComponent<PowerupHandler>().hasReduceOpponentSpeed = true;
                setSprite(slow);
                break;
            default:
                break;
        }
    }

    void setSprite(Sprite powerupImage) {
        if (this.transform.CompareTag("Player1")) {
            p1_powerup.enabled = true;
            p1_powerup.sprite = powerupImage;
            p1_powerup.type = Image.Type.Filled;
            p1_powerup.fillMethod = Image.FillMethod.Radial360;
            p1_powerup.fillAmount = 1;
            p1_powerup.transform.GetChild(0).gameObject.SetActive(true);
        } else if (this.transform.CompareTag("Player2")) {
            p2_powerup.enabled = true;
            p2_powerup.sprite = powerupImage;
            p2_powerup.type = Image.Type.Filled;
            p2_powerup.fillMethod = Image.FillMethod.Radial360;
            p2_powerup.fillAmount = 1;
            p2_powerup.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}