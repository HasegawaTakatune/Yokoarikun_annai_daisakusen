﻿using UnityEngine;
using System.Collections;
//using System;

public class Customers : MonoBehaviour {

	// Customers Type
	const byte Boy=0,Boy2=1,Girl=2;
	const byte OutsideTheArea=0,InArea=1;

	public byte moveStatus = 0;
	public byte type;

	public int CustomerNumber;
	public ArrayCharracter player;
	Enemy enemy;
	//public bool active=true;
	public bool Induction = true;
	bool hit = false;

	public Vector3 target = new Vector3 (0, 6, 0);
	float angle;
	float speed =0.05f;
	float range = 0.05f;
	public int direction = 0;

	Vector3[] movePosiResult = new Vector3[8];
	int Angle = 0;
	float time = 0;

	Animator animator;
	public static readonly int[] Direc = new int[] {
		// Boy Animator
		Animator.StringToHash ("CustomersSprite@Down"),
		Animator.StringToHash ("CustomersSprite@Up"),
		Animator.StringToHash ("CustomersSprite@Right"),
		Animator.StringToHash ("CustomersSprite@Left"),
		// Boy2 Animator
		Animator.StringToHash ("CustomersBoySprite@Down"),
		Animator.StringToHash ("CustomersBoySprite@Up"),
		Animator.StringToHash ("CustomersBoySprite@Right"),
		Animator.StringToHash ("CustomersBoySprite@Left"),
		// Girl Animator
		Animator.StringToHash ("CustomersGirlSprite@Down"),
		Animator.StringToHash ("CustomersGirlSprite@Up"),
		Animator.StringToHash ("CustomersGirlSprite@Right"),
		Animator.StringToHash ("CustomersGirlSprite@Left")
	};

	void Awake(){
		// アニメーター取得
		animator = GetComponent<Animator> ();
	}

	void Start(){
		GenderDetermination ();
		int i = 0;
		while (movePosiResult.Length != i) {
			movePosiResult [i] = new Vector3 (Mathf.Sin ((transform.localEulerAngles.y + 45 * i) * 3.14f / 180) * 0.02f, 
				Mathf.Cos ((transform.localEulerAngles.y + 45 * i) * 3.14f / 180) * 0.02f, 0);
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameStatus.stop) {
			if (Induction) {
				// Move to target
				angle = Mathf.Atan2 (target.y - transform.position.y, target.x - transform.position.x);
				if (Vector3.Distance (transform.position, target) >= range) {
					transform.position += new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * speed;
				}
			} else {
				switch(moveStatus){

				case InArea:
				// ランダム移動
					time -= Time.deltaTime;
					if (time <= 1) {
					SetAnimator (0);
					transform.position += movePosiResult [Angle];
					} 
					if (time <= 0) {
						Angle = (Random.Range (0, 8));
						time = Random.Range (1, 3);
						if (3 < transform.position.x || transform.position.x < -3)
							moveStatus = OutsideTheArea;
					}
					break;

				case OutsideTheArea:
					//エリア外行動
					if (transform.position.x >= 2)
						transform.position += movePosiResult [6];
					else if (transform.position.x <= -2)
						transform.position += movePosiResult [2];
					else
						moveStatus = InArea;
					
					SetAnimator (0);
					break;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		// Hit
		if (Induction && !hit) {
			if (other.gameObject.tag != "Customer" && other.gameObject.tag != "Player" && Induction) {
				enemy = other.gameObject.GetComponent<Enemy> ();
				player.Hit (CustomerNumber, enemy);
				hit = true;
			}
		}
	}

	// change Animetion
	public void SetAnimator(int direc){
		direction = direc;
		if (type == Boy2)direc += 4;
		else if (type == Girl)direc += 8;
		animator.Play (Direc [direc]);
	}

	public void KillMe(){
		Destroy (gameObject);
	}

	// 性別決定
	public void GenderDetermination(){
		type = (byte)Mathf.Floor (Random.Range (0, 3));
	}

	public void SetPosition(Vector3 input){
		transform.position = input;
	}

}