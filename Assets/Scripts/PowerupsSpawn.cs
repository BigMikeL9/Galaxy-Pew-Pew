using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] powerUpPrefab;


    void Start()
    {
        var randomSpawnRateTime = Random.Range(10, 16);
        InvokeRepeating("SpawnRandomPowerUp", 8, randomSpawnRateTime);
    }

 
    private void SpawnRandomPowerUp()
    {
        Vector3 powerUpPrefabPos = new Vector3(Random.Range(-4, 4), 11, 0);
        GameObject extraLife = Instantiate(powerUpPrefab[Random.Range(0, powerUpPrefab.Length)], powerUpPrefabPos, Quaternion.identity) as GameObject;
        extraLife.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
    }

}
