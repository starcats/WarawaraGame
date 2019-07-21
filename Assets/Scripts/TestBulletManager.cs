using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TestBulletManager : MonoBehaviour {

	float targetX;
	float targetY;
	float myX;
	float myY;
	float moveTime;
	float moveSpeedX;
	float moveSpeedY;
	const float MAX_MOVE_SPEED = 3.0f;
	DateTime nowTime;
	DateTime startTime;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		nowTime = DateTime.UtcNow;

		ReadyMove();
		Move();

	}


	public void AttackEnemy (float targetX, float targetY , float myX, float myY) {
		this.targetX = targetX;
		this.targetY = targetY;
		this.myX = myX;
		this.myY = myY;
		transform.position = new Vector3 (myX, myY, 0f);
	}

	void ReadyMove () {
		var disX = targetX - myX;
		var disY = targetY - myY;
		var disXTime = Math.Abs(disX) / MAX_MOVE_SPEED;
		var disYTime = Math.Abs(disY) / MAX_MOVE_SPEED;

		if (disXTime > disYTime) {
			moveTime = disXTime;
		} else { 
			moveTime = disYTime;
		}
		moveSpeedX = disX / moveTime;
		moveSpeedY = disY / moveTime;

		startTime = nowTime;
	}

	void Move () {
		if (startTime + TimeSpan.FromSeconds(moveTime) > nowTime) {
			// localPositionを使うとmoveSpeedに / 130をしないといけない、なんで？
			// どっかにworldの数字を決めるやつがあった気がする
			var translationX = Time.deltaTime * moveSpeedX; // / 130;
			var translationY = Time.deltaTime * moveSpeedY; // / 130;
			transform.Translate(translationX, translationY, 0f);	
		} 
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Enemy") {
			Destroy (this.gameObject);
		}
	}
}
