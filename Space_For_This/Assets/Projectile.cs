﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public float angle;
    [HideInInspector]
    public float spread;
    public float damage;
    public GameObject prefab;
    public bool enemyProjectile = false;
    Camera gameCamera;
    public GameObject explosion;
    public bool homing;
    public Vector3 distanceBeforeHome;
    private GameObject homingTarget;
    List<GameObject> targets = new List<GameObject>();
	public swarmTargetType targetType;
	public Vector3 dumbTarget;
	private bool isHoming; //to let us know if a scripted homing motion is in progress
	float stepBase;
	float stepY = 0;
	float stepX = 0;

    void Awake()
    {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (homing)
        {
            GameObject[] targetsArray = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject target in targetsArray)
            {
                targets.Add(target);
            }

            if (targets.Count > 0)
            {
                int homingTargetIndex = Random.Range(0, targets.Count);
                homingTarget = targets[homingTargetIndex];
            }
        }
    }

	void OnTriggerEnter2D(Collider2D collider)
    {
		GameObject target = collider.gameObject;
		if (target.tag == "Enemy" && !enemyProjectile) {
			
			target.GetComponent<EnemyFighterAI> ().applyDamage (damage);

			DestroyProjectile ();
		} else if (target.tag == "Player" && enemyProjectile) {
			if (!(target.GetComponent<Player_Controller> ().currentState == PlayerState.dodging)) {
				target.GetComponent<Player_Controller> ().playerShip.applyDamage (damage);

				DestroyProjectile ();
			}
		}
    }

	void DestroyProjectile(){
		if (explosion) {
			GameObject newExplosion = Instantiate (explosion) as GameObject;
			newExplosion.transform.position = transform.position;
		}

		Destroy (this.gameObject);
	}


    void Update()
    {
		if (transform.position.y > gameCamera.GetComponent<CameraScript>().maxY + 1 || transform.position.y < gameCamera.GetComponent<CameraScript>().minY - 1 || transform.position.x > gameCamera.GetComponent<CameraScript>().maxX + 1 || transform.position.x < gameCamera.GetComponent<CameraScript>().minX - 1)
        {
            Destroy(this.gameObject);
        }

        stepBase = speed * Time.deltaTime;
        stepY = stepBase;
        stepX = 0;

        if (angle != 0)
        {
            float radians = angle * Mathf.Deg2Rad;
            var ca = Mathf.Cos(radians);
            var sa = Mathf.Sin(radians);

            stepX = ca * stepBase - sa * stepBase;
            stepY = sa * stepBase + ca * stepBase;
        }

        if (homing)
        {
            if (homingTarget == null)
            {
                if (targets.Count > 0)
                {
                    int homingTargetIndex = Random.Range(0, targets.Count);
                    homingTarget = targets[homingTargetIndex];
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            else
            {
				if (!isHoming){
					IEnumerator coroutine = ExecuteHomingArc (homingTarget);
					StartCoroutine (coroutine); // execute arcing, then head towards enemies
				}
            }
        }
        else {
			if (enemyProjectile && targetType == swarmTargetType.straightAhead) {
				this.transform.position = new Vector3(transform.position.x + stepX, transform.position.y - stepY, transform.position.z);
			} else if (enemyProjectile && targetType == swarmTargetType.atPlayer){
				transform.position = Vector3.MoveTowards (transform.position, dumbTarget, stepBase);
			} else{
            	this.transform.position = new Vector3(transform.position.x + stepX, transform.position.y + stepY, transform.position.z);
			}
		}    
    }

	private IEnumerator ExecuteHomingArc(GameObject target){
		isHoming = true;
		float xPos = this.transform.position.x;
		float yPos = this.transform.position.y;
		float arcTime = 1.2f;
		Vector3[] bezierVectors = new Vector3[4];
		bezierVectors [0] = new Vector3 (xPos,yPos,0); //StartPoint
		bezierVectors [1] = new Vector3 (xPos-4,yPos,0); //EndControl
		bezierVectors [2] = new Vector3 (xPos-1,yPos,0); //StartControl
		bezierVectors [3] = new Vector3 (xPos-4,yPos+4,0); //EndPoint

		LeanTween.move (this.gameObject, bezierVectors, arcTime).setRepeat(0);
		yield return new WaitForSeconds (arcTime);
		LeanTween.move (this.gameObject, target.transform, arcTime).setRepeat(0);


	}
}
