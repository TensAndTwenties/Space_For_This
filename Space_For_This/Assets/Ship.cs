using UnityEngine;
using System.Collections;

public class Ship {

    public int shipSpeed { get; set;}
    public string shipName { get; set;}
    public Weapon[] weapons { get; set; }
    // Use this for initialization

    public Ship(string name, int speed)
    {
        shipSpeed = speed;
        shipName = name;
        weapons = new Weapon[1];
    } 

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
