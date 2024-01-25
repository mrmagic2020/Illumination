using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Linq;

public class LoadingManager : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public Transform sandGlassTransform;
    public string MenuSceneAddress;
    private AsyncOperationHandle<SceneInstance> menuSceneLoadHandle;
    private AsyncOperationHandle<IList<Locale>> localeLoadHandle;

    private List<Action> callbackList = new();
    private int totalLoadOperations = 0;
    private bool AllDone = false;



    // Start is called before the first frame update
    void Start()
    {
        LoadMainSceneAsync();
        LoadLocalizationAsync();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadMainSceneAsync()
    {
        totalLoadOperations++;
        menuSceneLoadHandle = Addressables.LoadSceneAsync(MenuSceneAddress, activateOnLoad: false);
        StartCoroutine(RegisterLoad(menuSceneLoadHandle, () =>
        {
            // Debug.Log("Scene initialization complete.");
            menuSceneLoadHandle.Result.ActivateAsync();
        }));
    }

    private void LoadLocalizationAsync()
    {
        totalLoadOperations++;
        AsyncOperationHandle<LocalizationSettings> handle = LocalizationSettings.InitializationOperation;
        // handle.WaitForCompletion();
        StartCoroutine(RegisterLoad(handle, () => {
            // Debug.Log("Localization initialization complete.");
        }));
    }

    private IEnumerator RegisterLoad<T>(AsyncOperationHandle<T> handle, Action callback)
    {
        while (!handle.IsDone)
        {
            float weight = 1f / totalLoadOperations;
            UpdateLoadingUI(handle.PercentComplete * 100f * weight, null);
            yield return null;
        }
        float finalWeight = 1f / totalLoadOperations;
        UpdateLoadingUI(handle.PercentComplete * 100f * finalWeight, callback);
    }

    private void UpdateLoadingUI(float percentage, Action callback)
    {
        if (callback != null)
        {
            callbackList.Add(callback);
        }
        loadingText.text = percentage.ToString("F0") + "%";

        // Check if all operations are complete
        if (callbackList.Count == totalLoadOperations && !AllDone)
        {
            // Debug.Log("Invoking callback...");
            AllDone = true;
            for (int i = 0; i < callbackList.Count; i++)
            {
                callbackList[i].Invoke();
            }
        }
    }

    private void OnDestroy()
    {
        // Addressables.Release(menuSceneLoadHandle);
    }
}
