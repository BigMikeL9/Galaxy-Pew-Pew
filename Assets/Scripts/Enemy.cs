using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int enemyPoints = 50;

    [Header("Enemy Shoot")]
    float shootCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 1.0f;
    [SerializeField] float enemyLaserSpeed = 30f;
    [SerializeField] GameObject enemyLaserPrefab;
    
    [Header("Enemy VFX")]
    [SerializeField] float durationOfExplosionVFX = 1.0f;
    [SerializeField] GameObject explosionParticleVFX;

    [Header("Enemy SFX")]
    [SerializeField] AudioClip enemyShootingSFX;
    [SerializeField] [Range(0, 1)] float enemyShootingSFXVolume = 0.6f; // This gives us a slider in Unity Interface to help us easily control the volume. REMEMBEEEERR.
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] [Range(0, 1)] float enemyDeathSFXVolume = 0.8f;



    private void Start()
    {
        shootCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);


    }

    private void Update()
    {
        CountDownAndShoot();
    }


    private void CountDownAndShoot()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter <= 0f) // When "shotCounter" reaches 0, it enemyLaserPrefab fires.
        {
            fire();
            shootCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots); // We added this line here again to reset the shorcounter everyframe. If we didnt the shotcounter will be decided once when the game starts and be the same throughout, which will result in the enemies shooting nonstop after a certain amount of time set by the "chotcounter" at the start of the game.
        }
    }

    private void fire()
    {
        // We added "GameObject enemyLaser =" to say this is what the projectile is. We also add "as GameObject" to make it clear that we are instantiating our GameObject "as" a "GameObject".
        GameObject enemyLaser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject; 
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
        AudioSource.PlayClipAtPoint(enemyShootingSFX, Camera.main.transform.position, enemyShootingSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other) // other is refering to the "other" thing/gameObject (laser gameObject) that bumped into THIS gameobject that this script is attached to (the enemy).
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // Check player script for explanation. Protects from null or of "damageDealer" script doesnt exist in a gameobject we collide with.
        ProcessHit(damageDealer); // Since we a have a parameter/argument in the method, we HAVE TO pass a paramter/argument when we call the method.
    }

    private void ProcessHit(DamageDealer damageDealer) // Not the easiest way to do it, but helps understand how "paramters/arguments" and "local variables work". 
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        FindObjectOfType<GameSession>().ScoreUpdate(enemyPoints);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionParticleVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosionVFX);
        AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position, enemyDeathSFXVolume); // "AudioSource.PlayClipAtPoint" allows to play the SFX at a particular position, even if the gameObject attached to it is destroyed.
                                                                                                         // "Camera.main.transform.position" plays the deathSFX at the camera instead of the position of the player.
    }

}
