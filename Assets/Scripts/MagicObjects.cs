using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public int value =1;
    float rotatingspeed = 45f;
    private Transform rotateAround;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward, rotatingspeed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if(other.gameObject.CompareTag("witch"))
        {
            //Debug.Log("objects in collision");
            Destroy(gameObject);
            MagicObjectsCounter.instance.increaseScore(value);
        }
    }
}
