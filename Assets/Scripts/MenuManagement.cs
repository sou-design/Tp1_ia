using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{   private GameObject gameobj;
    void Start()
    {
        gameobj = score.Obj;
        gameobj.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        gameobj.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
        gameObject.SetActive(false);
    }
}
