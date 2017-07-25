using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFighterAI : MonoBehaviour {
    Vector3 targetVector;
    Random rand;
    Camera gameCamera;
	public Ship ship { get; set; }
	public List<SwarmPathAction> swarmActions { get; set;}
	SwarmPathAction currentAction;
	int currentActionPosition;
	Vector3 currentMoveTarget = Vector3.zero;
	float currentMoveTime;

	public float shipSpeed;
	public shipType shipType;
	public Weapon[] weapons;
	public float maxHealth;
	public bool playerShip;
	public float dodgeLength;
	public float dodgeSpeed;
	public bool isComponent;
	public int componentCount;
	public List<Object> components;
	public componentType componentType;
	public bool damageMask; //do we want to display a mask on hit?
	public int scrapMin; //min scrap per drop
	public int scrapMax; //max scrap per drop
	public int scrapDrops; //number of distinct scraps dropped

	//for components
	public float maxTimeBetweenFiring = 4f;
	public float minTimeBetweenFiring = 1f;
	public float timeSinceLastFiring = 0;

    // Use this for initialization
    void Awake () {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		ship = new Ship(shipType, maxHealth, shipSpeed);
		ship.dodgeSpeed = dodgeSpeed;
		ship.dodgeLength = dodgeLength;
		ship.weapons [0] = Weapon.createBasicEnemyWeapon ();
		ship.isComponent = isComponent;
		ship.componentType = componentType;

		//instantiate weapons based on ship type

		switch (shipType) {
		case shipType.fighter:
			break;
		case shipType.frigate:
			ship.weapons [0] = Weapon.createFrigateSpreadWeapon ();
			break;
		}

		if (componentCount > 0) {
			//spawn in components

			string resourcePath = "";

			switch (shipType) {
			case shipType.frigate: 
				resourcePath = "frigate_components/";
				break;
			}

			//right now we only support 2 components per ship - 1 left and 1 right
			var values = componentType.GetValues(typeof (componentType));
			componentType left = (componentType) values.GetValue (Random.Range(0,values.Length));
			componentType right = (componentType) values.GetValue (Random.Range(0,values.Length));

			//totally janky
			Object LeftComponent = Resources.Load (resourcePath + "left_" + left.ToString ());
			components.Add (LeftComponent);

			Object RightComponent =	Resources.Load (resourcePath + "right_" + right.ToString ());
			components.Add (RightComponent);

			foreach (Object component in components) {
				//initialize component properties
				GameObject newComponent = Instantiate (component) as GameObject;
				newComponent.GetComponent<EnemyFighterAI> ().isComponent = true;
				newComponent.transform.parent = this.transform;

				switch (newComponent.GetComponent<EnemyFighterAI> ().componentType) {
				case componentType.shield:
					shipType type = this.ship.shipType;
					Object shield = null;
					GameObject newShield = null;

					switch (type) {
					case shipType.frigate:
						shield = Resources.Load (resourcePath + "shield");
						break;
					}

					newShield = Instantiate (shield) as GameObject;
					newShield.transform.parent = newComponent.transform;
					//set sheild and generator to both have null weapons
					newShield.GetComponent<EnemyFighterAI> ().ship.weapons = null;
					this.weapons = null;

					break;
				case componentType.missile:
					newComponent.GetComponent<EnemyFighterAI> ().ship.weapons [0] = Weapon.createBasicEnemymissile ();
					break;
				case componentType.rail:
					newComponent.GetComponent<EnemyFighterAI> ().ship.weapons [0] = Weapon.createBasicEnemyRail ();
					break;
				}
					
			}
		}


    }

	//todo: currently the player and AI ships have the weapon firing code duplicated. Maybe move to its own
	//place if the firing proves to be similar

	public void fireWeapons(SwarmFireDetails details){
		if (ship.weapons != null && ship.weapons.Length > 0) {
			int[] weaponIndexes = details.fireWeapons;
			foreach (int weaponIndex in weaponIndexes) {
				foreach (FireStream fs in ship.weapons[weaponIndex].fireStreams) {
					// NO cooldowns for enemy weapons so we can script whatever we want
					//if (fs.currentCooldown <= 0f) {
					IEnumerator coroutine = FireProjectile (fs, details.targetType, details.firings);
					StartCoroutine(coroutine);

					//FireProjectile (fs, details.targetType);
					//fs.currentCooldown = fs.fireRate;
					//}
				}
			}
		}
	}

	public IEnumerator FireProjectile(FireStream fireStream, swarmTargetType targetType, int firings )
	{
		for(int i = 1; i <= firings; i++){
		GameObject newProjectileObj = Instantiate(fireStream.projectile.prefab) as GameObject;
		newProjectileObj.transform.position = transform.position + fireStream.offset;
		newProjectileObj.GetComponent<Projectile>().angle = fireStream.angleOffset;
		newProjectileObj.GetComponent<Projectile>().enemyProjectile = true;
		newProjectileObj.GetComponent<Projectile>().targetType = targetType;


		if(targetType == swarmTargetType.atPlayer){
			Vector3 playerTarget = computeFireTarget(fireStream.offset);
			newProjectileObj.GetComponent<Projectile> ().dumbTarget = playerTarget;
		}

		if (fireStream.spread > 0f)
		{
			newProjectileObj.GetComponent<Projectile>().angle = fireStream.angleOffset + 45 + Random.Range(-fireStream.spread, fireStream.spread);
		}

		yield return new WaitForSeconds(fireStream.fireRate);
		}
	}

	public void applyDamage(float damage){
		this.ship.applyDamage (damage);
		if (damageMask) {
			IEnumerator coroutine = applyDamageMask (this.gameObject);
			StartCoroutine(coroutine);
		}
	}

	private IEnumerator applyDamageMask(GameObject objectToMask){

		this.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;

		yield return new WaitForSeconds(0.1f);

		this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
	}

	void destroyShip(){
		Destroy(this.gameObject);

		if (Random.Range(0, 10) == 1) {
			
			for (int i = 1; i <= scrapDrops; i++) {

				int scrapAmount = Random.Range (scrapMin, scrapMax);

				Object scrapObj = Resources.Load ("scrap");
				GameObject newScrap = Instantiate (scrapObj) as GameObject;
				newScrap.transform.position = this.transform.position;
				newScrap.GetComponent<Scrap> ().scrapAmount = scrapAmount;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
		if (ship.currentHealth <= 0)
		{
			destroyShip ();
		}

		if (!ship.isComponent) {
			if (currentAction == null) {
				currentAction = swarmActions [0];
				currentActionPosition = 0;
			}
			//execute the current swarm action
			if (currentAction.actionType == swarmActionType.move) {

				if (currentMoveTarget == Vector3.zero) {
					computeMoveTargetAndTime ();
				}

				if (transform.localPosition == currentMoveTarget) {
					//we're there - move to next action, or start over
					if (currentActionPosition == swarmActions.Count - 1) {

						SwarmPathAction oldAction = currentAction;

						currentAction = swarmActions [0];
						currentActionPosition = 0;
						computeMoveTargetAndTime ();

						if (oldAction.moveDetails.bezier) {
							//starting over form bezier - compute move target to be final bezier vector
							currentMoveTarget = oldAction.moveDetails.bezierVectors[oldAction.moveDetails.bezierVectors.Length-1];
							 
						}

					} else {
						currentAction = swarmActions [currentActionPosition + 1];
						currentActionPosition += 1;
						if (currentAction.actionType == swarmActionType.move) {
							computeMoveTargetAndTime ();
						}
					}
				} else { 
					if (LeanTween.isTweening (this.gameObject)) {
					} else {
						switch (currentAction.moveDetails.moveActionType) {
							case swarmMoveActionType.linear:
								LeanTween.moveLocal ( this.gameObject,currentMoveTarget, currentMoveTime);
								break;
							case swarmMoveActionType.bezier:
								//note the bad approximation of move time. Tough to cheaply predict the length of a bezier
								//curve based on it's control vectors
							LeanTween.moveLocal (this.gameObject, currentAction.moveDetails.bezierVectors, 1.2f * currentMoveTime).setRepeat(0);
							break;
						}

					}
					//transform.localPosition = Vector3.MoveTowards (transform.localPosition, currentMoveTarget, step);
				}
			} else if (currentAction.actionType == swarmActionType.fire) {
				//get weapon and fire at target

				if (shipType != shipType.dummy) {
					fireWeapons (currentAction.fireDetails);
				}

				if (currentActionPosition == swarmActions.Count - 1) {
					currentAction = swarmActions [0];
					currentActionPosition = 0;
					computeMoveTargetAndTime ();
				} else {
					currentAction = swarmActions [currentActionPosition + 1];
					currentActionPosition += 1;
					if (currentAction.actionType == swarmActionType.move) {
						computeMoveTargetAndTime ();
					}
				}
			}
		} else {
			//component update code
			//make components fire once every X seconds. Ensure there is at least Y seconds before firings

			timeSinceLastFiring += Time.deltaTime;

			if (Random.Range (minTimeBetweenFiring, maxTimeBetweenFiring) <= timeSinceLastFiring) {
				//fire

				SwarmFireDetails fireDetails = null;

				switch(this.componentType){
				case componentType.missile:
					fireDetails = new SwarmFireDetails (swarmTargetType.straightAhead, new int[1]  { 0 }, 3);
					break;
				case componentType.rail:
					fireDetails = new SwarmFireDetails (swarmTargetType.straightAhead, new int[1]  { 0 }, 10);
					break;
				}

				if (fireDetails != null) {
					fireWeapons (fireDetails);
				}

				timeSinceLastFiring = 0;
			}
		}
	}

	Vector3 computeFireTarget(Vector3 firestreamOffset){
		Vector3 playerPos = GameObject.Find ("Player").transform.position;
		Vector3 firingPos = this.gameObject.transform.position + firestreamOffset;
		Vector3 toReturn = playerPos - 5*(firingPos - playerPos);

		return toReturn;
	}

	void computeMoveTargetAndTime(){
		float targetX = currentAction.moveDetails.moveTarget.x + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		float targetY = currentAction.moveDetails.moveTarget.y + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		if (currentAction.moveDetails.moveActionType == swarmMoveActionType.bezier) {
			currentAction.moveDetails.bezierVectors[currentAction.moveDetails.bezierVectors.Length-1] = new Vector3 (targetX, targetY, 0);
			currentAction.moveDetails.bezierVectors[0] = this.transform.localPosition;
		}
		currentMoveTarget = new Vector3 (targetX, targetY, 0);
		float currentMoveDistance = (currentMoveTarget - this.transform.localPosition).magnitude;
		currentMoveTime = currentMoveDistance/currentAction.moveDetails.moveSpeed;
	}
		
}