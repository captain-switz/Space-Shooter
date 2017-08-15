using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;

    private GameController gameController;
    private PlayerController playerController;

    public int scoreValue;
    public int health;

    public bool fireRateBonus;
    public bool fireSpeedBonus;
    public bool shipSpeedBonus;
    public float bonusTime;
    public int bonusAmount;

    private void Start() {
        /* Get other GameObjects and scripts. */
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if(gameController == null) {
            Debug.Log("Cannot find 'GameController' script.");
        }

        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == ("Boundary")) {
            return;
        }

        if (other.tag == ("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        if (other.tag == ("Shooter"))
        {
            playerController.currentFire--;
        }

        if (health > 1) {
            health--;
        } else {
            Instantiate(explosion, transform.position, transform.rotation);

            if (fireRateBonus) {
                playerController.StartFireRateBonus(bonusAmount, bonusTime);
            }
            if (fireSpeedBonus) {
                playerController.SetFireSpeedBonus(bonusTime);
            }
            if (shipSpeedBonus) {
                playerController.SetShipSpeedBonus(bonusTime);
            }

            if (gameObject.tag == "Enemy") {
                gameController.EnemyDied();
            }

            gameController.AddScore(scoreValue);
            Destroy(gameObject);
        }

        Destroy(other.gameObject);
    }
}
