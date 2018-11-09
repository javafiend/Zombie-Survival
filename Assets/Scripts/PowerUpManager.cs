using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpSpawner : MonoBehaviour {

    public GameObject pickupEffect;
    [HideInInspector] public UnityEvent onDead;


    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
    }
    public GameObject SpawnAt(Vector3 pos, Quaternion rot)
    {
        GameObject pickup = Instantiate(pickupEffect, pos, rot);
        return pickup;
    }

}
