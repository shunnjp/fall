using UnityEngine;
using System.Collections;
using System.IO;

public class Character : MonoBehaviour {
	
	Vector3 velocity;
	float speed = 8.0f;
	float gravity = 20.0f;
	CharacterController controller;
	Animator animator;
	bool isGrounded = true;
	GameObject ground;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
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

		//Debug.Log (controller.isGrounded);
		if (controller.isGrounded) {
			//Debug.Log ("isGrounded == true");

			if(!isGrounded){
				isGrounded = true;
				animator.SetTrigger ("landing");
				Debug.Log ("着地");
			}
			velocity = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
			velocity *= speed;

			transform.LookAt (transform.position + velocity);

			animator.SetFloat ("velocity", velocity.magnitude);

			//Debug.Log (velocity.magnitude);

		} else {
			//Debug.Log ("isGrounded == false");

			if(isGrounded){
				isGrounded = false;
				animator.SetTrigger ("fall");
				//Debug.Log ("落下");
			}

			velocity.x = 0;
			velocity.z = 0;

		}
		velocity.y -= gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
			
		if(gameObject.transform.position.y < -20){
			gameObject.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			velocity.y = 0;
		}

	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		//Debug.Log ("OnControllerColliderHit");
		if(ground){
			ground.SendMessage ("ClearController");
		}

		ground = hit.gameObject;
		ground.SendMessage ("SetController", controller);
	}
}
