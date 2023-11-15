using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicObjectsCounter : MonoBehaviour
{
    public static MagicObjectsCounter instance;
    public TMP_Text ScoreText;
    public TMP_Text ScoreGameOver;
    public TMP_Text ScoreVictory;
    public Victory victory;
    public static int currentScore =0;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ScoreText.text = currentScore.ToString();
    }
    public void increaseScore(int v)
    {
        currentScore += v;
        ScoreText.text= currentScore.ToString();
        //ScoreGameOver.text = currentScore.ToString();
        ScoreVictory.text = currentScore.ToString();
        
    }
    // Update is called once per frame
    void Update()
    {
        victory.SetUp(currentScore);
    }
}
