using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	public Vector2 position;
	public int life = 1;
	private int maxLife = 4;

	private float r = 0;
	private float radius = 0.15f;
	private float speed = 0.5f;

	// Use this for initialization
	void Start () {
		r = Random.Range (0, 360);
	}
	
	// Update is called once per frame
	void Update () {
		if(life > 0){
			//Debug.Log (gameObject.transform.position.y);
			float newY = Mathf.Sin (r * Mathf.PI / 180) * radius;

			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, newY, gameObject.transform.position.z);
			r += speed;
			if(r > 360){
				r -= 360;
			}else if(r < 0){
				r += 360;
			}
		}
	}

	void SetPos(Vector2 p){
		position = p;
	}

	void SetLife(int l){
		if(l > maxLife) l = maxLife;
		life = l;
		if(life <= 0){
			iTween.MoveBy(gameObject, iTween.Hash ("y", -1, "time", 1, "easeType", iTween.EaseType.easeOutCubic));
			iTween.ColorTo (gameObject, iTween.Hash ("a", 0, "time", 1, "oncomletetarget", gameObject, "oncomplete", "DestroySelf"));

		}else{
			gameObject.BroadcastMessage("SetTextureOffset", new Vector2(0.25f * (life - 1), 0.0f));
		}
	}

	void AddLife(int value){
		SetLife(life + value);
	}

	/*
	void OnCollisionExit(Collision c){
		//Debug.Log ("Ground CollisionExit");
		if(c.gameObject.tag == "Player"){
			//Debug.Log ("Ground CollisionExit with Player");
			SetLife(life - 1);
		}
	}
	*/

	void DestroySelf(){
		Destroy (gameObject);
	}

	/*
	void OnMouseDown(){
		Debug.Log ("GroundTop MouseDown");
	}
	*/

	void MoveTo(){
		GameObject.FindWithTag ("GameController").SendMessage ("PlayerMove", gameObject);
	}
}
