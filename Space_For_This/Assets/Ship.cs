using UnityEngine;
using System.Collections;

public class Ship {

    public float shipSpeed { get; set;}
    public string shipName { get; set;}
    public Weapon[] weapons { get; set; }
	public float maxHealth { get; set; }
	public float currentHealth { get; set; }
    // Use this for initialization

	public Ship(string _shipName, float _maxHealth, float _shipSpeed = 0)
    {
		shipSpeed = _shipSpeed;
		shipName = _shipName;
		maxHealth = _maxHealth;
		currentHealth = _maxHealth;
        weapons = new Weapon[5];
    } 

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void applyDamage(float damage) {
		currentHealth -= damage;
	}
}
