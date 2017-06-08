using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFighterAI : MonoBehaviour {
    Vector3 targetVector;
    Random rand;
    //Camera gameCamera;
    //Ship ship;
	public List<SwarmPathAction> swarmActions { get; set;}
	SwarmPathAction currentAction;
	int currentActionPosition;
	Vector3 currentMoveTarget = Vector3.zero;

    public float health;

    public string shipName;
    public float speed;
    // Use this for initialization
    void Awake () {
        //gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //ship = new Ship("EnemyShip", speed);
    }

	// Update is called once per frame
	void Update () {

		if (currentAction == null){
			currentAction = swarmActions [0];
			currentActionPosition = 0;
		}
		//execute the current swarm action
		if (currentAction.actionType == swarmActionType.move) {

			if (currentMoveTarget == Vector3.zero) {
				computeMoveTarget ();
			}

			float step = currentAction.actionDetails.moveSpeed * Time.deltaTime;

			if (transform.position == currentMoveTarget) {
				//we're there - move to next action, or start over
				if (currentActionPosition == swarmActions.Count - 1) {
					currentAction = swarmActions[0];
					currentActionPosition = 0;
					computeMoveTarget ();
				} else {
					currentAction = swarmActions[currentActionPosition + 1];
					currentActionPosition += 1;
					computeMoveTarget ();
				}
			} else { 
				transform.position = Vector3.MoveTowards (transform.position, currentMoveTarget, step);
			}
		}
	}

	void computeMoveTarget(){
		float targetX = currentAction.actionDetails.moveTarget.x + Random.Range (-currentAction.actionDetails.moveTargetVariance, currentAction.actionDetails.moveTargetVariance);
		float targetY = currentAction.actionDetails.moveTarget.y + Random.Range (-currentAction.actionDetails.moveTargetVariance, currentAction.actionDetails.moveTargetVariance);
		currentMoveTarget = new Vector3 (targetX, targetY, 0);
	}

    public void applyDamage(float damage) {
        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}