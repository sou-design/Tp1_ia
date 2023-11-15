using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private float moveH, moveV;
    public PathGrid grid;
    [SerializeField] private float moveSpeed = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveH = Input.GetAxis("Horizontal") * moveSpeed;
        moveV = Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity = new Vector2(moveH, moveV);//OPTIONAL rb.MovePosition();

        Vector2 direction = new Vector2(moveH, moveV);
        Node nextNode = grid.NodeFromPos(direction);
        if (nextNode.isCollider)
        {
            return;
        }
        else
        {
            FindObjectOfType<PlayerAnimation>().SetDirection(direction);
        }
    }
}

