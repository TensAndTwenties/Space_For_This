using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour {

	private IEnumerator coroutine;
	private float maxDifficultyLevel = 500; //measure of current difficulty, 0-1000
	public float currentDifficultyLevel = 0.0f;
	private List<Swarm> activeSwarms; //all active swarms
	private List<SwarmGroup> groupsToSpawn; //possible swarm groups to spwan
    bool spawnDepressed;
    public float timeToNextSwarm = 15f;

	//private GameObject spawnTarget;
	void Awake () {

		groupsToSpawn = instantiateSwarmGroups ();

        //InvokeRepeating("AssesCurrentDifficulty",10f,10f);

        Invoke("SpawnNewSwarm", 1f);
        /*
		foreach (Swarm currentSwarm in groupsToSpawn[1].swarms) {
			coroutine = SpawnEnemies (currentSwarm);
			StartCoroutine(coroutine);
		}
        */
        
	}

    void Update()
    {
        
    }

    private List<SwarmGroup> instantiateSwarmGroups(){
		//create pre-defined, designed groups of swarms to spawn
		List<SwarmGroup> groupsToReturn = new List<SwarmGroup>();
		List<Swarm> currentSwarms;
		SwarmGroup currentSwarmGroup;

		#region Fighters circling frigate
		currentSwarms = new List<Swarm>();

		Swarm fightersCircling_frigate = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0,swarmTargetType.straightAhead,swarmActionShape.figureEight);
		Swarm fightersCircling_fighters = Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.circle).ChildSwarm();
		fightersCircling_frigate.childSwarm = fightersCircling_fighters;

		currentSwarms.Add(fightersCircling_frigate);

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters circling frigate");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		#region Fighters Double Laces
		currentSwarms = new List<Swarm>();

		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,30,6,0.2f,0.2f,swarmTargetType.straightAhead,swarmActionShape.laces));
		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,30,6,0.2f,0.2f,swarmTargetType.straightAhead,swarmActionShape.laces).MirrorOverX());

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters double laces");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		#region Fighters Double Steps
		/*
		currentSwarms = new List<Swarm>();
		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.laces));
		currentSwarms.Add(Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.laces).MirrorOverX());

		currentSwarmGroup = new SwarmGroup (currentSwarms,"fighters double steps");
		groupsToReturn.Add (currentSwarmGroup);
		*/
		#endregion

		#region Fighters Circling Frigates ArcSwoop
		currentSwarms = new List<Swarm>();

		Swarm FightersCirclingFrigatesArcswoop_frigate = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0,swarmTargetType.straightAhead,swarmActionShape.arcswoop);
		Swarm FightersCirclingFrigatesArcswoop_fighters = Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.circle).ChildSwarm();
		FightersCirclingFrigatesArcswoop_frigate.childSwarm = FightersCirclingFrigatesArcswoop_fighters;

		currentSwarms.Add(FightersCirclingFrigatesArcswoop_frigate);

		Swarm FightersCirclingFrigatesArcswoop_frigate2 = Swarm.GenerateSwarmWithShape (shipType.frigate,1,6,0,0,swarmTargetType.straightAhead,swarmActionShape.arcswoop).MirrorOverX();
		Swarm FightersCirclingFrigatesArcswoop_fighters2 = Swarm.GenerateSwarmWithShape (shipType.fighter,20,8,1,0.1f,swarmTargetType.atPlayer,swarmActionShape.circle).ChildSwarm();
		FightersCirclingFrigatesArcswoop_frigate2.childSwarm = FightersCirclingFrigatesArcswoop_fighters2;

		currentSwarms.Add(FightersCirclingFrigatesArcswoop_frigate2);

		currentSwarmGroup = new SwarmGroup (currentSwarms,"Fighters Circling Frigates ArcSwoop");
		groupsToReturn.Add (currentSwarmGroup);
		#endregion

		return groupsToReturn;

	}

    private void SpawnNewSwarm()
    {
        int swarmCount = groupsToSpawn.Count;
        int swarmIndexToSpawn = Random.Range(0, swarmCount-1);
        foreach (Swarm currentSwarm in groupsToSpawn[swarmIndexToSpawn].swarms)
        {
            coroutine = SpawnEnemies(currentSwarm);
            StartCoroutine(coroutine);
        }
        if (timeToNextSwarm > 10)
        {
            timeToNextSwarm -= 1;
        }
        Invoke("SpawnNewSwarm", timeToNextSwarm);
        
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