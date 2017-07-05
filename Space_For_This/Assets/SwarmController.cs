using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {

	private IEnumerator coroutine;
	private GameObject spawnTarget;
	void Awake () {
		spawnTarget = GameObject.Find ("SwarmParent");
		//make a swarm
		Swarm testSwarm = Swarm.GenerateSwarmWithShape ("Enemy1",100,6,1,0.3f,swarmActionShape.figureEight);

		//instantiate swarm ships
		Vector3 spawnVector = testSwarm.startingPoint;

		Swarm parentSwarm = Swarm.GenerateSwarmWithShape ("Enemy1",1,8,1,0.3f,swarmActionShape.diamond);

		spawnTarget.GetComponent<EnemyFighterAI> ().swarmActions = parentSwarm.swarmActions;

		//coroutine = SpawnEnemies (testSwarm);
		//StartCoroutine(coroutine);

		//testSwarm = Swarm.GenerateSwarmWithShape ("Enemy1",100,8,1,0.3f,swarmActionShape.diamond);
		//spawnVector = new Vector3 (-testSwarm.startingPoint.x, testSwarm.startingPoint.y);

		coroutine = SpawnEnemies (testSwarm);
		StartCoroutine(coroutine);

	}

	private IEnumerator SpawnEnemies(Swarm testSwarm)
	{
		Vector3 spawnVector = testSwarm.startingPoint;
		foreach (Object enemy in testSwarm.swarmShips) {
			GameObject newEnemy = Instantiate (enemy) as GameObject;
			newEnemy.transform.parent = spawnTarget.transform;
			newEnemy.transform.position = spawnVector;
			newEnemy.GetComponent<EnemyFighterAI> ().swarmActions = testSwarm.swarmActions;
			yield return new WaitForSeconds(Random.Range (0f, testSwarm.spawnVariance));
		}

	}
} 