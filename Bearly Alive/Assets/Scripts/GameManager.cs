using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int bullets = 15;
    public int _MAX_AMMO = 15;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Decrements weapon's ammo
    public void DecreaseAmmo(int amount)
    {
        bullets -= amount;
    }

    // Sets the weapon's ammo back to max capacity
    // Used in Reload function

    public void SetAmmo()
    {
        bullets = _MAX_AMMO;
    }


}
