using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyManager : MonoBehaviour {

	int hp = 100;
	int direction = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float moveSpeed = Time.deltaTime * 0.6f;

		transform.Translate (0, direction * moveSpeed, 0);

		if (transform.localPosition.y > 600) {
			direction = -1;
		}
		if (transform.localPosition.y < -600) {
			direction = 1;
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Bullet") {
			hp --;
			if (hp < 1) {
				Destroy (this.gameObject);
			}
		}
	}
}
