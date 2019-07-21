using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerManager : MonoBehaviour {

	int scaleDirection = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D (Collision2D collision) {
		// 敵との接触時
		if (collision.gameObject.tag == "Enemy") {
			//Destroy (this.gameObject);
		}
	}

	// 大きさ変えるやつ
	
	/* 
	void ChangeScale() {
		float changeScale = Time.deltaTime * UnityEngine.Random.Range (0.8f, 2.0f) * scaleDirection;
		this.gameObject.transform.localScale = this.gameObject.transform.localScale + new Vector3 (changeScale, changeScale, 0f);


		var nowScaleX = this.gameObject.transform.localScale.x;
		if (this.gameObject.transform.localScale.x > 1.2f) {
			//this.gameObject.transform.localScale = new Vector3 (1.6f, 1.6f, 0f);
			scaleDirection = -1;
		} else if (this.gameObject.transform.localScale.x < 0.4f) {
			// this.gameObject.transform.localScale = new Vector3 (0.4f, 0.4f, 0f);
			scaleDirection = 1;
		} else {
			return;
		}
		var nowScaleY = this.gameObject.transform.localScale.y;

	}
	*/

}
