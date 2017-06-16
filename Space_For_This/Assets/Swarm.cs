using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm {
	public List<Object> swarmShips { get; set;}
	public List<SwarmPathAction> swarmActions { get; set;}
	public Vector3 startingPoint { get; set;}
	public float spawnVariance { get; set;}

	public Swarm(List<Object> ships, List<SwarmPathAction> actions, Vector3 start, float _spawnVariance = 0){
		this.swarmShips = ships;
		this.swarmActions = actions;
		this.startingPoint = start;
		this.spawnVariance = _spawnVariance;
	}

	public static Swarm GenerateTestSwarm(){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();
		List<Object> ships = new List<Object> ();
		Vector3 start = new Vector3 (-7, 11, 0);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,0,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(4f,5,0)
				, 6, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,9,0)
				, 6, 1))
		);

		for (int i = 0; i < 200; i++ ) {
			ships.Add (
				Resources.Load ("Enemy1")
			);
		}

		float spawnVariance = 0.3f;
		return new Swarm (ships, actions, start, spawnVariance);
	}
}

public class SwarmPathAction {
	public swarmActionType actionType { get; set;}
	public SwarmMoveDetails moveDetails { get; set;}
	public SwarmFireDetails fireDetails { get; set;}

	public SwarmPathAction(SwarmMoveDetails _moveDetails)
	{
		this.actionType = swarmActionType.move;
		this.moveDetails = _moveDetails;
		this.fireDetails = null;
	}

	public SwarmPathAction(SwarmFireDetails _fireDetails)
	{
		this.actionType = swarmActionType.fire;
		this.fireDetails = _fireDetails;
		this.moveDetails = null;
	}


}

public class SwarmMoveDetails{
	public Vector3 moveTarget { get; set;}
	public float moveSpeed { get; set;}
	public float moveTargetVariance { get; set;}

	public SwarmMoveDetails(Vector3 target, float speed = 0, float variance = 0)
	{
		this.moveTarget = target;
		this.moveSpeed = speed;
		this.moveTargetVariance = variance;
	}
}

public class SwarmFireDetails{
	public swarmTargetType targetType { get; set;}
	public int[] fireWeapons { get; set;}
	public float fireTargetVariance { get; set;}

	public SwarmFireDetails(swarmTargetType _targetType, int[] _fireWeapons, float _fireTargetVariance = 0)
	{
		this.targetType = _targetType;
		this.fireWeapons = _fireWeapons;
		this.fireTargetVariance = _fireTargetVariance;
	}
}

public enum swarmActionType { move, fire, formation } 
public enum swarmTargetType { straightAhead, atPlayer}