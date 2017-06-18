using UnityEngine;
using System.Collections;

public class Ship {

    public float shipSpeed { get; set;}
    public string shipName { get; set;}
    public Weapon[] weapons { get; set; }
	public float maxHealth { get; set; }
	public float currentHealth { get; set; }
	public bool playerShip { get ; set;}
	public float dodgeLength { get; set; }
	public float dodgeSpeed { get; set;}
	public Shield shield { get; set;}
    // Use this for initialization

	public Ship(string _shipName, float _maxHealth, float _shipSpeed = 0, bool _playerShip = false)
    {
		shipSpeed = _shipSpeed;
		shipName = _shipName;
		maxHealth = _maxHealth;
		currentHealth = _maxHealth;
        weapons = new Weapon[5];
		playerShip = _playerShip;
    } 

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void applyDamage(float damage) {

		if (playerShip) {
			//if sheild is up, damage sheild and re-set the recharge on it. Otherwise damage health.
			if (shield.currentShield > 0) {
				if ((shield.currentShield -= damage) < 0) {
					shield.currentShield = 0;
				} else {
					shield.currentShield -= damage;
				}

				shield.resetRechargeDelay ();
				GameObject.Find("Canvas").GetComponent<UIcontroller>().updateShield();

			} else {
				currentHealth -= damage;
				GameObject.Find("Canvas").GetComponent<UIcontroller>().updateHealth();
			}


		}
	}
}
