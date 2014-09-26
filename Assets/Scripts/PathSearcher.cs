using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSearcher {

	public GroundNode startNode;
	public GroundNode goalNode;

	private GroundNode[,] groundNodeList;

	public PathSearcher(int countX, int countY){
		groundNodeList = new GroundNode[countX, countY];
	}

	public void AddNode(int x, int y, GameObject ground){
		groundNodeList [x, y] = new GroundNode(x, y, ground);
	}

	public void init(){
		//set neighbors
		for(int i = 0; i < groundNodeList.GetLength (0); i++){
			for(int j = 0; j < groundNodeList.GetLength (1); j++){
				//Debug.Log ("(" + j + "," + k + ") " + groundNodeList[j,k]);
				if(groundNodeList[i,j] == null) continue;
				GroundNode currentNode = groundNodeList[i,j];
				if(i > 0 && groundNodeList[i-1,j] != null) currentNode.AddNeighbor(groundNodeList[i-1,j]);
				if(i < groundNodeList.GetLength (0) - 1 && groundNodeList[i+1,j] != null) currentNode.AddNeighbor(groundNodeList[i+1,j]);
				if(j > 0 && groundNodeList[i,j-1] != null) currentNode.AddNeighbor(groundNodeList[i,j-1]);
				if(j < groundNodeList.GetLength (1) - 1 && groundNodeList[i,j+1] != null) currentNode.AddNeighbor(groundNodeList[i,j+1]);
			}
		}//for
	}

	public bool StartSearch(Vector2 startPos, Vector2 goalPos){
		Reset();

		startNode = groundNodeList[(int)startPos.x, (int)startPos.y];
		goalNode = groundNodeList[(int)goalPos.x, (int)goalPos.y];

		goalNode.SetMinimumCost(0);


		List<GroundNode> list = new List<GroundNode>();
		list.Add (goalNode);
		while(list.Count > 0){
			GroundNode tartgetNode = list[0];
			foreach(GroundNode neighbor in tartgetNode.neighbors){
				if(!neighbor.ground || (neighbor.ground && neighbor.ground.GetComponent<Ground>().life <= 0)) continue;//GroundのゲームオブジェクトがDestroyされている、またはlife0以下（まだDestroyはされていない）の場合はスキップする
				if (neighbor.minimumCost == -1
				    || tartgetNode.minimumCost + 1 < neighbor.minimumCost) {
					neighbor.SetMinimumCost (tartgetNode.minimumCost + 1, tartgetNode);
					list.Add (neighbor);
				}
			}
			list.RemoveAt (0);

			//最初にstartNodeに到達した経路が最短なので、今ループでstartNodeのminimumCostが確定したらループを抜ける
			if(startNode.minimumCost != -1) break;
		}

		//execute seach
		//Search (goalNode);

		//startNodeのminimumCostが-1なら到達できていないのでfalseを返す。それ以外はtrueを返す。
		return (startNode.minimumCost != -1);

		/*
		//return result
		List<GroundNode> list = new List<GroundNode> ();

		if (startNode.minimumCost != -1) {
			GroundNode currentNode = startNode;
			list.Add (currentNode);
			while(currentNode.nextNode is GroundNode) {
				list.Add (currentNode.nextNode);
				currentNode = currentNode.nextNode;
			}
		}
		
		return list;
		*/

	}

	void Search(GroundNode tartgetNode){
		//targetNodeのneighborsを探索
		//Debug.Log ("Search!");
		List<GroundNode> neighbors = tartgetNode.neighbors;
		for(int i = 0; i<neighbors.Count; i++){
			GroundNode neighbor = neighbors[i];
			if(!neighbor.ground) continue;//GroundのゲームオブジェクトがDestroyされている場合はスキップする
			if (neighbor.minimumCost == -1
			    || tartgetNode.minimumCost + 1 < neighbor.minimumCost) {
				neighbor.SetMinimumCost (tartgetNode.minimumCost + 1, tartgetNode);
				Search (neighbor);
			}
		}
	}

	public void Reset(){
		startNode = null;
		goalNode = null;
		//reset all nodes
		foreach(GroundNode i in groundNodeList){
			if(i != null){
				i.Reset();
			}
		}
		//set -1 into minimumCost
	}
}
