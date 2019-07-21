using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GameManager : MonoBehaviour {

	public int nowBacteriaCount = 0;
	int maxBacteria = 2;
	GameObject bacteriaPf;
	GameObject gameBack;
	GameObject enemyPf;
	int maxEnemy = 3;

	// Use this for initialization
	void Start () {
		enemyPf = Resources.Load<GameObject> ("Prefabs/EnemyBacteria");
		bacteriaPf = Resources.Load<GameObject> ("Prefabs/Bacteria");
		gameBack = GameObject.Find ("GameBack");
		CreateBacteria();
		for (int i = 0; i < maxEnemy; i++) {
			CreateEnemy();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			//print ("PlayerCount = " + BacteriaManager.listPlayer.Count);
		}
		if (BacteriaManager.isGrown == true && nowBacteriaCount < maxBacteria ) {
			CreateBacteria();
		}
	}

	void CreateBacteria () {
		nowBacteriaCount ++;
		var pf = Instantiate<GameObject> (bacteriaPf);
		pf.transform.SetParent (gameBack.transform, false);
	}

	void CreateEnemy () {
		var pf = Instantiate<GameObject> (enemyPf);
		pf.transform.SetParent (gameBack.transform, false);
	}
}
