using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBoxBehaviour : MonoBehaviour
{
    public EnemyHealth EnemyHealth;
    public float SprayDamage = 3f;
    void Start()
    {
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
   

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wasp"))
        {
            //EnemyHealth.health -= SprayDamage;
            if (other.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.health -= SprayDamage;
            }
            print("NOT THE BEES");
        }
         

        
    }
}
