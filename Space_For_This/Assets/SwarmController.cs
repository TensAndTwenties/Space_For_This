using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {
	void Awake () {
		//make a swarm
		Swarm testSwarm = Swarm.GenerateTestSwarm ();

		//instantiate swarm ships
		Vector3 spawnVector = testSwarm.startingPoint;

		foreach (Object enemy in testSwarm.swarmShips) {
			GameObject newEnemy = Instantiate(enemy) as GameObject;
			newEnemy.transform.position = spawnVector;
			newEnemy.GetComponent<EnemyFighterAI>().swarmActions = testSwarm.swarmActions;
		}
			

	}
} 