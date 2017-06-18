﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {

	private IEnumerator coroutine;

	void Awake () {
		//make a swarm
		//Swarm testSwarm = Swarm.GenerateTestSwarm ();
		Swarm testSwarm = Swarm.GenerateFigureEight ("Enemy1",100,6,1,0.3f);

		if (testSwarm.spawnVariance > 0) {
			Random rand = new Random ();
		}

		//instantiate swarm ships
		Vector3 spawnVector = testSwarm.startingPoint;

		coroutine = SpawnEnemies (testSwarm);
		StartCoroutine(coroutine);

	}

	private IEnumerator SpawnEnemies(Swarm testSwarm)
	{
		Vector3 spawnVector = testSwarm.startingPoint;
		foreach (Object enemy in testSwarm.swarmShips) {
			GameObject newEnemy = Instantiate (enemy) as GameObject;
			newEnemy.transform.position = spawnVector;
			newEnemy.GetComponent<EnemyFighterAI> ().swarmActions = testSwarm.swarmActions;
			yield return new WaitForSeconds(Random.Range (0f, testSwarm.spawnVariance));
		}

	}
} 