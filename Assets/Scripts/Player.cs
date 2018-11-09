using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    private Health health;
    [SerializeField] private Animator deathAnimator; 
    [HideInInspector] public bool isDead = false;

    // Use this for initialization
    void Start()
    {
        Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        deathAnimator = inGameUITransform.Find("Death").GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    void CheckHealth()
    {
        if (isDead)
            return;
        if (health.value <= 0)
        {
            isDead = true;
            deathAnimator.SetTrigger("Show");
            LevelManager.instance.GameOver();
            Invoke("RestartGame", 5f);
        }
    }

    void RestartGame()
    {
        deathAnimator.SetTrigger("Reset");
        LevelManager.instance.RestartGame();
        Destroy(gameObject);
        
    }
 
}
