using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	float r = 0;

	float previousY = 0;
	CharacterController controller = null;
	// Use this for initialization
	void Start () {
		previousY = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		previousY = gameObject.transform.position.y;
		//Debug.Log (gameObject.transform.position.y);
		float newY = Mathf.Sin (r * Mathf.PI / 180);

		if(controller){
			controller.Move (new Vector3(0.0f, previousY - newY, 0.0f));
			//Debug.Log (controller.gameObject.transform.position.y);
		}

		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, newY, gameObject.transform.position.z);
		r += 0.5f;


	}

	void ClearController(){
		controller = null;
	}

	void SetController(CharacterController c){
		controller = c;
	}
}
