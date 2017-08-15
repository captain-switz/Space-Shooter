using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    private PlayerController playerController;
    private GameController gameController;

    private void Start() {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null) {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
        if (playerController == null) {
            Debug.Log("Cannot find 'PlayerController' script.");
        }

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }

    }

    void OnTriggerExit(Collider other) {
        Destroy(other.gameObject);
        if (other.tag == ("Shooter")) {
            playerController.currentFire--;
        }
        if (other.tag == ("Enemy")) {
            gameController.EnemyDied();
        }
    }
}
