using UnityEngine;
using System.Collections;
using System.IO;

public class GameController : MonoBehaviour {

	public GameObject cameraRig;
	public GameObject playerPrefab;
	public GameObject jewelPrefab;

	// Use this for initialization
	void Start () {
		//iTween.RotateTo(cameraRig, iTween.Hash ("x", 0, "islocal", true, "time", 1.0f, "delay", 1.0f, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "Init", "oncompletetarget", gameObject));
		Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Init(){
		Debug.Log ("Init");
		//GenerateGround();
		StartCoroutine("GenerateGround");
	}

	IEnumerator GenerateGround(){
		TextAsset mapText = Resources.Load ("map") as TextAsset;
		StringReader reader = new StringReader(mapText.text);

		Vector3 playerPos = new Vector3(0, 6, 0);

		int row = 0;
		while (reader.Peek() > -1) {
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			int col = 0;
			foreach(string i in values){
				if(int.Parse(i) > 0){
					//GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.Euler (new Vector3(0.0f, 180.0f, 0.0f)));
					GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.identity);
					yield return new WaitForSeconds(0.02f);
					switch(int.Parse(i)){
					case 2:
						playerPos = new Vector3(5 + -1 * col, 6, -5 + 1 * row);
						break;
					case 3:
						GameObject.Instantiate (jewelPrefab, new Vector3(5 + -1 * col, 1, -5 + 1 * row), Quaternion.identity);
						break;
					default:
						break;
					}
				}
				col++;
			}
			row++;
		}

		GameObject.Instantiate (playerPrefab, playerPos, Quaternion.identity);
	}
}
