using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    //For controlling the player
    public float speed;
    public float tilt;
    private float yOffset;
    public Boundary boundary;

    //For firing shots
    public int fireRate;
    public GameObject shot;
    public Transform shotSpawn;
    public int currentFire;


    public GUIText bonusAlert;
    public float fireSpeedBonus;

    public GameObject explosion;

    //Rigidbody
    private Rigidbody rb;
    private GameController gameController;

    void Start() {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("PlayerController cannot find 'GameController' script.");
        }
        yOffset = -Input.acceleration.y;
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1") && currentFire < fireRate) {
            currentFire++;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }

    void FixedUpdate() {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        float moveHorizontal = Input.acceleration.x;
        float moveVertical = Input.acceleration.y + yOffset;


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "EnemyShooter") {
            Instantiate(explosion, transform.position, transform.rotation);
            gameController.GameOver();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator FireRateBonus(int amount, float duration) {
        fireRate = fireRate + amount;
        bonusAlert.text = "Fire rate increased.";
        yield return new WaitForSeconds(duration);
        fireRate = fireRate - amount;
        bonusAlert.text = "Fire rate decreased.";
        yield return new WaitForSeconds(1.5f);
        bonusAlert.text = "";

    }
    public void StartFireRateBonus(int amount, float duration) {
        StartCoroutine(FireRateBonus(amount, duration));
    }

    IEnumerator FireSpeedBonus(float amount) {
        fireSpeedBonus = amount + fireSpeedBonus;
        bonusAlert.text = "Bolt speed increased.";
        yield return new WaitForSeconds(1.5f);
        bonusAlert.text = "";
    }
    public void SetFireSpeedBonus(float amount) {
        StartCoroutine(FireSpeedBonus(amount));
    }

    IEnumerator ShipSpeedBonus(float amount) {
        speed+= amount;
        bonusAlert.text = "Ship speed increased.";
        yield return new WaitForSeconds(1.5f);
        bonusAlert.text = "";
    }
    public void SetShipSpeedBonus(float amount) {
        StartCoroutine(ShipSpeedBonus(amount));
    }
}
