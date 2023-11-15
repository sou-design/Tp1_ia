using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarEnemy : MonoBehaviour {

    public PathGrid grid;
    
    public GameObject destination;
    private List<Node> path;
    private List<Node> draw = new List<Node>();
    public bool drawPath;
    Vector3 End;
    bool oldAllowDiago;

    // Movement
    float lastMovement = 0;
    public float delay = 2.0f;
    int nextNodeIndex = -1;

    void FixedUpdate() {
        Vector3 deb = transform.position;
        Vector3 fin = destination.transform.position;
        float currentTime = Time.fixedTime;

        if (lastMovement + delay >= currentTime) {
            return;
        }
   
        lastMovement += delay;

        if (End != fin || grid.allowDiagonals != oldAllowDiago) {

            path = GetPath(deb, fin);

            if(path != null) {
                nextNodeIndex = 0;
            } else {
                nextNodeIndex = -1;
            }

            End = fin;
            oldAllowDiago = grid.allowDiagonals;
        }

    
        if (nextNodeIndex == -1) {
            return;
        }

        // We reached destination
        if(nextNodeIndex == path.Count) {
            Debug.Log("REACHED DESTINATION");
            //gameOver.setup();
            //gameObject.GetComponent<AStarEnemy>().enabled = false;
            //GameObject g = GameObject.Find("skeleton");
            //g.gameObject.GetComponent<DijkstraEnemy>().enabled = false;
            return;
        }

        Node currentPositionN = grid.NodeFromPos(transform.position);

        int horizontal = path[nextNodeIndex].positionX - currentPositionN.positionX;
        int vertical = path[nextNodeIndex].positionY - currentPositionN.positionY;

        ++nextNodeIndex;

        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromPos(nextPosition);

        // check if collider
        if (nextNode.isCollider) {
            return;
        }

        // sinon move
        transform.position = nextPosition;

    }

    public List<Node> GetPath(Vector3 sPos, Vector3 ePos) {

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = grid.NodeFromPos(sPos);
        Node endNode = grid.NodeFromPos(ePos);

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.parent = null;
        openList.Add(startNode);

        while(openList.Count != 0) {

            Node currentNode = MinimumCost(openList);
            if(currentNode == endNode) {
                return MakePath(startNode, endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbourList = grid.GetNeighbourNodes(currentNode);

            foreach(Node neighbour in neighbourList) {
                
                // check if colliders or neighbours
                if(closedList.Contains(neighbour) || neighbour.isCollider) {
                    continue;
                }
                int gCost = currentNode.gCost + CalculateDistance(neighbour, currentNode);
                int hCost = CalculateDistance(neighbour, endNode);

                if (!openList.Contains(neighbour)) {
                    neighbour.gCost = gCost;
                    neighbour.hCost = hCost;
                    neighbour.parent = currentNode;
                    openList.Add(neighbour);

                } else {
                    int fCost = gCost + hCost;
                    if (fCost < neighbour.fCost) {
                        neighbour.gCost = gCost;
                        neighbour.hCost = hCost;
                        neighbour.parent = currentNode;
                    }
                }

            }

        }



        return new List<Node>() ;
    }

    private int CalculateDistance(Node startNode, Node endNode) {
        //4 or 8
        if (grid.allowDiagonals) {
            return Math.Max( Math.Abs(startNode.positionX - endNode.positionX), Math.Abs(startNode.positionY - endNode.positionY) );
        } else {
            return Math.Abs(startNode.positionX - endNode.positionX) + Math.Abs(startNode.positionY - endNode.positionY);
        }
    }

    private Node MinimumCost(List<Node> openList) {
        Node minCost = openList[0];

        for (int i = 1; i < openList.Count; ++i) {
            if(openList[i].fCost < minCost.fCost) {
                minCost = openList[i];
            }
        }
        return minCost;
    }

    public List<Node> MakePath(Node start, Node end) {

        List<Node> path = new List<Node>();
        Node current = end;
        while (current != start) {
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

    private void OnDrawGizmos() {
        if (drawPath) {
            Gizmos.color = Color.cyan;
            if (draw.Count > 0) {
                foreach (Node n in draw) {//draw path in debug mode
                    Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1) * grid.resolution);
                }
            }
        }
    }
}