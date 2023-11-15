using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(int score)
    {
        if (score == 15)
        {
            gameObject.SetActive(true);
        }
    }
    public void Replay()
    {
        SceneManager.LoadSceneAsync(1);
    }
    }
