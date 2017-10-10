using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
	public GameObject bullet;		//弾オブジェクト
//	public ParticleSystem particle;	//パーティクル
	public Canvas title_canvas, game_canvas, finish_canvas;	//GUI関係
	bool end_flag = true;			//終了フラグ
	float time_d = 10f;				//敵出現の間隔
	System.DateTime last_dt;		//最後に敵出現した時間
	Rigidbody bullet_rd;			//弾のリジッドボディ

	void Start () {
		last_dt = System.DateTime.UtcNow;
		bullet_rd = bullet.GetComponent<Rigidbody>();
	}
	
	void Update () {
		//
		if(IsFinished()){
			return;
		}
		//敵出現
		System.DateTime dt = System.DateTime.UtcNow;
		System.TimeSpan ts = dt - last_dt;
		if(ts.Seconds > time_d){
			CreateCube();
			last_dt = dt;
		}
		//攻撃
		if(Input.GetKeyDown(KeyCode.Space)){
			Fire();
		}
	}

	void FixedUpdate(){
		//
		if(IsFinished()){
			return;
		}
		//
		float x = Input.GetAxis("Vertical");
		float y = Input.GetAxis("Horizontal");
		Vector3 v = transform.rotation.eulerAngles;
		v.z -= x;
		v.y += y;
		if(v.z < 0){
			v.z = 0;
		}
		transform.rotation = Quaternion.Euler(v);
	}

	//砲台に敵が接触
	void OnTriggerEnter(Collider collider){
		//
		if(IsFinished()){
			return;
		}
		//
		if(collider.tag == "Enemy"){
//			particle.transform.position = collider.transform.position;
//			particle.Play();
			GameOver();
		}
	}

	//ゲームの初期化
	void Initial(){
		end_flag = false;
	}

	//敵出現の間隔を短くする
	public void ChangeTime(){
		time_d -= 0.1f;
		if(time_d < 1f){
			time_d = 1f;
		}
	}

	//終了チェック
	public bool IsFinished(){
		Debug.Log(end_flag);
		return end_flag;
	}

	//スタートボタン処理
	public void OnStartBtnClick(){
		title_canvas.enabled = false;
		game_canvas.enabled = true;
		Initial();
	}

	//ゲーム終了時
	public void GameOver(){
		end_flag = true;
		finish_canvas.enabled = true;
	}

	//敵生成
	void CreateCube(){
		float rr = Random.value * 50 + 50;
		float ra = Random.value * Mathf.PI * 2;
		float rx = Mathf.Sin(ra) * rr + 250;
		float rz = Mathf.Cos(ra) * rr + 250;
		float ry = Random.value * 90 + 10;

		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.tag = "Enemy";
		cube.transform.position = new Vector3(rx, ry, rz);
		cube.transform.localScale = new Vector3(10f, 10f, 10f);

		Rigidbody rb = cube.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = false;
		Renderer rd = cube.GetComponent<Renderer>();
		rd.material.color = Color.red;

		MoveCube(rb);
	}

	//敵を動かす
	void MoveCube(Rigidbody rd){
		float tx = Random.value * 200f - 100f;
		float ty = Random.value * 600f - 300f;
		float tz = Random.value * 1000f - 500f;
		rd.AddTorque(new Vector3(tx, ty, tz));

		Vector3 p1 = transform.position;
		Vector3 p2 = rd.transform.position;
		Vector3 p3 = (p1 - p2);
		rd.AddForce(p3);
	}

	//攻撃
	void Fire(){
		bullet.transform.position = transform.position;
		bullet_rd.velocity = Vector3.zero;
		bullet_rd.angularVelocity = Vector3.zero;
		Vector3 v = transform.rotation.eulerAngles;
		float rd = (v.y + 90) * Mathf.Deg2Rad;
		float rd2 = (v.z + 10) * Mathf.Deg2Rad;
		float dx = Mathf.Sin(rd);
		float dz = Mathf.Cos(rd);
		float dy = Mathf.Sin(rd2);
		Vector3 nv = new Vector3(dx, dy, dz);
		nv *= 1000f;
		bullet_rd.useGravity = true;
		bullet_rd.AddForce(nv);
	}
}
