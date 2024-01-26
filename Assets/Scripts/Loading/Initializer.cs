using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        InitializePlayerPrefs();
    }

    private void InitializePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("init"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("init", 1);
            Debug.Log("Player Settings Initialized.");
        }
    }
}
