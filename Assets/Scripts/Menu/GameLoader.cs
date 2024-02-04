using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    [Tooltip("The parent object containing the saved games.")]
    public ToggleGroup toggleGroup;
    [Tooltip("The prefab of each file item in the list.")]
    public GameObject prefab;
    [Tooltip("The interactable property of the selected UI elements will be set to FALSE when no game save is selected.")]
    public List<Button> affectedButtons;

    private string path;
    private DirectoryInfo directoryInfo;
    private FileInfo[] files;

    private InputManager inputActions;

    void Awake()
    {
        inputActions = new InputManager();

        path = Application.persistentDataPath;
        directoryInfo = new(path);
        files = directoryInfo.GetFiles();
        if (!Directory.Exists(path))
        {
            directoryInfo.Create();
            Debug.Log($"Created persistent data path {path}");
        }
        else
        {
            // Debug.Log(path);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshList();
    }

    void OnEnable()
    {
        inputActions.GameLoader.Enable();
    }

    void OnDisable()
    {
        inputActions.GameLoader.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonInteractableState(toggleGroup.AnyTogglesOn());
        if (inputActions.GameLoader.Delete.ReadValue<float>() > 0)
        {
            Debug.Log(inputActions.GameLoader.Delete.ReadValue<float>());
            OnDeleteRequest();
        }
    }

    private void RefreshList()
    {
        files = directoryInfo.GetFiles();
        foreach (Transform child in toggleGroup.transform)
        {
            // Destroy all items except for the hidden prefab.
            if (child.gameObject.activeSelf) 
            {
                // Debug.Log($"Destroying {child.gameObject.name}");
                Destroy(child.gameObject);
            }
        }
        foreach (var file in files)
        {
            GameObject obj = Instantiate(prefab, toggleGroup.transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().SetText(file.Name);
            obj.SetActive(true);
        }
    }

    private void SetButtonInteractableState(bool interactable)
    {
        foreach (var button in affectedButtons)
        {
            button.interactable = interactable;
        }
    }

    public void OnNewSaveRequest()
    {
        string filePath = path + $"/AutoSave {DateTime.Now.ToString().Replace("/", "-")}.illumination";
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        else
        {
            Debug.LogError("An error occurred while creating a new game save.");
        }
        RefreshList();
    }

    public void OnDeleteRequest()
    {
        if (new List<Toggle>(toggleGroup.ActiveToggles()).Count == 0)
        {
            return;
        }

        Toggle toggle = new List<Toggle>(toggleGroup.ActiveToggles())[0];
        string filePath = path + $"/{toggle.GetComponentInChildren<TextMeshProUGUI>().text}";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogError("Cannot find referenced game save for this GameObject.");
        }
        RefreshList();
    }
}
