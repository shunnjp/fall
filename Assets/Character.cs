using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
		Vector3 velocity;
		float speed = 8.0f;
		float gravity = 20.0f;
		CharacterController controller;
		Animator animator;
		bool isGrounded = true;
		// Use this for initialization
		void Start () {
			controller = GetComponent<CharacterController> ();
			animator = GetComponent<Animator>();
		}
	
		// Update is called once per frame
		void Update () {
				if (controller.isGrounded) {
						//Debug.Log ("isGrounded == true");

						if(!isGrounded){
								isGrounded = true;
								animator.SetTrigger ("landing");
								//Debug.Log ("着地");
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

		}
}
