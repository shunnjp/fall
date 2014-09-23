using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameController : MonoBehaviour {

	public GameObject cameraRig;
	public GameObject playerPrefab;
	public GameObject jewelPrefab;

	public GroundNode[,] groundNodeList;

	private PathSearcher pathSearcher;

	// Use this for initialization
	void Start () {
		//iTween.RotateTo(cameraRig, iTween.Hash ("x", 0, "islocal", true, "time", 1.0f, "delay", 1.0f, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "Init", "oncompletetarget", gameObject));
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		//camera rotation
		if(Input.GetMouseButtonDown(0)){
			if(Input.mousePosition.x < Screen.width / 2){
				iTween.RotateAdd (cameraRig, iTween.Hash ("y", -90, "islocal", true, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
			}else{
				iTween.RotateAdd (cameraRig, iTween.Hash ("y", 90, "islocal", true, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
			}
		}
		*/
	}

	void Init(){
		//GenerateGround();
		StartCoroutine("GenerateGround");
	}

	IEnumerator GenerateGround(){
		TextAsset mapText = Resources.Load ("map") as TextAsset;
		StringReader reader = new StringReader(mapText.text);

		Vector2 playerPos = new Vector3(0, 0);

		//list for search path
		int rowCount = 0;
		string[] lines = mapText.text.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
		rowCount = lines.Length;
		int colCount = 0;
		string[] firstLine = lines[0].Split (',');
		colCount = firstLine.Length;
		pathSearcher = new PathSearcher (colCount, rowCount);

		int row = 0;
		while (reader.Peek() > -1) {
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			int col = 0;
			GameObject g;
			int num;

			/*
			//for search path
			if(groundNodeList == null){
				groundNodeList = new GroundNode[lineCount, values.Length];
			}
			*/

			foreach(string i in values){
				num = int.Parse(i);
				if(num > 0){

					//GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, -1, -5 + 1 * row), Quaternion.Euler (new Vector3(0.0f, 180.0f, 0.0f)));
					g = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/Ground"), new Vector3(5 + -1 * col, 0, -5 + 1 * row), Quaternion.identity);
					g.SendMessage ("SetPos", new Vector2(col, row));
					g.SendMessage ("SetLife", num);
					iTween.ColorFrom (g, iTween.Hash ("a", 0, "time", 0.5f, "easetype", iTween.EaseType.easeInOutCubic));
					iTween.MoveFrom (g, iTween.Hash ("y", g.transform.position.y - 1, "time", 0.5f, "easetype", iTween.EaseType.easeInOutCubic));					yield return new WaitForSeconds(0.02f);

					switch(num){
					case 5:
						//playerPos = new Vector3(5 + -1 * col, 6, -5 + 1 * row);
						playerPos = new Vector2(col, row);
						break;
					case 6:
						GameObject.Instantiate (jewelPrefab, new Vector3(5 + -1 * col, 1.5f, -5 + 1 * row), Quaternion.identity);
						break;
					default:
						break;
					}

					//groundNodeList[col, row] = new GroundNode(col, row, g);
					pathSearcher.AddNode(col, row, g);
				}
				col++;
			}//foreach
			row++;

		}//while
		
		//GameObject.Instantiate (playerPrefab, playerPos, Quaternion.identity);
		GameObject player = (GameObject)GameObject.Instantiate (playerPrefab, new Vector3(5 + -1 * playerPos.x, 6, -5 + 1 * playerPos.y), Quaternion.identity);
		player.SendMessage ("SetCurrentPosition", playerPos);
		
		//set neighbor int ground nodes for search path
		pathSearcher.init ();
		/*
		int j,k;
		for(j = 0; j < groundNodeList.GetLength (0); j++){
			for(k = 0; k < groundNodeList.GetLength (1); k++){
				//Debug.Log ("(" + j + "," + k + ") " + groundNodeList[j,k]);
				if(groundNodeList[j,k] == null) continue;
				GroundNode currentNode = groundNodeList[j,k];
				if(j > 0 && groundNodeList[j-1,k] != null) currentNode.AddNeighbor(groundNodeList[j-1,k]);
				if(j < groundNodeList.GetLength (0)-1 && groundNodeList[j+1,k] != null) currentNode.AddNeighbor(groundNodeList[j+1,k]);
				if(k > 0 && groundNodeList[j,k-1] != null) currentNode.AddNeighbor(groundNodeList[j,k-1]);
				if(k < groundNodeList.GetLength (1)-1 && groundNodeList[j,k+1] != null) currentNode.AddNeighbor(groundNodeList[j,k+1]);
			}
		}//for
		*/
	}

	void PlayerMove(GameObject g){
		Ground ground = g.GetComponent<Ground> ();
		Debug.Log (ground);
		Vector2 groundPos = ground.position;

		Player player = GameObject.FindWithTag ("Player").GetComponent<Player>();
		Vector2 playerPos = player.currentPosition;
		List<GroundNode> list = pathSearcher.StartSearch (playerPos, groundPos);
		if (list.Count == 0) {
			Debug.Log ("can't reach there");
		} else {
			Debug.Log ("===== PathSearcher Result =====");
			foreach(GroundNode i in list){
				Debug.Log (i.xIndex + "," + i.yIndex);
			}
			Debug.Log ("===== /PathSearcher Result =====");
		}
	}


}
