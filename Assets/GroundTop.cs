using UnityEngine;
using System.Collections;

public class GroundTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetTextureOffset(Vector2 offset){
		//Debug.Log ("SetTextureOffset");
		gameObject.renderer.material.SetTextureOffset("_MainTex", offset);
	}

	void OnMouseDown(){
		Debug.Log ("GroundTop MouseDown");
		transform.parent.gameObject.SendMessage ("MoveTo");
	}
}
