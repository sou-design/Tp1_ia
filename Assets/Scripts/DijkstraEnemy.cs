using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraEnemy : MonoBehaviour {

    public PathGrid grid;
    public GameObject destination;
    private List<Node> path;
    private List<Node> draw = new List<Node>();

    Vector3 End;
    bool oldAllowDiago;
    public bool drawPath;
    public float retard = 1.0f;
    float lastMovement = 0;
    
    int nextNodeIndex = -1;

    void FixedUpdate() {

        float currentTime = Time.fixedTime;
        if (lastMovement + retard >= currentTime) {
            return;
        }
        lastMovement += retard;

        Vector3 start = transform.position;
        Vector3 end = destination.transform.position;

        if (End != end || grid.allowDiagonals != oldAllowDiago) {

            path = FindPath(start, end);

            if (path != null) {
                nextNodeIndex = 0;
            } else {
                nextNodeIndex = -1;
            }

            End = end;
            oldAllowDiago = grid.allowDiagonals;
        }
        if (nextNodeIndex == -1) {
            return;
        }

        // We reached destination
        if (nextNodeIndex == path.Count) {
            Debug.Log("REACHED DESTINATION");
            //gameOver.setup();
            //gameObject.GetComponent<DijkstraEnemy>().enabled = false;
            //GameObject g = GameObject.Find("skeleton2");
            //g.gameObject.GetComponent<AStarEnemy>().enabled = false;
            return;
        }

        Node currentPositionN = grid.NodeFromPos(transform.position);

        int horizontal = path[nextNodeIndex].positionX - currentPositionN.positionX;
        int vertical = path[nextNodeIndex].positionY - currentPositionN.positionY;

        ++nextNodeIndex;

        Vector3 nextPos = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromPos(nextPos);
        //CHECK if collider
        if (nextNode.isCollider) {
            return;
        }
        transform.position = nextPos;

    }
    //trouver le chemin le plus court
    public List<Node> FindPath(Vector3 sPos, Vector3 ePos) {

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = grid.NodeFromPos(sPos);
        Node endNode = grid.NodeFromPos(ePos);

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.parent = null;
        openList.Add(startNode);

        while (openList.Count != 0) {

            Node currentNode = MinimumCost(openList);
            if (currentNode == endNode) {
                return MakePath(startNode, endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbourList = grid.GetNeighbourNodes(currentNode);

            foreach (Node neighbour in neighbourList) {
                if (closedList.Contains(neighbour) || neighbour.isCollider) {
                    continue;
                }
                int gCost = currentNode.gCost + CalculateDistance(neighbour, currentNode);

                if (!openList.Contains(neighbour)) {
                    neighbour.gCost = gCost;
                    neighbour.parent = currentNode;
                    openList.Add(neighbour);

                } else {
                    if (gCost < neighbour.gCost) {
                        neighbour.gCost = gCost;
                        neighbour.parent = currentNode;
                    }
                }

            }

        }

        return new List<Node>();
    }

    private int CalculateDistance(Node startNode, Node endNode) {
        // 4 or 8
        if (grid.allowDiagonals) {
            return Math.Max(Math.Abs(startNode.positionX - endNode.positionX), Math.Abs(startNode.positionY - endNode.positionY));
        } else {
            return Math.Abs(startNode.positionX - endNode.positionX) + Math.Abs(startNode.positionY - endNode.positionY);
        }
    }
    //trouver le min
    private Node MinimumCost(List<Node> openList) {
        Node minimumCost = openList[0];

        for (int i = 1; i < openList.Count; ++i) {
            if (openList[i].gCost < minimumCost.gCost) {
                minimumCost = openList[i];
            }
        }
        return minimumCost;
    }

    public List<Node> MakePath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current.parent != start) {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        if (drawPath) {
            draw = new List<Node>(path);
        }

        return path;
    }
    public Vector3 WorldPointFromNode(Node node) {
        return grid.PosFromNode(node);
    }
    //voir si c'est correcte
    private void OnDrawGizmos() {
        if (drawPath) {
            Gizmos.color = Color.yellow;
            if (draw.Count > 0) {
                foreach (Node n in draw) {
                    Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1) * grid.resolution);
                }
            }
        }
    }
}