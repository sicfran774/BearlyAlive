using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    public Text bulletsRemaining;

    void Start()
    {
        refresh();
    }

    //function: refresh
    //purpose: Updates player's remaining bullet count UI
    public void refresh()
    {
        bulletsRemaining.text = "BulletsRemaining: " + GameManager.instance.bullets.ToString();
    }

}
