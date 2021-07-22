using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    // This script will have all the stuff related to damage (laser, missilies, bombs, enemies bumping into the player, etc)

    [SerializeField] int damage = 100;


    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
