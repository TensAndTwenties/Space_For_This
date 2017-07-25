using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {

	private IEnumerator coroutine;
	private float maxDifficultyLevel = 500; //measure of current difficulty, 0-1000
	public float currentDifficultyLevel = 0.0f;
	private List<Swarm> activeSwarms; //all active swarms
	private List<SwarmGroup> groupsToSpawn; //possible swarm groups to spwan

	//private GameObject spawnTarget;
	void Awake () {
		//spawnTarget = GameObject.Find ("SwarmParent");
		//make a swarm

		groupsToSpawn = instantiateSwarmGroups ();
		//Swarm testSwarm = Swarm.GenerateSwarmWithShape (shipType.fighter,70,8,1,0.8f,swarmActionShape.figureEight);

		//InvokeRepeating("AssesCurrentDifficulty",10f,10f);

		//Swarm currentSwarm = Swarm.GenerateSwarmWithShape (shipType.fighter, 20, 8, 0.5f, 0.2f, swarmActionShape.lacesLeft);

		foreach (Swarm currentSwarm in groupsToSpawn[2].swarms) {
			coroutine = SpawnEnemies (currentSwarm);
			StartCoroutine(coroutine);
		}

	}

	private List<SwarmGroup> instantiateSwarmGroups(){
		//create pre-defined, designed groups of swarms to spawn
		List<SwarmGroup> groupsToReturn = new List<SwarmGroup>();
		List<Swarm> currentSwarms;
		SwarmGroup currentSwarmGroup;

		#region Fighters circling frigate
		currentSwarms = new List<Swarm>();

		Swarm frigate = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0,swarmTargetType.straightAhead,swarmActionShape.figureEight);
		Swarm fighters = Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.circle);
		frigate.childSwarm = fighters;

		currentSwarms.Add(frigate);

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters circling frigate");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		#region Fighters Double Laces
		currentSwarms = new List<Swarm>();

		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,30,6,0.2f,0.2f,swarmTargetType.straightAhead,swarmActionShape.lacesLeft));
		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,30,6,0.2f,0.2f,swarmTargetType.straightAhead,swarmActionShape.lacesLeft).MirrorOverX());

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters double laces");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		#region Fighters Double Steps
		currentSwarms = new List<Swarm>();
		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.circle));

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters double laces");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		return groupsToReturn;

	}

	private void AssesCurrentDifficulty(){
		//invoked repeatedly
		float currentTotalDifficulty = 0.0f;
		GameObject[] currentEnemies =  GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in currentEnemies) {
				float shipDifficulty = 0.0f;

			if (enemy.GetComponent<EnemyFighterAI>().isComponent) {
					//component difficulty calculated seperately
				switch (enemy.GetComponent<EnemyFighterAI>().componentType) {
					case componentType.missile:
						shipDifficulty = 5;
						break;
					case componentType.shield:
						shipDifficulty = 7;
						break;
					case componentType.rail:
						shipDifficulty = 5;
						break;
					}
				} else {
					//ship difficulty
				switch (enemy.GetComponent<EnemyFighterAI>().shipType) {
					case shipType.fighter:
						shipDifficulty = 2f;
						break;
					case shipType.drone:
						shipDifficulty = 1f;
						break;
					case shipType.frigate:
						shipDifficulty = 10f;
						break;
					}
				}
			currentTotalDifficulty += shipDifficulty;
			}
		
		currentDifficultyLevel = currentTotalDifficulty;

		if (currentDifficultyLevel < maxDifficultyLevel * 0.7f) {
			//spawn more
			int swarmTypeRoll = Random.Range(1,2);

			switch (swarmTypeRoll) {
			case 1:
				//spawn random pre-defined group


				var values = componentType.GetValues (typeof(componentType));
				swarmActionShape randomShape = (swarmActionShape)values.GetValue (Random.Range (0, values.Length));
				float spawnVariance = Random.Range (0.2f, 0.8f);
				float moveVariance = Random.Range (0.1f, 0.5f);
				int numberOfShips = Random.Range (20, 80);

				Swarm currentSwarm = Swarm.GenerateSwarmWithShape (shipType.fighter,numberOfShips,8,moveVariance,spawnVariance,swarmTargetType.atPlayer,randomShape);
				coroutine = SpawnEnemies (currentSwarm);
				StartCoroutine(coroutine);
				break;
			case 2:
				//spawn random swarm from pattern
				break;
			}
		}
	}

	private IEnumerator SpawnEnemies(Swarm currentSwarm, GameObject swarmParent = null)
	{
		GameObject currentDummy = null;
		
		Vector3 spawnVector = currentSwarm.startingPoint;
		foreach (Object enemy in currentSwarm.swarmShips) {
			GameObject newEnemy = Instantiate (enemy) as GameObject;

			if (newEnemy.GetComponent<EnemyFighterAI> ().ship.shipType == shipType.dummy) {
				currentDummy = newEnemy;
			}

			if (swarmParent != null) {
				newEnemy.transform.parent = swarmParent.transform;
			}

			newEnemy.transform.position = spawnVector;
			newEnemy.GetComponent<EnemyFighterAI> ().swarmActions = currentSwarm.swarmActions;
			yield return new WaitForSeconds(Random.Range (0f, currentSwarm.spawnVariance));
		}

		if (currentSwarm.childSwarm != null && currentDummy != null) {
			IEnumerator coroutine = SpawnEnemies (currentSwarm.childSwarm, currentDummy);
			StartCoroutine(coroutine);
		}

	}
} 