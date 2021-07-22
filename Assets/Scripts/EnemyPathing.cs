using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints; // List is more flexible than arrays. Its more dynamic.
                                                // We use tranform "type" because we are only looking for the position of each waypoint.We could have also used the " GamObject" type and then used "waypoints.transform...." to get the position.

    int waypointIndex = 0;
    
    // Start is called before the first frame update
void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position; // This line of code means that the start position of the enemy will be at the first "waypointIndex" (since we set it to 0) or the "waypoint[0]".

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();

    }


    // This method is so to separate the movement speed and path away from the enemy prefabm and into the enemySpawner script/class. We are getting the waveIndex or number from the enemySpawner Script/Class and feeding it in here.
    public void SetWaveCount(WaveConfig waveConfig) // We are calling this "Set"WaveCount and not "GET"WaveCount (like we did in the waveConfigScript) because we are FEEDING information into this method and getting or RETURNING anything from it.
    {
        this.waveConfig = waveConfig;  /* Okay. VERY IMPORTANT INFO: The "waveConfig" set as a **PARAMATER** for the "SetWaveCount(WaveConfig waveConfig)" method is DIFFERENT from the **CLASS** "waveConfig" 
                                          at the top, which is why we use "this.waveConfig = waveConfig;" to set them equal to each other, so that whenever we use this public method in any other 
                                          script/class and we pass a parameter in it "(SetWaveCount(WaveConfig waveConfig)", we use the main waveConfig Script. */
                                        
                                       /* Also whenever we set a parameter to a method, we ALWAYS HAVE TO give it a parameter value when we use it in other scripts or in the same script, for
                                          example, we can't just write "SetWaveCount()" and leave the parameter blank, we HAVE TO give it a parameter value. */
                                        
                                       /* "this.waveConfig" refers to the variable used in the entire class/script (at the top), whereas "= waveConfig" refers to the parameter set in this method.
                                       
                                       /* We can name the parameter or the class varaiable differently but it makes more sense to give them these names */
    }


    private void EnemyMove()
    {
        if (waypointIndex <= waypoints.Count - 1) // This means "if haven't yet reached last waypoint then..." do this. We put "- 1" in the end to make sure this line will continue, unless we are higher than our list (or higher than 3).
        {
            var targetPosition = waypoints[waypointIndex].transform.position; // The position of the wyapoint we're moving to. The current active Index.
            var enemySpeedinFrames = waveConfig.GetenemyMoveSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemySpeedinFrames);

            if (transform.position == targetPosition) // Check if we have reached the next waypoint, and if so then add 1 to the waypointIndex so that it would go to the next one.
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
