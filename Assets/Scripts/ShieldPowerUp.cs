using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        //shieldPrefab.transform.position = player.transform.position;
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            StartCoroutine(ShieldUpdate());
            spriteRenderer.enabled = false;
            Destroy(gameObject, 10f);
        }
    }

    private IEnumerator ShieldUpdate() // Need to add an ontriggerenter2d to the shield prefab and set the trigger on on the collider
    {
        player.GetComponent<Player>().shield = true;
        Debug.Log("Shield is Active");
        yield return new WaitForSeconds(2);
        player.GetComponent<Player>().shield = false;
        Debug.Log("Shield is not Active");
    }
}
