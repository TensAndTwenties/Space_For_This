using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public bool firing;
    public FireStream[] fireStreams;
    GameObject playerShip;
	// Use this for initialization
	void Awake () {
        firing = false;
        playerShip = GameObject.Find("player");
    }

    public Weapon(FireStream[] _fireStreams) {
        fireStreams = _fireStreams;
    }
}



public class FireStream {
    public float fireRate { get; set; }
    public float currentCooldown { get; set; }
    public Projectile projectile { get; set; }
    public Vector3 offset { get; set; }
    public float angleOffset { get; set; }
    public float spread { get; set; }

    public FireStream(float _fireRate, Projectile _projectile, Vector3 _offset, float _angleOffset = 0, float _spread = 0) {
        fireRate = _fireRate;
        projectile = _projectile;
        offset = _offset;
        angleOffset = _angleOffset;
        spread = _spread;
    }

}

