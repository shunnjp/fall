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
		if(Input.GetMouseButtonDown(0)){
			if(Input.mousePosition.x < Screen.width / 2){
				iTween.RotateAdd (cameraRig, iTween.Hash ("y", -90, "islocal", true, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
			}else{
				iTween.RotateAdd (cameraRig, iTween.Hash ("y", 90, "islocal", true, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
			}
		}
	}

	void Init(){
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
			GameObject g;
			int num;
			foreach(string i in values){
				num = int.Parse(i);
				if(num > 0){

					//GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.Euler (new Vector3(0.0f, 180.0f, 0.0f)));
					g = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.identity);
					g.SendMessage ("SetLife", num);
					yield return new WaitForSeconds(0.02f);
					switch(num){
					case 5:
						playerPos = new Vector3(5 + -1 * col, 6, -5 + 1 * row);
						break;
					case 6:
						GameObject.Instantiate (jewelPrefab, new Vector3(5 + -1 * col, 1.5f, -5 + 1 * row), Quaternion.identity);
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
