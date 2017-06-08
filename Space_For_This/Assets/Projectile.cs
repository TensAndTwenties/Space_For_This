using UnityEngine;
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
    private bool enemyProjectile = false;
    Camera gameCamera;
    public GameObject explosion;
    public bool homing;
    public Vector3 distanceBeforeHome;
    private GameObject homingTarget;
    List<GameObject> targets = new List<GameObject>();

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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            GameObject Enemy = collider.gameObject;
            Enemy.GetComponent<EnemyFighterAI>().applyDamage(damage);

            if (explosion)
            {
                GameObject newExplosion = Instantiate(explosion) as GameObject;
                newExplosion.transform.position = transform.position;
            }

            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        if (transform.position.y > gameCamera.GetComponent<CameraScript>().maxY + 1)
        {
            Destroy(this.gameObject);
        }

        float stepBase = speed * Time.deltaTime;
        float stepY = stepBase;
        float stepX = 0;

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
                transform.position = Vector3.MoveTowards(transform.position, homingTarget.transform.position, stepBase);
            }
        }
        else {
            this.transform.position = new Vector3(transform.position.x + stepX, transform.position.y + stepY, transform.position.z);
        }    
    }
}
