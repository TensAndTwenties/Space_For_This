using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scrap : MonoBehaviour 
{
	public int scrapAmount { get; set; }
	public float moveSpeed;
	public Vector3 moveTarget { get; set; }
	Camera gameCamera;

	// Use this for initialization
	void Awake ()
	{
		gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		//pick a random target behind the player
		moveTarget = new Vector3 (Random.Range (-5, 5), -11);
		LeanTween.rotateZ (this.gameObject, 360, 1f); 
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject target = collider.gameObject;
		if (target.tag == "Player") {
			target.GetComponent<Player_Controller> ().currentScrap += this.scrapAmount;
			Destroy(this.gameObject);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		float stepBase = moveSpeed * Time.deltaTime;

		if (transform.position.y > gameCamera.GetComponent<CameraScript>().maxY + 1 || transform.position.y < gameCamera.GetComponent<CameraScript>().minY - 1 || transform.position.x > gameCamera.GetComponent<CameraScript>().maxX + 1 || transform.position.x < gameCamera.GetComponent<CameraScript>().minX - 1)
		{
			Destroy(this.gameObject);
		}

		transform.position = Vector3.MoveTowards(transform.position, moveTarget, stepBase);
	}
}

