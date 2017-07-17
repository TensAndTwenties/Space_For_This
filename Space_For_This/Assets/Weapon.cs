using UnityEngine;
using System.Collections;

public class Weapon
{
    public bool firing;
    public FireStream[] fireStreams;
    GameObject playerShip;
	// Use this for initialization
	void Awake () {
        firing = false;
        playerShip = GameObject.Find("player");
    }

    public Weapon(FireStream[] _fireStreams) {
        fireStreams = _fireStreams;
    }

	public static Weapon createBasicEnemyWeapon(){
	
		FireStream[] fireStreams = new FireStream[1];

		GameObject projectilePrefab = Resources.Load("projectile_1_slow") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),0);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;

	}

	public static Weapon createBasicEnemymissile(){

		FireStream[] fireStreams = new FireStream[1];

		GameObject projectilePrefab = Resources.Load("missile_nonhome") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),0);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;

	}

	public static Weapon createBasicEnemyRail(){

		FireStream[] fireStreams = new FireStream[1];

		GameObject projectilePrefab = Resources.Load("rail") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.01f, projectile, new Vector3(0f, 0, 0),0);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;

	}

	public static Weapon createFrigateSpreadWeapon(){

		FireStream[] fireStreams = new FireStream[3];

		GameObject projectilePrefab = Resources.Load("projectile_1_slow") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),60);
		fireStreams[1] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),0);
		fireStreams[2] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),30);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;

	}

	public static Weapon createBasicWeap1()
	{

		FireStream[] fireStreams = new FireStream[2];

		GameObject projectilePrefab = Resources.Load("projectile_1") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.1f, projectile, new Vector3(-0.3f, 0, 0));
		fireStreams[1] = new FireStream(0.1f, projectile, new Vector3(0.3f, 0, 0));

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;
	}

	public static Weapon createBasicWeap2()
	{

		FireStream[] fireStreams = new FireStream[1];

		GameObject projectilePrefab = Resources.Load("missile_1") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(1f, projectile, new Vector3(0, 0, 0));

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;
	}

	public static Weapon createBasicWeap3()
	{

		FireStream[] fireStreams = new FireStream[6];

		GameObject projectilePrefab = Resources.Load("projectile_1") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.2f, projectile, new Vector3(-0.45f, 0, 0),60);
		fireStreams[1] = new FireStream(0.2f, projectile, new Vector3(-0.45f, 0, 0));
		fireStreams[2] = new FireStream(0.2f, projectile, new Vector3(-0.45f, 0, 0),30);

		fireStreams[3] = new FireStream(0.2f, projectile, new Vector3(0.45f, 0, 0),60);
		fireStreams[4] = new FireStream(0.2f, projectile, new Vector3(0.45f, 0, 0));
		fireStreams[5] = new FireStream(0.2f, projectile, new Vector3(0.45f, 0, 0),30);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;
	}

	public static Weapon createBasicWeap4()
	{

		FireStream[] fireStreams = new FireStream[1];

		GameObject projectilePrefab = Resources.Load("projectile_2_(spread)") as GameObject;
		Projectile projectile = projectilePrefab.GetComponent<Projectile>();
		projectile.prefab = projectilePrefab;

		fireStreams[0] = new FireStream(0.1f, projectile, new Vector3(0f, 0, 0),0,4f);

		Weapon newWeapon = new Weapon(fireStreams);

		return newWeapon;
	}
}



public class FireStream {
    public float fireRate { get; set; }
    public float currentCooldown { get; set; }
    public Projectile projectile { get; set; }
    public Vector3 offset { get; set; }
    public float angleOffset { get; set; }
    public float spread { get; set; }

    public FireStream(float _fireRate, Projectile _projectile, Vector3 _offset, float _angleOffset = 0, float _spread = 0) {
        fireRate = _fireRate;
        projectile = _projectile;
        offset = _offset;
        angleOffset = _angleOffset;
        spread = _spread;
    }

}

