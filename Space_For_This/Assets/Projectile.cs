using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float angle = 0;
    public float spread;
    public GameObject prefab;
    private bool enemyProjectile = false;
    Camera gameCamera;

    void Awake()
    {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

        this.transform.position = new Vector3(transform.position.x + stepX, transform.position.y + stepY, transform.position.z);       
    }
}
