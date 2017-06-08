using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerState_Controller : MonoBehaviour {

    public Slider healthSlider;

    public float currentHealth;
    public float maxHealth;

	// Use this for initialization
	void Awake () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void damagePlayer(float damage) {

        if (currentHealth - damage < 0)
        {
            currentHealth = 0;
        }
        else {
            currentHealth -= damage;
        }

        healthSlider.value = currentHealth;

    }
}

