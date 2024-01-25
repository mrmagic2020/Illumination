using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public List<Button> affectedButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonInteractableState(toggleGroup.AnyTogglesOn());
    }

    private void SetButtonInteractableState(bool interactable)
    {
        foreach (var button in affectedButtons)
        {
            button.interactable = interactable;
        }
    }
}
