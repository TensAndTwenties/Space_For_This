using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{

	public float damage;
	public GameObject prefab;
	public bool enemyBeam = false;

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject target = collider.gameObject;
		if (target.tag == "Enemy" && !enemyBeam) {

			target.GetComponent<EnemyFighterAI> ().applyDamage (damage);

		} else if (target.tag == "Player" && enemyBeam) {
			if (!(target.GetComponent<Player_Controller> ().currentState == PlayerState.dodging)) {
				target.GetComponent<Player_Controller> ().playerShip.applyDamage (damage);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

