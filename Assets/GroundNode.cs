using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundNode {

	public List<GroundNode> neighbors;//隣接ノード
	public int minimumCost = -1;//最小到達コスト。-1は未探索
	public GroundNode nextNode;//最小到達コストノード
	public int xIndex;
	public int yIndex;

	public GameObject ground;

	public GroundNode(int xi, int yi, GameObject g){
		xIndex = xi;
		yIndex = yi;
		ground = g;
		neighbors = new List<GroundNode>();
	}

	public void SetMinimumCost(int cost, GroundNode neighborNode = null){
		minimumCost = cost;
		nextNode = neighborNode;
	}

	public void Reset(){
		minimumCost = -1;
		nextNode = null;
	}

	public void AddNeighbor(GroundNode node){
		neighbors.Add (node);
	}
}
