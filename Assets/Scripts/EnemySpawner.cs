using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    IEnumerator Start() // We are making the start method a coroutine only so that we can loop the "SpawnAllWaves()" over and over by using the yield retur. Otherwise it won't work.
    {
        do             // This is called a do-while loop that allows us to loop all waves over and over.
        {                                                                        
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping); // this means that looping is set to TRUE. Don't ask how.

    }



    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            waveIndex = Random.Range(0, waveConfigs.Count);
            var currentWave = waveConfigs[waveIndex]; // This line just says which wave we are going to spawn
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave)); // This line of code spawns the waves (with their enemies) attached to the "EnemySpawner" gameObject, one after another (AFTER EACH WAVE FINISHES SPAWNING ALL OF ITS ENEMIES).
        }
    }


                                                                      // Varaiables in parameters are always DIFFERENT from the classes set at the top, EVEN if they have the same name and type.****
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig) // This is to spawn the number of enemies in a single wave.
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetnumberOfEnemies(); enemyCount++) // If we dont add the for-loop then only one enemyPrefab will spawn, and not the "GetnumberOfEnemies()" indicated in the waveCofig Scriptable Object.
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveCount(waveConfig); // This line of code is so to separate the movement speed and path away from the enemy prefabm and into the enemySpawner script/class.
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns()); // Anything below the "yield" line of code will be executed after the seconds indicated in the "WaitForSeconds".***
            //Debug.Log("Enemy speed is: " + waveConfig.GetenemyMoveSpeed());
        }

    }

}
