using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour {

    private enum PlayerDirection {up,down,left,right};

    Ship playerShip;
    Camera gameCamera;

    Vector3 shipSize;
    float shipWidth;
    KeyCode previouslyPressed = 0;

    // Use this for initialization
    void Awake() {
        playerShip = new Ship("testShip", 4);
        playerShip.weapons[0] = createBasicWeap1();
        shipSize = GetComponent<Renderer>().bounds.size;
        shipWidth = shipSize.x;

        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        float step = playerShip.shipSpeed * Time.deltaTime; //movement value

        foreach (FireStream fs in playerShip.weapons[0].fireStreams)
        {
            if (fs.currentCooldown > 0)
            {
                fs.currentCooldown -= Time.deltaTime;
            }
            else {
                fs.currentCooldown = 0;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            FireWeapon();

            //previouslyPressed = KeyCode.Space;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (transform.position.x >= gameCamera.GetComponent<CameraScript>().minX + step)
                transform.position = new Vector2(transform.position.x - step, transform.position.y);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if(transform.position.x <= gameCamera.GetComponent<CameraScript>().maxX - step)
                transform.position = new Vector2(transform.position.x + step, transform.position.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.y >= gameCamera.GetComponent<CameraScript>().minY + step)
                transform.position = new Vector2(transform.position.x, transform.position.y - step);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.y <= gameCamera.GetComponent<CameraScript>().maxY - step)
                transform.position = new Vector2(transform.position.x, transform.position.y + step);
        }
    }

    public void FireWeapon()
    {
        foreach (FireStream fs in playerShip.weapons[0].fireStreams)
        {
            //InvokeRepeating("FireProjectile", 0.0F, fs.fireRate);
            if (fs.currentCooldown <= 0f)
            {
                FireProjectile(fs);
                fs.currentCooldown = fs.fireRate;
            }
        }
    }

    public void CeaseWeapon()
    {
        foreach (FireStream fs in playerShip.weapons[0].fireStreams)
        {

        }
    }

    public void FireProjectile(FireStream fireStream)
    {
        GameObject newProjectileObj = Instantiate(fireStream.projectile.prefab) as GameObject;
        newProjectileObj.transform.position = transform.position + fireStream.offset;
        newProjectileObj.GetComponent<Projectile>().angle = fireStream.angleOffset;
    }

    public static Weapon createBasicWeap1()
    {

        FireStream[] fireStreams = new FireStream[2];

        GameObject projectilePrefab = Resources.Load("projectile_1") as GameObject;
        Projectile projectile = projectilePrefab.GetComponent<Projectile>();
        projectile.speed = 14;
        projectile.prefab = projectilePrefab;

        fireStreams[0] = new FireStream(0.3f, projectile, new Vector3(-0.3f, 0, 0), 80);
        fireStreams[1] = new FireStream(0.3f, projectile, new Vector3(0.3f, 0, 0), 10);

        Weapon newWeapon = new Weapon(fireStreams);

        return newWeapon;
    }
}
