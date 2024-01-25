using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popup;
    public bool defaultStatus = false;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        if (popup.activeSelf != defaultStatus)
            popup.SetActive(defaultStatus);
        if (TryGetComponent<Button>(out button))
        {
            button.onClick.AddListener(SetPopup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPopup(bool status)
    {
        popup.SetActive(status);
    }

    public void SetPopup()
    {
        popup.SetActive(!popup.activeSelf);
    }

    public bool TogglePopup()
    {
        popup.SetActive(!popup.activeSelf);
        return popup.activeSelf;
    }

    public void CancelQuit()
    {
        popup.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }
}
