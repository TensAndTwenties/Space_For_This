using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm {
	public List<Object> swarmShips { get; set;}
	public List<SwarmPathAction> swarmActions { get; set;}
	public Vector3 startingPoint { get; set;}
	public float spawnVariance { get; set;}
	public Object swarmParent { get; set;}
	public Swarm childSwarm { get; set;}

	public Swarm(List<Object> ships, List<SwarmPathAction> actions, Vector3 start, float _spawnVariance = 0, Object _swarmParent = null){
		this.swarmShips = ships;
		this.swarmActions = actions;
		this.startingPoint = start;
		this.spawnVariance = _spawnVariance;
		this.swarmParent = _swarmParent;
	}
		
	public static Swarm GenerateSwarmWithShape(shipType type, int size, float moveSpeed, float moveVariance, float spawnVariance, swarmTargetType targetType, swarmActionShape shape){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();
		List<Object> enemies = new List<Object> ();
		Object parent = null;

		Vector3 start = new Vector3 (-7, 11, 0);

		switch (shape) {
		case swarmActionShape.figureEight:
			actions = FigureEight (moveSpeed, moveVariance, targetType);
			break;
		case swarmActionShape.diamond:
			actions = Diamond (moveSpeed, moveVariance, targetType);
			break;
		case swarmActionShape.circle:
			actions = Circle (moveSpeed, moveVariance, targetType);
			break;
		case swarmActionShape.laces:
			actions = Laces (moveSpeed, moveVariance, targetType);
			break;
		case swarmActionShape.arcswoop:
			actions = ArcSwoop (moveSpeed, moveVariance, targetType);
			break;
		}
			
		//check for errors, incompatabailities:
		foreach (SwarmPathAction action in actions) {
			if (action.moveDetails != null) {
				if (action.moveDetails.moveActionType == swarmMoveActionType.bezier && action.moveDetails.moveTargetVariance != 0) {
				//	throw new System.ArgumentException("Swarm Path includes a bezier curve with non-zero moveTargetVariance");
				}
			}
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

	private static List<SwarmPathAction> Circle(float moveSpeed, float moveVariance, swarmTargetType targetType){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		/*
		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, moveSpeed, moveVariance))
		);
	*/
		
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

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1] { 0 }, 1))
		);

		bezierVectors = new Vector3[4];
		bezierVectors [0] = new Vector3 (4,5,0);
		bezierVectors [1] = new Vector3 (-4,9,0);
		bezierVectors [2] = new Vector3 (4, 9,0);
		bezierVectors [3] = new Vector3 (-4, 5,0);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				bezierVectors
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1] { 0 }, 1))
		);

		return actions;
	}

	private static List<SwarmPathAction> ArcSwoop(float moveSpeed, float moveVariance, swarmTargetType targetType){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,9,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,7,0)
				, moveSpeed, moveVariance))
		);
			
		Vector3[] bezierVectors = new Vector3[4];
		bezierVectors [0] = new Vector3 (-4,7,0);
		bezierVectors [1] = new Vector3 (4,9,0);
		bezierVectors [2] = new Vector3 (-4, 5,0);
		bezierVectors [3] = new Vector3 (4, 7,0);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				bezierVectors
				, moveSpeed, moveVariance))
		);

		bezierVectors = new Vector3[4];
		bezierVectors [0] = new Vector3 (4,7,0);
		bezierVectors [1] = new Vector3 (-4,0,0);
		bezierVectors [2] = new Vector3 (4, 0,0);
		bezierVectors [3] = new Vector3 (-4, 7,0);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				bezierVectors
				, moveSpeed, moveVariance))
		);

		return actions;
	}
		
	private static List<SwarmPathAction> Laces(float moveSpeed, float moveVariance, swarmTargetType targetType){

		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,9,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(4f,8,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-3f,7,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(3f,6,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-2f,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(2f,4,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-1f,3,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(1f,2,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,1,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
				, new int[1]  { 0 }, 1))
		);

		return actions;
	}

	private static List<SwarmPathAction> FigureEight(float moveSpeed, float moveVariance, swarmTargetType targetType){
	
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, moveSpeed, moveVariance))
		);

		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
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
				targetType
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

	private static List<SwarmPathAction> Diamond(float moveSpeed, float moveVariance, swarmTargetType targetType){
		List<SwarmPathAction> actions = new List<SwarmPathAction> ();

		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(-4f,5,0)
				, moveSpeed, moveVariance))
		);


		actions.Add (
			new SwarmPathAction (new SwarmFireDetails (
				targetType
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
				targetType
				, new int[1]  { 0 }, 1))
		);


		actions.Add (
			new SwarmPathAction (new SwarmMoveDetails (
				new Vector3(0,9,0)
				, moveSpeed, moveVariance))
		);
			
		return actions;
	}

	public Swarm MirrorOverX(){
		foreach (SwarmPathAction action in swarmActions) {
			if (action.actionType == swarmActionType.move) {
				if(action.moveDetails.moveActionType == swarmMoveActionType.linear){
					//flip over x axis
					action.moveDetails.moveTarget = new Vector3(-1*action.moveDetails.moveTarget.x,action.moveDetails.moveTarget.y);
				}
			}
		}
		this.startingPoint = new Vector3 (-1 * this.startingPoint.x, this.startingPoint.y);
		return this;
	}

	public Swarm ChildSwarm(){
		//alter swarm so that it treats 0,5 as origin
		foreach (SwarmPathAction action in swarmActions) {
			if (action.actionType == swarmActionType.move) {
				if (action.moveDetails.moveActionType == swarmMoveActionType.linear) {
					//flip over x axis
				} else if (action.moveDetails.moveActionType == swarmMoveActionType.bezier) {
					int counter = 0;
					foreach (Vector3 vector in action.moveDetails.bezierVectors) {
						action.moveDetails.bezierVectors [counter] = new Vector3 (action.moveDetails.bezierVectors [counter].x, action.moveDetails.bezierVectors [counter].y - 5);
						counter += 1;
					}
				}

				action.moveDetails.moveTarget = new Vector3 (action.moveDetails.moveTarget.x, action.moveDetails.moveTarget.y - 5);
			}
		}
		return this;
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
	public swarmMoveActionType moveActionType { get; set; }

	public SwarmMoveDetails(Vector3 target, float speed = 0, float variance = 0)
	{
		this.moveTarget = target;
		this.moveSpeed = speed;
		this.moveTargetVariance = variance;
		this.moveActionType = swarmMoveActionType.linear;
	}

	public SwarmMoveDetails(Vector3[] bezierVectors, float speed = 0, float variance = 0)
	{
		this.moveTarget = bezierVectors[bezierVectors.Length-1];
		this.bezierVectors = bezierVectors;
		this.bezier = true;
		this.moveSpeed = speed;
		this.moveTargetVariance = variance;
		this.moveActionType = swarmMoveActionType.bezier;
	}
}

public class SwarmFireDetails{
	public swarmTargetType targetType { get; set;}
	public int[] fireWeapons { get; set;}
	public float fireTargetVariance { get; set;}
	public int firings { get; set; }

	public SwarmFireDetails(swarmTargetType _targetType, int[] _fireWeapons, int _firings = 1, float _fireTargetVariance = 0)
	{
		this.targetType = _targetType;
		this.fireWeapons = _fireWeapons;
		this.fireTargetVariance = _fireTargetVariance;
		this.firings = _firings;
	}
}

public class SwarmGroup{
	public string description { get; set; }
	public List<Swarm> swarms { get; set; }

	public SwarmGroup(List<Swarm> _swarms, string _description = ""){
		description = _description;
		swarms = _swarms;
	}
}

public enum swarmMoveActionType { linear, bezier, rotation}
public enum swarmActionType { move, fire, formation } 
public enum swarmTargetType { straightAhead, atPlayer}
public enum swarmActionShape { figureEight, diamond, circle, laces, arcswoop}
public enum shipType { fighter, frigate, drone, dummy, component, playerShip1 }
public enum componentType { missile, shield, rail }