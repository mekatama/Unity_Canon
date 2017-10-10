using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {
	public Canon canonScript;		//
	public GameObject canon;		//
	public Text score;				//
//	public ParticleSystem particle;	//パーティクル
	Rigidbody rb;					//弾のリジッドボディ
	int score_point = 0;			//

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
	}

	//敵の破壊
	void DestroyEnemy(Collision collision){
//		particle.Stop();
//		particle.transform.position = collision.transform.position;
//		particle.Play();
		GameObject.Destroy(collision.gameObject);
		ResetBullet();
		score_point += 100;
		score.text = "" + score_point;
		canonScript.ChangeTime();
	}

	//弾のリセット
	public void ResetBullet(){
		rb.useGravity = false;
		Vector3 v = canon.transform.position;
		v.y = 5;
		transform.position = v;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	//弾と敵の接触判定
	void OnCollisionEnter(Collision collision){
		//
		if(canonScript.IsFinished()){
			return;
		}
		//
		if(collision.gameObject.tag == "Enemy"){
			DestroyEnemy(collision);
		}
	}
}
