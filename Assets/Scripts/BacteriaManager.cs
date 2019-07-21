using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public class BacteriaManager : MonoBehaviour {

	int growthCount = 0;
	public static bool isGrown;
	bool localIsGrown;
	int hp = 1;
	GameManager gameManager;

	DateTime nowTime;
	DateTime listStartTime;
	float listMoveTime;
	float listMoveSpeedX;
	float listMoveSpeedY;
	float maxMoveSpeed = 30.0f;

	// Use this for initialization
	void Start () {

		this.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		isGrown = false;
		localIsGrown = false;
	}
	
	// Update is called once per frame
	void Update () {

		nowTime = DateTime.UtcNow;

		// 成長判定
		if (Input.GetMouseButtonDown(0) && localIsGrown != true) {

			if (growthCount > 9) {
				print ("true");
				isGrown = true;
				localIsGrown = true;
				return;
			}
			growthCount ++;
			this.transform.localScale = new Vector3 (growthCount * 0.1f, growthCount * 0.1f, 1);
		}

		// 成長済みなら動く
		if (localIsGrown == true) {
			MoveReadyPlayer();
			MovePlayer();
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		// 敵との接触時
		if (collision.gameObject.tag == "Enemy") {
				hp --;
			if (hp <= 0) {
				print ("やられた");
				gameManager.nowBacteriaCount --;
				Destroy (this.gameObject, 2.0f);
			}
		}
	}

	void MoveReadyPlayer() {
			
		float nearestTime = 10000.0f;
		// 敵
		for (int j = 0; j < EnemyBacteriaManager.listEnemy.Count; j++) {

	
			var disX = EnemyBacteriaManager.listEnemy[j].transform.position.x - transform.position.x;
			var disY = EnemyBacteriaManager.listEnemy[j].transform.position.y - transform.position.y;
	
			var disXTime = Math.Abs(disX) / maxMoveSpeed;
			var disYTime = Math.Abs(disY) / maxMoveSpeed;
			
			// X,Y遠い方で敵までの距離を算出
			if (disXTime > disYTime) {
				listMoveTime = disXTime;
			} else { 
				listMoveTime = disYTime;
			}

			// もし今までの敵の中で一番時間が短ったら目標座標に代入
			if (nearestTime > listMoveTime) {
				nearestTime = listMoveTime;
				listMoveSpeedX = disX / listMoveTime;
				listMoveSpeedY = disY / listMoveTime;
				listStartTime = nowTime;
			}
			listMoveTime = nearestTime;

		}
	}

	void MovePlayer () {
		float translationX = 0;
		float translationY = 0;

		if (listStartTime + TimeSpan.FromSeconds(listMoveTime) > nowTime) {
			// localPositionを使うとmoveSpeedに /130をしないといけない、なんで？
			translationX = Time.deltaTime * listMoveSpeedX; // / 130;
			translationY = Time.deltaTime * listMoveSpeedY; // / 130;
			this.gameObject.transform.Translate(translationX, translationY, 0f);	
		} 
	}
}
