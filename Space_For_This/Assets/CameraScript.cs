using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    Camera gameCamera;
    // Use this for initialization
    void Start () {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        minX = gameCamera.transform.position.x - gameCamera.orthographicSize;
        maxX = gameCamera.transform.position.x + gameCamera.orthographicSize;
        minY = gameCamera.transform.position.y - gameCamera.orthographicSize * 2;
        maxY = gameCamera.transform.position.y + gameCamera.orthographicSize * 2;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
