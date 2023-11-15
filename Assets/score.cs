using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    public static GameObject Obj { get; private set; }
    void Awake()
    {
        score.Obj = obj;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
