using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm {
	public List<Object> swarmShips { get; set;}
	public List<SwarmPathAction> swarmActions { get; set;}
	public Vector3 startingPoint { get; set;}

	public Swarm(List<Object> ships, List<SwarmPathAction> actions, Vector3 start){
		this.swarmShips = ships;
		this.swarmActions = actions;
		this.startingPoint = start;
	}

	public static Swarm GenerateTestSwarm(){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();
		List<Object> ships = new List<Object> ();
		Vector3 start = new Vector3 (-7, 11, 0);

		actions.Add (
			new SwarmPathAction (swarmActionType.move, new SwarmActionDetails (
				new Vector3(-4f,5,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (swarmActionType.move, new SwarmActionDetails (
				new Vector3(0,0,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (swarmActionType.move, new SwarmActionDetails (
				new Vector3(4f,5,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (swarmActionType.move, new SwarmActionDetails (
				new Vector3(0,9,0)
				, 6, 1))
		);

		for (int i = 0; i < 400; i++ ) {
			ships.Add (
				Resources.Load ("Enemy1")
			);
		}

		return new Swarm (ships, actions, start);
	}
}

public class SwarmPathAction {
	public swarmActionType actionType { get; set;}
	public SwarmActionDetails actionDetails { get; set;}

	public SwarmPathAction(swarmActionType type, SwarmActionDetails details)
	{
		this.actionType = type;
		this.actionDetails = details;
	}
}

public class SwarmActionDetails{
	public Vector3 moveTarget { get; set;}
	public float moveSpeed { get; set;}
	public float moveTargetVariance { get; set;}

	public SwarmActionDetails(Vector3 target, float speed = 0, float variance = 0)
	{
		this.moveTarget = target;
		this.moveSpeed = speed;
		this.moveTargetVariance = variance;
	}
}

public enum swarmActionType { move, fire, formation } 
