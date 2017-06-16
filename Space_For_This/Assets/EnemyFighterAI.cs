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

    public float health;

    public string shipName;
    public float speed;
    // Use this for initialization
    void Awake () {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		ship = new Ship("EnemyShip", health, speed);

		//todo: Move weapons (and abilities?) to loadout object that can be applied to prefabs
		ship.weapons[0] = Weapon.createBasicEnemyWeapon();
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

		if (currentAction == null){
			currentAction = swarmActions [0];
			currentActionPosition = 0;
		}
		//execute the current swarm action
		if (currentAction.actionType == swarmActionType.move) {

			if (currentMoveTarget == Vector3.zero) {
				computeMoveTarget ();
			}

			float step = currentAction.moveDetails.moveSpeed * Time.deltaTime;

			if (transform.position == currentMoveTarget) {
				//we're there - move to next action, or start over
				if (currentActionPosition == swarmActions.Count - 1) {
					currentAction = swarmActions[0];
					currentActionPosition = 0;
					computeMoveTarget ();
				} else {
					currentAction = swarmActions[currentActionPosition + 1];
					currentActionPosition += 1;
					if (currentAction.actionType == swarmActionType.move) {
						computeMoveTarget ();
					}
				}
			} else { 
				transform.position = Vector3.MoveTowards (transform.position, currentMoveTarget, step);
			}
		} else if (currentAction.actionType == swarmActionType.fire) {
			//get weapon and fire at target

			fireWeapons (currentAction.fireDetails);
			

			if (currentActionPosition == swarmActions.Count - 1) {
				currentAction = swarmActions[0];
				currentActionPosition = 0;
				computeMoveTarget ();
			} else {
				currentAction = swarmActions[currentActionPosition + 1];
				currentActionPosition += 1;
				if (currentAction.actionType == swarmActionType.move) {
					computeMoveTarget ();
				}
			}
		}
	}

	Vector3 computeFireTarget(Vector3 firestreamOffset){
		Vector3 playerPos = GameObject.Find ("Player").transform.position;
		Vector3 firingPos = this.gameObject.transform.position; //+ firestreamOffset;

		//the -5 scalar below is a cheat. Not sure why that works but whatever
		//this can be more accrate - change later
		Vector3 toReturn = -5*(firingPos - playerPos);

		return toReturn;
	}

	void computeMoveTarget(){
		float targetX = currentAction.moveDetails.moveTarget.x + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		float targetY = currentAction.moveDetails.moveTarget.y + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		currentMoveTarget = new Vector3 (targetX, targetY, 0);
	}
		
}