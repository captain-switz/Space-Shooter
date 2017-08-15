using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed;
    public bool addGlobalSpeed;
    public bool addBonusSpeed;

    private GameController gameController;
    private PlayerController playerController;
    private Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }

        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }

        if (addGlobalSpeed) {
            rb.velocity = transform.forward * (speed + gameController.globalSpeed);
        } else if (addBonusSpeed) {
            rb.velocity = transform.forward * (speed + playerController.fireSpeedBonus);
        } else {
            rb.velocity = transform.forward * speed;
        }

	}
	
	void Update () {
		
	}
}
