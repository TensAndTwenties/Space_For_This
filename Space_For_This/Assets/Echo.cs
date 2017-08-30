using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LeanTween.alpha (this.gameObject, 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.GetComponent<SpriteRenderer> ().color.a == 0) {
			Destroy(this.gameObject);
		}
	}
}
