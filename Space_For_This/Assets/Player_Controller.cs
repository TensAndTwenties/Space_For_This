using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour {

    //private enum PlayerDirection {up,down,left,right};
	private enum DodgeDirection
	{
		N,S,E,W,NW,SW,NE,SE
	}

    public Ship playerShip;
    Camera gameCamera;
	public PlayerState currentState = PlayerState.normal;
	DodgeDirection currentDodgeDirection;
	Vector3 currentDodgeTarget;

    Vector3 shipSize;
    float shipWidth;
    KeyCode previouslyPressed = 0;

    Object enemyShip1; 
    Object enemyShip2; 
    Object[] enemies = new Object[2];

	public float shipSpeed;
	public string shipName;
	public Weapon[] weapons;
	public float maxHealth;
	public float dodgeLength;
	public float dodgeSpeed;
	public float maxShield;
	public float shieldRechargeRate;
	public float shieldRechargeDelay;

    // Use this for initialization
    void Awake() {
		playerShip = new Ship(shipType.playerShip1, maxHealth, shipSpeed, true);
		playerShip.dodgeLength = dodgeLength;
		playerShip.dodgeSpeed = dodgeSpeed;

        playerShip.weapons[0] = Weapon.createBasicWeap1();
		playerShip.weapons[1] = Weapon.createBasicWeap1();
		playerShip.weapons[2] = Weapon.createBasicWeap2();
		playerShip.weapons[3] = Weapon.createBasicWeap3();
		playerShip.weapons[4] = Weapon.createBasicWeap4();

		playerShip.shield = new Shield (maxShield, shieldRechargeRate, shieldRechargeDelay);

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

		if (currentState == PlayerState.dodging) {
			step = playerShip.dodgeSpeed * Time.deltaTime; 
		}

		UpdateShield ();

        foreach (FireStream fs in playerShip.weapons[0].fireStreams)
        {
            if (fs.currentCooldown > 0)
            {
                fs.currentCooldown -= Time.deltaTime;
            }
			else if (fs.currentCooldown < 0) {
                fs.currentCooldown = 0;
            }
        }

		if (currentState != PlayerState.dodging) {
			if (Input.GetKey (KeyCode.Space)) {
				FireWeapon ();

				//previouslyPressed = KeyCode.Space;
			}

			if (Input.GetKey (KeyCode.LeftControl)) {
				if (previouslyPressed != KeyCode.LeftControl) {
					//used to spawn enemies for testing
					/*float targetX = Random.Range (gameCamera.GetComponent<CameraScript> ().minX, gameCamera.GetComponent<CameraScript> ().maxX);
					float targetY = Random.Range (gameCamera.GetComponent<CameraScript> ().minY + 12, gameCamera.GetComponent<CameraScript> ().maxY);
					Vector3 spawnVector = new Vector3 (targetX, targetY, 0);

					Object enemyToSpawn = enemies [Random.Range (0, enemies.Length)];
					GameObject newEnemy = Instantiate (enemyToSpawn) as GameObject;
					newEnemy.transform.position = spawnVector;
					*/
				}
			}

			if (Input.GetKeyUp (KeyCode.Space)) {
				previouslyPressed = 0;
			}

			if (Input.GetKey (KeyCode.Alpha1)) {
				playerShip.weapons [0] = playerShip.weapons [1];
			}

			if (Input.GetKey (KeyCode.Alpha2)) {
				playerShip.weapons [0] = playerShip.weapons [2];
			}

			if (Input.GetKey (KeyCode.Alpha3)) {
				playerShip.weapons [0] = playerShip.weapons [3];
			}

			if (Input.GetKey (KeyCode.Alpha4)) {
				playerShip.weapons [0] = playerShip.weapons [4];
			}

			if (Input.GetKey (KeyCode.W)) {
				if (transform.position.x >= gameCamera.GetComponent<CameraScript> ().minX + step)
					transform.position = new Vector2 (transform.position.x - step, transform.position.y);
			}
			if (Input.GetKey (KeyCode.S)) {
				if (transform.position.x <= gameCamera.GetComponent<CameraScript> ().maxX - step)
					transform.position = new Vector2 (transform.position.x + step, transform.position.y);
			}
			if (Input.GetKey (KeyCode.A)) {
				if (transform.position.y >= gameCamera.GetComponent<CameraScript> ().minY + step)
					transform.position = new Vector2 (transform.position.x, transform.position.y - step);
			}
			if (Input.GetKey (KeyCode.D)) {
				if (transform.position.y <= gameCamera.GetComponent<CameraScript> ().maxY - step)
					transform.position = new Vector2 (transform.position.x, transform.position.y + step);
			}
			if (Input.GetKey (KeyCode.LeftShift)) {
				//dodge! Based on which direction keys are currently held
				if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.W)) {
					ExecuteDodge (DodgeDirection.NE);
				} else if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.S)) {
					ExecuteDodge (DodgeDirection.SE);
				} else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.W)) {
					ExecuteDodge (DodgeDirection.NW);
				} else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.A)) {
					ExecuteDodge (DodgeDirection.SW);
				} else {
					if (Input.GetKey (KeyCode.D)) {
						ExecuteDodge (DodgeDirection.E);
					}
					if (Input.GetKey (KeyCode.A)) {
						ExecuteDodge (DodgeDirection.W);
					}
					if (Input.GetKey (KeyCode.W)) {
						ExecuteDodge (DodgeDirection.N);
					}
					if (Input.GetKey (KeyCode.S)) {
						ExecuteDodge (DodgeDirection.S);
					}
				}
			}
		} else {
			//if (transform.position.y > gameCamera.GetComponent<CameraScript>().maxY + 1 || transform.position.y < gameCamera.GetComponent<CameraScript>().minY - 1 || transform.position.x > gameCamera.GetComponent<CameraScript>().maxX + 1 || transform.position.x < gameCamera.GetComponent<CameraScript>().minX - 1)
			transform.position = Vector3.MoveTowards (transform.position, currentDodgeTarget, step);

			bool ceaseDodge = false;

			if (transform.position.y > gameCamera.GetComponent<CameraScript> ().maxY + step) {
				transform.position = new Vector3 (transform.position.x, gameCamera.GetComponent<CameraScript> ().maxY + step);
				ceaseDodge = true;
			} else if (transform.position.y < gameCamera.GetComponent<CameraScript> ().minY - step) {
				transform.position = new Vector3 (transform.position.x, gameCamera.GetComponent<CameraScript> ().minY - step);
				ceaseDodge = true;
			} else if (transform.position.x < gameCamera.GetComponent<CameraScript> ().minX - step) {
				transform.position = new Vector3 (gameCamera.GetComponent<CameraScript> ().minX - step,transform.position.y);
				ceaseDodge = true;
			} else if (transform.position.x > gameCamera.GetComponent<CameraScript> ().maxX + step) {
				transform.position = new Vector3 (gameCamera.GetComponent<CameraScript> ().maxX + step,transform.position.y);
				ceaseDodge = true;
			}

			if(transform.position == currentDodgeTarget || ceaseDodge) {
				CeaseDodge ();
			}
				//dodging, wait until done
		}
    }

	private void ExecuteDodge(DodgeDirection direction)
	{
		currentState = PlayerState.dodging;
		currentDodgeDirection = direction;
		float xyDodgeLength = Mathf.Sqrt((playerShip.dodgeLength*playerShip.dodgeLength)/2);
		//IEnumerable coroutine = Dodge ();

		switch (direction) {
		case DodgeDirection.E:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (0, playerShip.dodgeLength);
			break;
		case DodgeDirection.N:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (-playerShip.dodgeLength,0);
			break;
		case DodgeDirection.W:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (0,-playerShip.dodgeLength);
			break;
		case DodgeDirection.S:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (playerShip.dodgeLength,0);
			break;
		case DodgeDirection.NE:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (-xyDodgeLength,xyDodgeLength);
			break;
		case DodgeDirection.SE:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (xyDodgeLength,xyDodgeLength);
			break;
		case DodgeDirection.NW:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (-xyDodgeLength,-xyDodgeLength);
			break;
		case DodgeDirection.SW:
			currentDodgeTarget = this.gameObject.transform.position + new Vector3 (xyDodgeLength,-xyDodgeLength);
			break;
		}

	}
		
	private void CeaseDodge (){
		currentState = PlayerState.normal;
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

	public void UpdateShield()
	{
		//controls shield recharge and recharge delay updates
		//if(playerShip.shield.currentRechargeDelay)
		if (playerShip.shield.currentRechargeDelay == 0 && playerShip.shield.currentShield < playerShip.shield.maxShield) {
			//recharge
			if ((playerShip.shield.currentShield += playerShip.shield.rechargeRate) > playerShip.shield.maxShield) {
				playerShip.shield.currentShield = playerShip.shield.maxShield;
			} else {
				playerShip.shield.currentShield += playerShip.shield.rechargeRate;
			}

			GameObject.Find("Canvas").GetComponent<UIcontroller>().updateShield();
		} else {
			if ((playerShip.shield.currentRechargeDelay -= Time.deltaTime) < 0) {
				playerShip.shield.currentRechargeDelay = 0;
			} else {
				playerShip.shield.currentRechargeDelay -= Time.deltaTime;
			}
		}
	}
}

public enum PlayerState
{
	normal,
	dodging
}
