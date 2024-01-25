using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTinter : MonoBehaviour
{
    public bool tintMode = true;
    public Color onColor;

    private Toggle toggle;
    private Color normColor;
    private bool tinted;
    
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<Toggle>(out toggle))
        {
            // offColor = toggle.targetGraphic.color;
            // if (tintMode) onColor = Tint(toggle.targetGraphic.color, onColor);
            normColor = toggle.targetGraphic.color;
            toggle.onValueChanged.AddListener(value =>
            {
                tinted = value;
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tinted)
        {
            toggle.targetGraphic.color = Tint(toggle.targetGraphic.color, onColor);
        }
        else
        {
            toggle.targetGraphic.color = normColor;
        }
    }

    private Color Tint(Color origial, Color tint)
    {
        return new Color(origial.r * tint.r, origial.g * tint.g, origial.b * tint.b) * tint.a;
    }
}
