using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAnimation : MonoBehaviour
{

    [SerializeField] float destroyAfterSeconds;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
