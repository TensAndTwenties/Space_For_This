using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour {

    private enum PlayerDirection {up,down,left,right};

    public Ship playerShip;
    Camera gameCamera;

    Vector3 shipSize;
    float shipWidth;
    KeyCode previouslyPressed = 0;

    Object enemyShip1; 
    Object enemyShip2; 
    Object[] enemies = new Object[2];
    


    // Use this for initialization
    void Awake() {
        playerShip = new Ship("testShip", 200, 4, true);
        playerShip.weapons[0] = Weapon.createBasicWeap1();
		playerShip.weapons[1] = Weapon.createBasicWeap1();
		playerShip.weapons[2] = Weapon.createBasicWeap2();
		playerShip.weapons[3] = Weapon.createBasicWeap3();
		playerShip.weapons[4] = Weapon.createBasicWeap4();
        shipSize = GetComponent<Renderer>().bounds.size;
        shipWidth = shipSize.x;



        enemies[0] = Resources.Load("Enemy1");
        enemies[1] = Resources.Load("Enemy2");

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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (previouslyPressed != KeyCode.LeftControl)
            {
                float targetX = Random.Range(gameCamera.GetComponent<CameraScript>().minX, gameCamera.GetComponent<CameraScript>().maxX);
                float targetY = Random.Range(gameCamera.GetComponent<CameraScript>().minY + 12, gameCamera.GetComponent<CameraScript>().maxY);
                Vector3 spawnVector = new Vector3(targetX, targetY, 0);

                Object enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
                GameObject newEnemy = Instantiate(enemyToSpawn) as GameObject;
                newEnemy.transform.position = spawnVector;

            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            previouslyPressed = 0;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            playerShip.weapons[0] = playerShip.weapons[1];
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            playerShip.weapons[0] = playerShip.weapons[2];
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            playerShip.weapons[0] = playerShip.weapons[3];
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            playerShip.weapons[0] = playerShip.weapons[4];
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
        if (fireStream.spread > 0f)
        {
            newProjectileObj.GetComponent<Projectile>().angle = fireStream.angleOffset + 45 + Random.Range(-fireStream.spread, fireStream.spread);
        }
    }
}
