using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class TestMoveManager : MonoBehaviour {

	int targetEnemy = 0;
	List <GameObject> enemyList = new List<GameObject>();
	GameObject enemyPf;

	float direction = 1;
	float direction2 = -1;
	float direction3 = -1;

	float refleshTime = 0;

	GameObject kinPf;
	GameObject groundImage;
	GameObject groundImage2;
	Vector3 clickPosition;
	Vector3 worldClickPosition;
	float MAX_MOVE_SPEED = 2f;
	DateTime nowTime;

	List<GameObject> listClone = new List<GameObject>();
	List<DateTime> listStartTime = new List<DateTime>();
	List<float> listMoveTime = new List<float>();
	List<float> listMoveSpeedX = new List<float>();
	List<float> listMoveSpeedY = new List<float>();
	List<bool> listIsAttack = new List<bool>();
	List<float> listAttackDelayTime = new List<float>();
	List<GameObject> listTargetEnemy = new List<GameObject>();

	GameObject bulletPf;

	// Use this for initialization
	void Start () {

		groundImage = GameObject.Find ("GroundImage");
		groundImage2 = GameObject.Find ("GroundImage2");
		//groundImageButton = groundImage.GetComponent<Button> ();
		kinPf = Resources.Load<GameObject> ("Prefabs/TestPlayerPf");

		enemyPf = Resources.Load<GameObject> ("Prefabs/TestEnemyPf");

		bulletPf = Resources.Load<GameObject> ("Prefabs/TestBulletPf");
		CreateEnemy(200, 200);
		CreateEnemy(-200, -200);
		CreateEnemy(0, 0);

		for (int i = 0; i < 10; i++) {
			//CreateKin();
		}
		MoveReadyClone();

	}
	
	// Update is called once per frame
	void Update () {

		nowTime = DateTime.UtcNow;

		// マウス入力で左クリックを押している間
		// GetMouseButtonDown(0)は押した時のみ、こっちの方が軽い気がする
		if (Input.GetMouseButtonDown(0)) {
			for (int i = 0; i < 1; i++) {
				CreateKin();
			}
		}	

		refleshTime += Time.deltaTime;

		listClone.Remove(null);
		enemyList.Remove(null);
		if (listClone.Count != 0) {
			if (refleshTime > 0.35f) {
				refleshTime = 0;
				MoveReadyClone();
			}
			MoveClone();
		} 
		ReadyAttackEnemy();
	}

	void CreateEnemy (int x, int y) {
		var enemy = Instantiate<GameObject> (enemyPf);
		enemy.transform.SetParent (groundImage2.transform, false);
		enemy.transform.localPosition = new Vector3 (x, y, 0f);
		enemyList.Add(enemy);
	}

	void CreateKin () {
		var kin = (GameObject)Instantiate (kinPf);
		kin.transform.SetParent(groundImage.transform, false);
		kin.transform.localPosition = new Vector3 (UnityEngine.Random.Range(-400, 400),
														UnityEngine.Random.Range(-600, 600),
														0f);

		listClone.Add(kin);
		listMoveTime.Add(0);
		listStartTime.Add(nowTime);
		listMoveSpeedX.Add(0);
		listMoveSpeedY.Add(0);
		listIsAttack.Add(false);
		listAttackDelayTime.Add(0);
		listTargetEnemy.Add(null);
	}

	void ReadyAttackEnemy () {
		// 攻撃
		for (int i = 0; i < listClone.Count; i++) {
			if (listIsAttack[i] == true && listTargetEnemy[i] != null) {
				var pf = Instantiate<GameObject> (bulletPf);
				pf.transform.SetParent (groundImage.transform, false);
				var manager =  pf.GetComponent<TestBulletManager> ();
				manager.AttackEnemy(listTargetEnemy[i].transform.position.x,
										listTargetEnemy[i].transform.position.y,
										listClone[i].transform.position.x,
										listClone[i].transform.position.y);
				listAttackDelayTime[i] = 1.0f;
				listIsAttack[i] = false;
			}
			listAttackDelayTime[i] -= Time.deltaTime;
		}
	}


	void MoveReadyClone() {
		// 仲間
		for (int i = 0; i <= listClone.Count - 1; i++) {
			
			float nearestTime = 10000.0f;
			// 敵
			for (int j = 0; j <= enemyList.Count - 1; j++) {

				if (listClone[i] != null && enemyList != null) {
					var disX = enemyList[j].transform.position.x - listClone[i].transform.position.x;
					var disY = enemyList[j].transform.position.y - listClone[i].transform.position.y;

					// 攻撃可能範囲
					if (-1.2f < disX && disX < 1.2f) {
						disX += UnityEngine.Random.Range (-1.2f, 1.2f);
						if (listAttackDelayTime[i] < 0f) {
							print ("i");
							listIsAttack[i] = true;
						}
					}
					if (-1.2 < disY && disY < 1.2f) {
						disY += UnityEngine.Random.Range (-1.2f, 1.2f);
						if (listAttackDelayTime[i] < 0f) {
							listIsAttack[i] = true;
						}
					}
			

					var disXTime = Math.Abs(disX) / MAX_MOVE_SPEED;
					var disYTime = Math.Abs(disY) / MAX_MOVE_SPEED;
					nowTime = DateTime.UtcNow;
					
					// X,Y遠い方で敵までの距離を算出
					if (disXTime > disYTime) {
						listMoveTime[i] = disXTime;
					} else { 
						listMoveTime[i] = disYTime;
					}


					// もし今までの敵の中で一番時間が短ったら目標座標に代入
					if (nearestTime > listMoveTime[i]) {
						nearestTime = listMoveTime[i];
						listMoveSpeedX[i] = disX / listMoveTime[i];
						listMoveSpeedY[i] = disY / listMoveTime[i];
						listStartTime[i] = nowTime;
						listTargetEnemy[i] = enemyList[j];
					}
					listMoveTime[i] = nearestTime;
				}
			}
		}
	}

	void MoveClone () {
		float translationX = 0;
		float translationY = 0;
		for (int i = 0; i <= listClone.Count - 1; i++) {

			if (listClone[i] != null) {
				if (listStartTime[i] + TimeSpan.FromSeconds(listMoveTime[i]) > nowTime) {
					// localPositionを使うとmoveSpeedに /130をしないといけない、なんで？
					translationX = Time.deltaTime * listMoveSpeedX[i]; // / 130;
					translationY = Time.deltaTime * listMoveSpeedY[i]; // / 130;
					listClone[i].transform.Translate(translationX, translationY, 0f);	
				} 
			}
		}
	}
}
