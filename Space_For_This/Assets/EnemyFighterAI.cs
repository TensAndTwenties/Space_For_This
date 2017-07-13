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

		if (componentCount > 0) {
			//spawn in components

			if (shipType == shipType.frigate) {
				//add components

				var values = componentType.GetValues(typeof (componentType));
				componentType left = (componentType) values.GetValue (Random.Range(0,values.Length));
				componentType right = (componentType) values.GetValue (Random.Range(0,values.Length));
		
				Object LeftComponent = Resources.Load ("left_" + left.ToString ());
				components.Add (LeftComponent);
				Object RightComponent =	Resources.Load ("right_" + right.ToString ());
				components.Add (RightComponent);
				
			}

			foreach (Object component in components) {
				//initialize component properties
				GameObject newComponent = Instantiate (component) as GameObject;
				newComponent.GetComponent<EnemyFighterAI> ().isComponent = true;
				newComponent.transform.parent = this.transform;
			}
		}


    }

	//todo: currently the player and AI ships have the weapon firing code duplicated. Maybe move to its own
	//place if the firing proves to be similar

	public void fireWeapons(SwarmFireDetails details){
		int[] weaponIndexes = details.fireWeapons;
		foreach (int weaponIndex in weaponIndexes) {
			foreach (FireStream fs in ship.weapons[weaponIndex].fireStreams) {
				// NO cooldowns for enemy weapons so we can script whatever we want
				//if (fs.currentCooldown <= 0f) {
				FireProjectile (fs, details.targetType);
					fs.currentCooldown = fs.fireRate;
				//}
			}
		}
	}

	public void FireProjectile(FireStream fireStream, swarmTargetType targetType )
	{
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
	}

	// Update is called once per frame
	void Update () {
		
		if (ship.currentHealth <= 0)
		{
			Destroy(this.gameObject);
		}

		if (!ship.isComponent) {
			if (currentAction == null) {
				currentAction = swarmActions [0];
				currentActionPosition = 0;
			}
			//execute the current swarm action
			if (currentAction.actionType == swarmActionType.move) {

				if (!currentAction.moveDetails.bezier) {
					if (currentMoveTarget == Vector3.zero) {
						computeMoveTarget ();
					}

					float step = currentAction.moveDetails.moveSpeed * Time.deltaTime;

					if (transform.localPosition == currentMoveTarget) {
						//we're there - move to next action, or start over
						if (currentActionPosition == swarmActions.Count - 1) {
							currentAction = swarmActions [0];
							currentActionPosition = 0;
							computeMoveTarget ();
						} else {
							currentAction = swarmActions [currentActionPosition + 1];
							currentActionPosition += 1;
							if (currentAction.actionType == swarmActionType.move) {
								computeMoveTarget ();
							}
						}
					} else { 
						transform.localPosition = Vector3.MoveTowards (transform.localPosition, currentMoveTarget, step);
					}
				} else {
					//currentAction = swarmActions [0];
					LeanTween.move (this.gameObject, currentAction.moveDetails.bezierVectors, 2f);
				}
			} else if (currentAction.actionType == swarmActionType.fire) {
				//get weapon and fire at target

				if (shipType != shipType.dummy) {
					fireWeapons (currentAction.fireDetails);
				}

				if (currentActionPosition == swarmActions.Count - 1) {
					currentAction = swarmActions [0];
					currentActionPosition = 0;
					computeMoveTarget ();
				} else {
					currentAction = swarmActions [currentActionPosition + 1];
					currentActionPosition += 1;
					if (currentAction.actionType == swarmActionType.move) {
						computeMoveTarget ();
					}
				}
			}
		} else {
			//component update code
			//make components fire once every X seconds. Ensure there is at least Y seconds before firings

			timeSinceLastFiring += Time.deltaTime;

			if (Random.Range (minTimeBetweenFiring, maxTimeBetweenFiring) <= timeSinceLastFiring) {
				//fire
				SwarmFireDetails fireDetails = new SwarmFireDetails(swarmTargetType.straightAhead, new int[1]  { 0 });
				fireWeapons (fireDetails);
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

	void computeMoveTarget(){
		float targetX = currentAction.moveDetails.moveTarget.x + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		float targetY = currentAction.moveDetails.moveTarget.y + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		currentMoveTarget = new Vector3 (targetX, targetY, 0);
	}
		
}