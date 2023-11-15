﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGrid : MonoBehaviour
{

	public Tilemap collidableMap;
	public Vector2 gridWorldSize;
	public float resolution;
	public bool allowDiagonals;
	public bool showGrid;
	Node[,] nodes;
	private int gridX, gridY;
	private float offset;

	void Start() {
		//set gridX, gridY, offset
		gridX = Mathf.RoundToInt(gridWorldSize.x / resolution);
		gridY = Mathf.RoundToInt(gridWorldSize.y / resolution);

		offset = resolution / 2;

		//start building the grid
		BuildGrid();
	}

	void BuildGrid() {
		//create the map array
		nodes = new Node[gridX, gridY];
		//Bottom left corner of bottom left tile
		Vector3 startPos = transform.position - new Vector3(gridWorldSize.x / 2, gridWorldSize.y / 2, 0);
		//iterate across the map space
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				//check the middle of each node
				Vector3 checkPos = startPos + new Vector3(x * resolution + offset, y * resolution + offset, 0);
				bool isSolid = false;

				//if there is a collidable tile there, then mark the node as solid
				if (collidableMap.HasTile(collidableMap.WorldToCell(checkPos))) {
					isSolid = true;
				}

				//update the node map
				nodes[x, y] = new Node(isSolid, x, y);
			}
		}
	}

    internal List<Node> GetNeighbourNodes(Node node) {

		List<Node> neighborList = new List<Node>();

		int x = node.positionX;
		int y = node.positionY;

		//edge checks
		bool left = x != 0;
		bool right = x != gridX - 1;
		bool down = y != 0;
		bool up = y != gridY - 1;

		//Horizontal and vertical neighbors
		if (left) {
			neighborList.Add(nodes[x - 1, y]);
		}
		if (right) {
			neighborList.Add(nodes[x + 1, y]);
		}
		if (down) {
			neighborList.Add(nodes[x, y - 1]);
		}
		if (up) {
			neighborList.Add(nodes[x, y + 1]);
		}

		//Diagonal neighbors
		if (allowDiagonals) {
			if (left) {
				if (down) {
					if (!nodes[x - 1, y].isCollider && !nodes[x, y - 1].isCollider) {
						neighborList.Add(nodes[x - 1, y - 1]);
					}
				}
				if (up) {
					if (!nodes[x - 1, y].isCollider && !nodes[x, y + 1].isCollider) {
						neighborList.Add(nodes[x - 1, y + 1]);
					}
				}
			}
			if (right) {
				if (down) {
					if (!nodes[x + 1, y].isCollider && !nodes[x, y - 1].isCollider) {
						neighborList.Add(nodes[x + 1, y - 1]);
					}
				}
				if (up) {
					if (!nodes[x + 1, y].isCollider && !nodes[x, y + 1].isCollider) {
						neighborList.Add(nodes[x + 1, y + 1]);
					}
				}
			}
		}


		return neighborList;

    }

    public Node NodeFromPos(Vector3 worldPos) {
		//Center on the world center
		Vector3 centered = worldPos - transform.position;

		//Get pos at % of total grid width, limit between 0-1, multiply by # of nodes - 1 for position in grid
		int x = Mathf.RoundToInt(Mathf.Clamp01(centered.x / gridWorldSize.x + 0.5f) * (gridX - 1));
		int y = Mathf.RoundToInt(Mathf.Clamp01(centered.y / gridWorldSize.y + 0.5f) * (gridY - 1));

		return nodes[x, y];
	}

	public Vector3 PosFromNode(Node node) {
		//Center on grid position
		Vector3 pos = transform.position;

		pos.x = pos.x - (gridWorldSize.x / 2) + ((node.positionX + 0.5f) * resolution);
		pos.y = pos.y - (gridWorldSize.y / 2) + ((node.positionY + 0.5f) * resolution);

		return pos;
	}

	private void OnDrawGizmos() {
		//if debugging
		if (showGrid && nodes != null) {
			//color solid nodes yellow, moveable nodes white
			foreach (Node n in nodes) {
				if (n.isCollider) {
					Gizmos.color = Color.red;
				} else {
					Gizmos.color = Color.clear;
				}
				Gizmos.DrawWireCube(PosFromNode(n), new Vector3(1, 1, 1) * resolution);
			}
		}
	}
}
