using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechniqueUIManager : MonoBehaviour
{
    public static TechniqueUIManager instance = null;

    public GameObject TechniqueDetailsUI;

    public Text techniqueDescription;
    public Text techniqueName;
    public Text techniqueCooldown;
    public Text techniqueDamage;
    public Image techniqueImage;

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

}
