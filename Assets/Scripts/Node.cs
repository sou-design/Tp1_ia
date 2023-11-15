using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	public bool isCollider;
	public int positionX, positionY;
	public int gCost, hCost;//hcost is the manhattan distance
	public Node parent;
	public int fCost {
		get {
			return gCost + hCost;//somme
		}
	}
	public Node(bool iscollider, int x, int y) {
		isCollider = iscollider;
		positionX = x;
		positionY = y;
	}
}