using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    public Text bulletsRemaining;
    public Text scoreLabel;
    public Text highScoreLabel;

    void Start()
    {
        refresh();
    }

    private void Update()
    {
        //Score label is updated to UI
        scoreLabel.text = "Score: " + GameManager.instance.score;

        //High score label is updated to UI
        highScoreLabel.text = "High Score: " + GameManager.instance.highScore;
    }

    //function: refresh
    //purpose: Updates player's remaining bullet count UI
    public void refresh()
    {
        bulletsRemaining.text = "BulletsRemaining: " + GameManager.instance.bullets.ToString();

    }

}
