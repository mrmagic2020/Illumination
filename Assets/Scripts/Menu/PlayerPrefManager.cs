using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VariableType { Int, Float, Bool, String }

[System.Serializable]
public class FlexibleVariable
{
    public VariableType type;

    public int intValue;
    public float floatValue;
    public bool boolValue;
    public string stringValue;
}


public class PlayerPrefManager : MonoBehaviour
{
    public string key;
    public FlexibleVariable flexibleVariable;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Toggle>() != null)
        {
            bool value;
            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetInt(key) != 0;
            }
            else
            {
                value = flexibleVariable.boolValue;
            }
            GetComponent<Toggle>().isOn = value;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(int value)
    {
        PlayerPrefs.SetInt(key, value);
        Save(value);
    }

    public void Set(float value)
    {
        PlayerPrefs.SetFloat(key, value);
        Save(value);
    }

    public void Set(bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        Save(value);
    }

    public void Set(string value)
    {
        PlayerPrefs.SetString(key, value);
        Save(value);
    }

    private void Save(object value = null)
    {
        PlayerPrefs.Save();
        SendMessage("OnSettingsUpdate", value,SendMessageOptions.DontRequireReceiver);
    }
}
