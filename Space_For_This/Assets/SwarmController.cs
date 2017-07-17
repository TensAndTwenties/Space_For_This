using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {

	private IEnumerator coroutine;
	private float difficultyLevel = 5; //measure of current difficulty, 0-10
	private List<Swarm> activeSwarms; //all active swarms
	private List<SwarmGroup> groupsToSpawn; //possible swarm groups to spwan

	//private GameObject spawnTarget;
	void Awake () {
		//spawnTarget = GameObject.Find ("SwarmParent");
		//make a swarm

		groupsToSpawn = instantiateSwarmGroups ();
		//Swarm testSwarm = Swarm.GenerateSwarmWithShape (shipType.fighter,70,8,1,0.8f,swarmActionShape.figureEight);

		//Swarm testSwarm = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0f,swarmActionShape.diamond);
		//instantiate swarm ships

		//Swarm parentSwarm = Swarm.GenerateSwarmWithShape ("Enemy1",1,8,1,0.3f,swarmActionShape.diamond);

		//spawnTarget.GetComponent<EnemyFighterAI> ().swarmActions = parentSwarm.swarmActions;

		//coroutine = SpawnEnemies (testSwarm);
		//StartCoroutine(coroutine);

		//testSwarm = Swarm.GenerateSwarmWithShape ("Enemy1",100,8,1,0.3f,swarmActionShape.diamond);
		//spawnVector = new Vector3 (-testSwarm.startingPoint.x, testSwarm.startingPoint.y);

		foreach (Swarm currentSwarm in groupsToSpawn[0].swarms) {
			coroutine = SpawnEnemies (currentSwarm);
			StartCoroutine(coroutine);
		}

	}

	private List<SwarmGroup> instantiateSwarmGroups(){
		//create pre-defined, designed groups of swarms to spawn
		List<SwarmGroup> groupsToReturn = new List<SwarmGroup>();

		//fighters circling frigate
		List<Swarm> currentSwarms = new List<Swarm>();

		Swarm frigate = Swarm.GenerateSwarmWithShape (shipType.frigate,2,6,0,3f,swarmActionShape.figureEight);
		//Swarm frigate2 = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0f,swarmActionShape.test);
		//Swarm fighters = Swarm.GenerateSwarmWithShape (shipType.fighter,150,8,1,0.1f,swarmActionShape.figureEight);
		//frigate.childSwarm = fighters;

		currentSwarms.Add(frigate);
		//currentSwarms.Add(frigate2);
		//currentSwarms.Add(fighters);

		SwarmGroup current = new SwarmGroup (currentSwarms,"fighters circling frigate");
		groupsToReturn.Add (current);

		return groupsToReturn;

	}

	private void AssesCurrentDifficulty(){
		//invoked repeatedly

	}

	private IEnumerator SpawnEnemies(Swarm currentSwarm)
	{
		Vector3 spawnVector = currentSwarm.startingPoint;
		foreach (Object enemy in currentSwarm.swarmShips) {
			GameObject newEnemy = Instantiate (enemy) as GameObject;

			if (currentSwarm.swarmParent != null) {
				//get the dummy of the parent swarm and assign this swarm as the child
			}

			newEnemy.transform.position = spawnVector;
			newEnemy.GetComponent<EnemyFighterAI> ().swarmActions = currentSwarm.swarmActions;
			yield return new WaitForSeconds(Random.Range (0f, currentSwarm.spawnVariance));
		}

	}
} 