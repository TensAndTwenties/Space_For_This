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
		ship.weapons[0] = Player_Controller.createBasicWeap1();
    }

	//todo: currently the player and AI ships have the weapon firing code duplicated. Maybe move to its own
	//place if the firing proves to be similar

	public void fireWeapons(int[] weaponIndexes){
		foreach (int weaponIndex in weaponIndexes) {
			foreach (FireStream fs in ship.weapons[weaponIndex].fireStreams) {
				//InvokeRepeating("FireProjectile", 0.0F, fs.fireRate);
				if (fs.currentCooldown <= 0f) {
					FireProjectile (fs);
					fs.currentCooldown = fs.fireRate;
				}
			}
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

			fireWeapons (currentAction.fireDetails.fireWeapons);
			

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

	void computeMoveTarget(){
		float targetX = currentAction.moveDetails.moveTarget.x + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		float targetY = currentAction.moveDetails.moveTarget.y + Random.Range (-currentAction.moveDetails.moveTargetVariance, currentAction.moveDetails.moveTargetVariance);
		currentMoveTarget = new Vector3 (targetX, targetY, 0);
	}
		
}