using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {

	Slider healthBar;
	Slider shieldBar;
	Ship playerShip;

	// Use this for initialization
	void Start () {
		healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
		shieldBar = GameObject.Find("ShieldBarSlider").GetComponent<Slider>();
		playerShip = GameObject.Find("Player").GetComponent<Player_Controller>().playerShip;
		healthBar.maxValue = playerShip.maxHealth;
		healthBar.value = playerShip.maxHealth;
		shieldBar.maxValue = playerShip.shield.maxShield;
		shieldBar.value = playerShip.shield.maxShield;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 10, 300, 20), "Time until next wave: " + GameObject.Find("SwarmController").GetComponent<SwarmController>().timeToNextSwarm.ToString());
		GUI.Label(new Rect(10, 20, 300, 20), "Current Scrap: " + GameObject.Find("Player").GetComponent<Player_Controller>().currentScrap.ToString());
	}

	public void updateHealth()
	{
		healthBar.value = playerShip.currentHealth;
	}

	public void updateShield()
	{
		shieldBar.value = playerShip.shield.currentShield;
	}
}
