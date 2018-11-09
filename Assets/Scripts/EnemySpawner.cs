using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject zombiePrefab;

    public GameObject SpawnAt(Vector3 pos, Quaternion rot)
    {
        GameObject enemy = Instantiate(zombiePrefab, pos, rot);
        return enemy;
    }
	
}
