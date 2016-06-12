using UnityEngine;
using System.Collections;

public class EnemyFighterAI : MonoBehaviour {
    Vector3 targetVector;
    Random rand;
    Camera gameCamera;
    Ship ship;
    // Use this for initialization
    void Awake () {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        ship = new Ship("EnemyShip", 3);
        generateTarget();
    }

    void generateTarget() {
        Random rand = new Random();
        float targetX = Random.Range(gameCamera.GetComponent<CameraScript>().minX, gameCamera.GetComponent<CameraScript>().maxX);
        float targetY = Random.Range(gameCamera.GetComponent<CameraScript>().minY + 12, gameCamera.GetComponent<CameraScript>().maxY);
        targetVector = new Vector3(targetX, targetY, 0);
    }
	// Update is called once per frame
	void Update () {
        float step = ship.shipSpeed * Time.deltaTime;

        if (transform.position == targetVector)
        {
            generateTarget();
        } else { 
            transform.position = Vector3.MoveTowards(transform.position, targetVector, step);
        }
	}
}
