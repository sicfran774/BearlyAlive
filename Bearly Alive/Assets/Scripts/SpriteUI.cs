using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUI : MonoBehaviour
{
    public static SpriteUI instance = null;

    public Sprite sprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
