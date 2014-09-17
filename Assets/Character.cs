using UnityEngine;
using System.Collections;
using System.IO;

public class Character : MonoBehaviour {
	
	Vector3 velocity;
	float speed = 5.0f;
	//float gravity = 20.0f;
	Animator animator;
	bool isGrounded = true;
	GameObject ground;

	// Use this for initialization
	void Start () {
		//gameObject.rigidbody.freezeRotation = false;
		
		animator = GetComponent<Animator>();

		TextAsset mapText = Resources.Load ("map") as TextAsset;
		StringReader reader = new StringReader(mapText.text);


		int row = 0;
		while (reader.Peek() > -1) {
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			int col = 0;
			foreach(string i in values){
				if(i == "1"){
					GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.identity);
				}
				col++;
			}
			row++;
		}

	}
	
	// Update is called once per frame
	void Update () {



		if(isGrounded){
			velocity = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
			velocity *= speed;

			animator.SetFloat ("velocity", velocity.magnitude);
			gameObject.rigidbody.velocity = new Vector3(velocity.x, gameObject.rigidbody.velocity.y, velocity.z);

			transform.LookAt (transform.position + velocity);
		}

		//Debug.Log (gameObject.rigidbody.velocity.y);
		if(isGrounded && gameObject.rigidbody.velocity.y < -3){
			animator.SetTrigger ("fall");
			isGrounded = false;
			//Debug.Log ("落下");
			gameObject.rigidbody.velocity = new Vector3(0.0f, gameObject.rigidbody.velocity.y, 0.0f);
		}
		
		if(!isGrounded){
			animator.SetFloat ("velocityY", gameObject.rigidbody.velocity.y);
		}

		//Debug.Log (velocity.magnitude);
		if(gameObject.transform.position.y < -10){
			//Debug.Log ("A' : " + (gameObject.transform.position.x*-1) + "," + gameObject.transform.position.y + "," + (gameObject.transform.position.z*-1));			//gameObject.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			gameObject.transform.Translate(new Vector3(gameObject.transform.position.x * -1.0f, gameObject.transform.position.y * -1.0f + 6.0f, gameObject.transform.position.z * -1.0f), Space.World);
			//gameObject.rigidbody.MovePosition (new Vector3(0.0f, 6.0f, 0.0f));
			//velocity.y = 0;
			gameObject.rigidbody.velocity = Vector3.zero;
		}
	}

	void OnCollisionEnter(Collision c){
		if(isGrounded == false && c.gameObject.tag == "Ground"){
			animator.SetTrigger ("landing");
			isGrounded = true;
			gameObject.rigidbody.velocity = Vector3.zero;
			Debug.Log ("着地");
		}
	}

	/*
	void OnControllerColliderHit(ControllerColliderHit hit){
		//Debug.Log ("OnControllerColliderHit");
	}
	*/
}
