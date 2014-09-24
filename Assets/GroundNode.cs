using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundNode {

	public List<GroundNode> neighbors;//隣接ノード
	public int xIndex;
	public int yIndex;
	public Vector2 index;

	public GameObject ground = null;

	public int minimumCost = -1;//最小到達コスト。-1は未探索
	public GroundNode nextNode = null;//最小到達コストノード

	public GroundNode(int xi, int yi, GameObject g){
		xIndex = xi;
		yIndex = yi;
		index = new Vector2(xi, yi);
		ground = g;
		neighbors = new List<GroundNode>();
	}

	public void SetMinimumCost(int cost, GroundNode neighborNode = null){
		minimumCost = cost;
		nextNode = neighborNode;
		//Debug.Log ("SetMinimumCost : " + xIndex + "," + yIndex + " / nextNode : " + (nextNode != null ? nextNode.xIndex + "," + nextNode.yIndex : ""));
	}

	public void Reset(){
		minimumCost = -1;
		nextNode = null;
	}

	public void AddNeighbor(GroundNode node){
		neighbors.Add (node);
	}
}
