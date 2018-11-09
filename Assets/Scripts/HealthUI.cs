using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    private Health health;
    public Text healthText;
    public Animator hitAnimator;
	// Use this for initialization
	void Start () {

        Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        healthText = inGameUITransform.Find("Health").GetComponent<Text>();
        hitAnimator = inGameUITransform.Find("Hit").GetComponent<Animator>();

        health = GetComponent<Health>();
        health.onHit.AddListener(() =>
        {
            hitAnimator.SetTrigger("Show");
            UpdateHealthText();
        });
        UpdateHealthText();
	}
	
	// Update is called once per frame
	void UpdateHealthText () {
        healthText.text = "HP: " + health.value;
	}
}
