using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    public float expansionRate;
    public float expansionMax;
    public float damage;
    // Use this for initialization
    SphereCollider collider;
	void Awake () {
	    collider = this.gameObject.GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            GameObject Enemy = collider.gameObject;
			Enemy.GetComponent<EnemyFighterAI>().ship.applyDamage(damage);
        }
    }

    // Update is called once per frame
    void Update () {
        float step = expansionRate * Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x + step, transform.localScale.y + step, transform.localScale.z + step) ;

        if ((transform.localScale.x + step) > expansionMax)
        {
            Destroy(gameObject);
        }
    }
}
