using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSearcher {

	private GroundNode startNode;
	private GroundNode goalNode;

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

	public List<GroundNode> StartSearch(Vector2 startPos, Vector2 goalPos){
		startNode = groundNodeList[(int)startPos.x, (int)startPos.y];
		goalNode = groundNodeList[(int)goalPos.x, (int)goalPos.y];

		goalNode.SetMinimumCost(0);

		//execute seach
		Search (goalNode);

		//return result
		List<GroundNode> list = new List<GroundNode> ();

		if (startNode.minimumCost != -1) {
			GroundNode currentNode = startNode;
			do {
				list.Add (currentNode);
				currentNode = currentNode.nextNode;
			}while(currentNode != goalNode);
		}

		return list;
	}

	void Search(GroundNode tartgetNode){
		List<GroundNode> neighbors = tartgetNode.neighbors;
		for(int i = 0; i<neighbors.Count; i++){
			GroundNode neighbor = neighbors[i];
			if (startNode.minimumCost == -1
			    || tartgetNode.minimumCost + 1 < neighbor.minimumCost) {
				neighbor.SetMinimumCost (tartgetNode.minimumCost + 1, tartgetNode);
				Search (neighbor);
			}
		}
	}

	public void Reset(){
		//reset all nodes

		//set -1 into minimumCost
	}
}
