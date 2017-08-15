using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBoundary {
    public float xMax, xMin, zMax, zMin;
}

public class EnemyController : MonoBehaviour {

    public float speed;
    public float forwardSpeed;
    public float gameOverSpeed;
    public float movementRandomnes;
    private float lastMove = 0;
    public float tilt;

    public GameObject shot;
    public float shotFrequency;
    public Transform shotSpawn;

    private Rigidbody rb;
    private GameController gameController;
    public EnemyBoundary boundary;

	void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("EnemyController cannot find 'GameController' script.");
        }

        rb = GetComponent<Rigidbody>();
        StartCoroutine(Shooter());

        rb.velocity = transform.forward * forwardSpeed;
	}
	
	void Update () {
        if (gameController.GetGameOver()) {
            movementRandomnes = 0;
            speed = 0;
            forwardSpeed = gameOverSpeed;
            shotFrequency = 10;
        }
	}

    private void FixedUpdate() {
        if(lastMove < Time.time) {
            lastMove = Time.time + movementRandomnes * Random.value;
            speed = speed * -1;
            rb.velocity = (transform.right * speed) + (transform.forward * forwardSpeed);
        }
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
    }

    IEnumerator Shooter() {
        while(!false){
            yield return new WaitForSeconds(shotFrequency);
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }
}
