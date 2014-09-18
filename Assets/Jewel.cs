using UnityEngine;
using System.Collections;

public class Jewel : MonoBehaviour {

	float initY = 0;
	float r = 0;
	float radius = 0.5f;
	float speed = 0.2f;

	// Use this for initialization
	void Start () {
		initY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		float newY = Mathf.Sin (r * Mathf.PI / 180) * radius + initY;
		
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, newY, gameObject.transform.position.z);
		r += speed;
	}
}
