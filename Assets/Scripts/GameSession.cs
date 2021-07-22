using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        SetupSingleton();
        
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ScoreUpdate(int scoreValue)
    {
        score += scoreValue;
    }

    public void ScoreReset()
    {
        Destroy(gameObject);
    }

}
