using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBacteriaManager : MonoBehaviour {

	public static List<GameObject> listEnemy = new List<GameObject>();
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3 (UnityEngine.Random.Range (-250.0f, 250.0f),
												UnityEngine.Random.Range (-150.0f, 400.0f),
												0f);

		listEnemy.Add (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Player") {
			listEnemy.Remove (this.gameObject);
			Destroy (this.gameObject);
		}
	}
}
