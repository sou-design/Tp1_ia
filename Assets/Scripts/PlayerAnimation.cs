using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    //tableaux des directions
    public string[] staticDirections = { "Static N",  "Static W", "Static S",  "Static E" };
    public string[] DynamicDirections = { "Run N",  "Run W",  "Run S",  "Run E" };
    //Last direction
    int LDirection;
    //get animator component
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SetDirection(Vector2 direction)
    {
        string[] directionTab = null;

        if(direction.magnitude < 0.01)
        {
            directionTab = staticDirections;
        }
        else
        {
            directionTab = DynamicDirections;

            LDirection = GetIndex(direction);
        }

        anim.Play(directionTab[LDirection]);
    }

    private int GetIndex(Vector2 direction)
    {
        Vector2 norDir = direction.normalized;

        float step = 360 / 4;//4 directions
        float offset = step / 2;

        float angle = Vector2.SignedAngle(Vector2.up, norDir);

        angle += offset;

        if(angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}
