using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePowerUp : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.AddLife();
            Destroy(gameObject);
        }
    }
}
