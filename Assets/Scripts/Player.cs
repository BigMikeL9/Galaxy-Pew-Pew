using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Configuration Paramters
    [Header("Player")]
    public float speed;
    [SerializeField] float xPadding = .5f;
    [SerializeField] float yPadding = .5f;
    public int health;
    //public int numOfLives = 3;
    [SerializeField] Image[] lives; //[0] [1] [2]...
    [SerializeField] GameObject shieldPrefab;
    public bool shield = false;
    Animator animator;

    // [SerializeField] TextMeshProUGUI healthText;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed;
    [SerializeField] float laserFiringPeriod;
    [SerializeField] GameObject deathVFX;

    [Header("Player SFX")]
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.2f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume;
    [SerializeField] AudioClip gotHitSFX;
    [SerializeField] [Range(0, 1)] float gotHitSFXVolume;


    Coroutine shootingCoroutine; // This is just to store the "StartCoroutine(FireContinously());" in, and then use it in "StopCoroutine(....)". Wouldn't work otherwise.
    
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shieldPrefab.gameObject.SetActive(false);
        MoveBoundaries();
        health = lives.Length; // = 3

        // healthText.text = health.ToString();
        // healthText.text = "HP: " + health;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        shieldOn();
        RightAnimation();
        LeftAnimation();
    }


    private void Move()
    {
        // We use "Time.Deltatime" to make our game FRAMRATE independent. Which means that no matter how fast or slow your computer runs, you will havethe same experience.

        // Rick's way of writing the code (Mine and Rick's way do the same thing, but mine is easier to understand):
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        var newPosX = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newPosY = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newPosX, newPosY);


        /* // My way of writing this code, but couldn't us "Mathf.Clamp" with the "transform.translate" method because its a Vector2 and Mathf.Clamp requires a float. 
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontalInput); 
        transform.Translate(Vector2.up * Time.deltaTime * speed * verticalInput); */
    }

    private void RightAnimation()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("PlayerGoingRight", true);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("PlayerGoingRight", false);
        }
    }

    private void LeftAnimation()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("PlayerGoingLeft", true);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("PlayerGoingLeft", false);
        }
    }


    private void MoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding; // Check the "Camera.ViewportToWorldPoint" scripting API to better understand this. We set y and z values to 0 because we dont need them for the xMin. And we added ".x" in the end because we only want to convert the x from viewport space to world space, we dont care about the y and z.
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
    }


    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shootingCoroutine = StartCoroutine(FireContinously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(shootingCoroutine);
            // StopAllCoroutines(); // This method stops all coroutines on this gameObject, which is not good practice since we can have more than one coroutine and we wouldnt want to stop all of them.
        }
    }


    private IEnumerator FireContinously()
    {
        while (true) // "While(true)" means that once "FireContinously()" is called it will keep running because it will always be true, until the coroutine is stopped. 
        {            // The while loop starts with the while keyword, and it must include a boolean conditional expression inside brackets that returns either true or false. It executes the code block until the specified conditional expression returns false.
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject; // "Instantiate" is a OBJECT, so inorder to use it for other things (like RigidBody2D), we need to make it a "GameObject" which is why we added "GameObject laser = ". We also added "as GameObject" in the end just to make to sure that it the instantiated object is changed to a "GameObject" (we don't actually need it there).
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
            yield return new WaitForSeconds(laserFiringPeriod);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // This is line is just to protect us from errors if we collide with something that deosn't have the damagedealerS script. It says that if the player collides with an object that doessnt have a "damagedealer" script/class, then "return" or end (dont execute the rest of the ontrigger2D code).
        ProcessHit(damageDealer);

    }

    

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (!shield) // if shield is equal to false
        {
            health -= damageDealer.GetDamage(); // 3-1=2
                                                // healthText.text = health.ToString();
                                                // healthText.text = "HP: " + health;
            damageDealer.Hit();
            AudioSource.PlayClipAtPoint(gotHitSFX, Camera.main.transform.position, gotHitSFXVolume);
            // We have 0, 1, 2 lives images
            // When health is equal to 3, error happens
            lives[health].gameObject.SetActive(false);  // I want to set this to inactive first and then set it to active from a script on the hp powerup
            // Destroy(lives[health].gameObject); //Destroy(lives[health].gameObject); // Destroy(extraLives[2].gameObject); 
            if (health <= 0)
            {
                Die();
            }
        }
        
    }

    private void shieldOn()
    {
        if (shield)
        {
            shieldPrefab.gameObject.SetActive(true);
        }
        else if (!shield)
        {
            shieldPrefab.gameObject.SetActive(false);
        }
    }

    public int GetHealth()
    {
        return health;
    }


    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }

    public void AddLife()
    {

        if (health == 2 )
        {
            health += 1;
            lives[2].gameObject.SetActive(true);
        }
        if (health == 1 && lives[1].gameObject.activeSelf == false)
        {
            health += 1;
            lives[1].gameObject.SetActive(true);
        }
        if (health >= 3)
        {
            return;
        } 
        
    }

}
