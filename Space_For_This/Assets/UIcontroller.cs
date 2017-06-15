using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {

	Slider healthBar;
	Ship playerShip;

	// Use this for initialization
	void Start () {
		healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
		playerShip = GameObject.Find("Player").GetComponent<Player_Controller>().playerShip;
		healthBar.maxValue = playerShip.maxHealth;
		healthBar.value = playerShip.maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateHealth()
	{
		healthBar.value = playerShip.currentHealth;
	}
}
