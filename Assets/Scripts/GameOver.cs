using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setup()
    {
        
        gameObject.SetActive(true);
    }
    public void Replay()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
