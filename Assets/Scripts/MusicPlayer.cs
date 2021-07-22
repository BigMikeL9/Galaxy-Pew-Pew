using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1) // "GetType()" gets the gameObject that this script/class is attached to, instead of identifying the name of the gameobject in between <...>
        {
            gameObject.SetActive(false); // This is just a safe guard. See "Block Breaker" game for more details.
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
