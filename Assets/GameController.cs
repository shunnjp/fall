using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameController : MonoBehaviour {

	public GameObject cameraRig;
	public GameObject playerPrefab;
	public GameObject jewelPrefab;

	public GameObject player;

	public GroundNode[,] groundNodeList;

	private PathSearcher pathSearcher;

	private List<GroundNode> moveList;
	
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

		moveList = new List<GroundNode>();

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
		
		player = (GameObject)GameObject.Instantiate (playerPrefab, new Vector3(5 + -1 * playerPos.x, 6, -5 + 1 * playerPos.y), Quaternion.identity);
		player.SendMessage ("SetInitCoordinates", playerPos);
		player.SendMessage ("SetCurrentCoordinates", playerPos);
		
		//set neighbor int ground nodes for search path
		pathSearcher.init ();
	}

	void PlayerMove(GameObject g){
		Ground ground = g.GetComponent<Ground> ();
		Vector2 groundPos = ground.position;

		Player playerComp = GameObject.FindWithTag ("Player").GetComponent<Player>();
		Vector2 playerPos = playerComp.currentCoordinates;
		//Debug.Log ("playerPos : " + playerPos.x + "," + playerPos.y);

		//探索開始
		bool result = pathSearcher.StartSearch (playerPos, groundPos);

		if (result == true) {
			moveList.Clear ();

			//Debug.Log ("===== PathSearcher Result =====");
			GroundNode currentNode = pathSearcher.startNode;
			moveList.Add (currentNode);
			//Debug.Log ("(" + currentNode.xIndex + "," + currentNode.yIndex + ") next(" + currentNode.nextNode + ")");
			while(currentNode.nextNode is GroundNode){
				currentNode = currentNode.nextNode;
				moveList.Add (currentNode);
				//Debug.Log ("(" + currentNode.xIndex + "," + currentNode.yIndex + ") next(" + currentNode.nextNode + ")");
			}
			//GameObject.FindWithTag ("Player").GetComponent<Animator>().SetFloat ("velocity", 3.0f);
			ExecMoveQueue();
			//Debug.Log ("===== /PathSearcher Result =====");
		}else{
			Debug.Log ("can't reach there");
		}
	}

	void ExecMoveQueue(){

		GroundNode groundNode = moveList[0];


		if(groundNode.index == player.GetComponent<Player>().currentCoordinates){
			//Debug.Log ("座標が一緒なので移動はスキップ");
			CompleteMove();
		}else{
			player.GetComponent<Animator>().SetFloat ("velocity", 3.0f);
			iTween.MoveTo(player, iTween.Hash (
				"x", groundNode.ground.transform.position.x,
				"z", groundNode.ground.transform.position.z,
				"time", 0.3f,
				"delay", 0.0f,
				"easetype", iTween.EaseType.linear,
				"oncompletetarget", gameObject,
				"oncomplete", "CompleteMove"
			));

			player.transform.LookAt (new Vector3(groundNode.ground.transform.position.x, player.transform.position.y, groundNode.ground.transform.position.z));
			
			}
	}

	void CompleteMove(){
		GroundNode previousNode = moveList[0];
		moveList.RemoveAt (0);

		player.SendMessage ("SetCurrentCoordinates", new Vector2(previousNode.xIndex, previousNode.yIndex));//playerのcurrentCoordinateに異動先の座標をセット

		//Debug.Log ("complete : " + groundNode.xIndex + "," + groundNode.yIndex);
		if(moveList.Count > 0){
			//lifeはそのマスを離れるときに減らすため、最後のひとつの場合はlifeを減らさない。つまり最後の一つでない場合は減らす。
			previousNode.ground.SendMessage ("AddLife", -1);
			//次のキューへ
			ExecMoveQueue();
		}else{
			player.GetComponent<Animator>().SetFloat ("velocity", 0.0f);
			//Debug.Log ("移動完了");
		}
	}

}
