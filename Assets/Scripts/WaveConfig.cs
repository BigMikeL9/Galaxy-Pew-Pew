using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")] // This is how you name the AssetMenu option that you have created, which shows up when you right click and choose create.
public class WaveConfig : ScriptableObject  // This is how you make a scriptableObject, by chnaging the "monobehavior" class to a "scriptableObject" class. Then Naming it (line above). Then you'll find it in the create option after right clicking.
{                                           // Scriptable objects needs a scripts attched to them that tells them what to do. Check API.


    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject enemyPathPrefab;
    [Tooltip("Time between spawns is randomized")]
    [SerializeField] float minTimeBetweenSpawns = 0.2f;
    [SerializeField] float maxTimeBetweenSpawns = 1f;
    [SerializeField] float minSpeed = 2f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] int numberOfEnemies = 5;


    public GameObject GetEnemyPrefab() // We do this to avoid making the "{serializedFields]" public, which will allow other scripts to change their values (which is not good practice).
    {
        var enemyPrefabsIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[enemyPrefabsIndex];
    }

    public List<Transform> GetWaypoints()
    {
        var waveWayPoints = new List<Transform>();
        foreach (Transform child in enemyPathPrefab.transform) // We are adding a foreach loop inorder to add each child's transform (position) under "enemyPathPrefab" to the new "waveWayPoints" list.
        {
            waveWayPoints.Add(child); // This means that we are adding the child (or children under the "enemyPathPrefab" which we added from the "paths" folder in unity) to the LIST called "waveWayPoints" that we created in line 25.
        }

        return waveWayPoints;
    }

    public float GetTimeBetweenSpawns()
    {
        var timeBetweenSpawnsRandomized = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        return timeBetweenSpawnsRandomized;
    }


    public float GetenemyMoveSpeed()
    {
        var enemyMoveSpeed = Random.Range(minSpeed, maxSpeed);
        return enemyMoveSpeed;
    }

    public int GetnumberOfEnemies()
    {
        return numberOfEnemies;
    }


}
