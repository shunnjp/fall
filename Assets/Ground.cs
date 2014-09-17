using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	float r = 0;
	float radius = 0.12f;
	float speed = 0.5f;

	// Use this for initialization
	void Start () {
		r = Random.Range (0, 360);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (gameObject.transform.position.y);
		float newY = Mathf.Sin (r * Mathf.PI / 180) * radius;

		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, newY, gameObject.transform.position.z);
		r += speed;

	}
}
