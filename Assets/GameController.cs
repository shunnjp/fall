using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject cameraRig;

	// Use this for initialization
	void Start () {
		iTween.RotateTo(cameraRig, iTween.Hash ("x", 0, "islocal", true, "time", 1.0f, "delay", 2.0f, "easetype", iTween.EaseType.easeInBack));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
