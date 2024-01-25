using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLightControl : MonoBehaviour
{
    public GameObject[] Lights;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MLC");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
