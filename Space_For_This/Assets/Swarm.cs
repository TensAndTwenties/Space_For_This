using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm : MonoBehaviour {
	List<Ship> swarmShips;
	List<SwarmPathAction> swarmActions;

	public Swarm(List<Ship> ships, List<SwarmPathAction> actions){
		this.swarmShips = ships;
		this.swarmActions = actions;
	}

	public Swarm GenerateTestSwarm(){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();
		List<Ship> ships = new List<Ship> ();

		actions.Add (
			new SwarmPathAction (swarmActionType.move, new SwarmActionDetails (Vector3.zero, 3))
		);

		return new Swarm (ships, actions);
	}
}

public class SwarmPathAction {
	swarmActionType actionType;
	SwarmActionDetails actionDetails;

	public SwarmPathAction(swarmActionType type, SwarmActionDetails details)
	{
		this.actionType = type;
		this.actionDetails = details;
	}
}

public class SwarmActionDetails{
	Vector3 moveTarget;
	float moveSpeed;

	public SwarmActionDetails(Vector3 target, float speed = 0)
	{
		this.moveTarget = target;
		this.moveSpeed = speed;
	}
}

public enum swarmActionType { move, fire, formation } 
