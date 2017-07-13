using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm {
	public List<Object> swarmShips { get; set;}
	public List<SwarmPathAction> swarmActions { get; set;}
	public Vector3 startingPoint { get; set;}
	public float spawnVariance { get; set;}
	public Object swarmParent { get; set;}

	public Swarm(List<Object> ships, List<SwarmPathAction> actions, Vector3 start, float _spawnVariance = 0, Object _swarmParent = null){
		this.swarmShips = ships;
		this.swarmActions = actions;
		this.startingPoint = start;
		this.spawnVariance = _spawnVariance;
		this.swarmParent = _swarmParent;
	}

//	public static Swarm GenerateSwarmFromPattern(){
		//generate a swarm from one of several pre-defined patterns
		//enemyShipName -- selects swarm member type
		//size -- number of members
		//spawn variance -- delay between member spawn
		//move variance -- delay between moves
		//targetType -- type of targeting pattern - at player, straightahead, etc
//	}

//	public static Swarm GenerateRandomSwarmWithEnemy(string enemyName, int aggressiveness)
//	{
		//generate a random path and random firing pattern using the specified ship.
		//ensure proper spacing between move points
		//ensure proper number of firings via aggressiveness variable - controls firing vs. move actions
//	}

	public static Swarm GenerateSwarmWithShape(shipType type, int size, float moveSpeed, float moveVariance, float spawnVariance, swarmActionShape shape){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();
		List<Object> enemies = new List<Object> ();
		Object parent = null;

		Vector3 start = new Vector3 (-7, 11, 0);

		switch (shape) {
		case swarmActionShape.figureEight:
			actions = FigureEight (moveSpeed, moveVariance);
			break;
		case swarmActionShape.diamond:
			actions = Diamond (moveSpeed, moveVariance);
			break;
		case swarmActionShape.test:
			actions = Test (moveSpeed, moveVariance);
			break;
		}
			
		for (int i = 0; i < size; i++ ) {
			enemies.Add (
				Resources.Load (type.ToString())
			);
		}

		if (enemies.Count == 1) {
			enemies.Add (
				Resources.Load (shipType.dummy.ToString())
			);
			parent = enemies[1];
		}

		return new Swarm (enemies, actions, start, spawnVariance, parent);
	}

	private static List<SwarmPathAction> Test(float moveSpeed, float moveVariance){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		Vector3[] bezierVectors = new Vector3[4];
		bezierVectors [0] = new Vector3 (-4,5,0); //StartPoint
		bezierVectors [1] = new Vector3 (4,1,0); //EndControl
		bezierVectors [2] = new Vector3 (-4, 1,0); //StartControl
		bezierVectors [3] = new Vector3 (4, 5,0); //EndPoint

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				bezierVectors
				, moveSpeed, moveVariance))
		);

		bezierVectors [0] = new Vector3 (4,5,0);
		bezierVectors [1] = new Vector3 (-4,9,0);
		bezierVectors [2] = new Vector3 (4, 9,0);
		bezierVectors [3] = new Vector3 (-4, 5,0);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				bezierVectors
				, moveSpeed, moveVariance))
		);

		return actions;
	}

	private static List<SwarmPathAction> FigureEight(float moveSpeed, float moveVariance){
	
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-2f,2.5f,0)
				, moveSpeed, moveVariance))
		);
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,5,0)
				, moveSpeed, moveVariance))
		);
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(2,7.5f,0)
				, moveSpeed, moveVariance))
		);
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(4,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(2,2.5f,0)
				, moveSpeed, moveVariance))
		);
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,5,0)
				, moveSpeed, moveVariance))
		);
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-2f,7.5f,0)
				, moveSpeed, moveVariance))
		);

		return actions;
	}

	private static List<SwarmPathAction> Diamond(float moveSpeed, float moveVariance){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1] { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,0,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(4f,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				swarmTargetType.atPlayer
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,9,0)
				, moveSpeed, moveVariance))
		);
			
		return actions;
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
	public bool bezier { get; set; }
	public Vector3[] bezierVectors { get; set;}

	public SwarmMoveDetails(Vector3 target, float speed = 0, float variance = 0)
	{
		this.moveTarget = target;
		this.moveSpeed = speed;
		this.moveTargetVariance = variance;
	}

	public SwarmMoveDetails(Vector3[] bezierVectors, float speed = 0, float variance = 0)
	{
		this.moveTarget = bezierVectors[bezierVectors.Length-1];
		this.bezierVectors = bezierVectors;
		this.bezier = true;
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
public enum swarmActionShape { figureEight, diamond, test }
public enum shipType { fighter, frigate, drone, dummy, component, playerShip1 }
public enum componentType { missle, shield, rail }