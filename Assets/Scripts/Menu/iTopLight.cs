using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; // Required for 2D lights

public class iTopLight : MonoBehaviour
{
    [Header("Game Objects Import")]
    public GameObject globalLight;
    public GameObject SCLocalLight;
    public GameObject ScrollHint;
    public GameObject soundEffect;
    public GameObject backgroundMusic;
    // [Tooltip("Selected text elements will appear after activation of global light.")]
    // public GameObject[] UITexts;
    [Tooltip("Selected elements will appear after activation of global light.")]
    public GameObject UICanvas;
    public float transitionDuration = 2;
    [Header("Flickering Effect")]
    public bool flicker = true;
    public float flickerSpeed = 10;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2;
    [Header("Scrolling Constraints")]
    public float sensitivity = 1;
    public float minInnerRadius = 0;
    public float maxInnerRadius = 15;
    public float minOuterRadius = 1;
    public float maxOuterRadius = 30;

    private Light2D light2D;
    private Light2D globalLight2D;
    private Light2D scLight2D;
    private TextMeshProUGUI SCTextMeshPro;
    private SpriteRenderer SCSpriteRenderer;
    private Crossfading crossfading;
    private AudioSource backgroundMusicSource;
    private float random;
    private InputManager inputActions;
    private float scroll;
    private float innerRadius = 0;
    private float outerRadius = 1;
    private bool freeze = false;
    private bool showScrollHint = true;
    private Coroutine coroutine_TSH;
    private readonly string key = "Menu.Settings.Game_Settings.Entrance_Animation";
    private bool isEnabled = true;

    void Awake()
    {
        inputActions = new InputManager();
    }

    void Start()
    {
        backgroundMusicSource = backgroundMusic.GetComponent<AudioSource>();
        
        if (PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) == 0)
        {
            isEnabled = false;
            backgroundMusicSource.volume = 1;
            backgroundMusicSource.Play();
            return;
        }

        light2D = GetComponent<Light2D>();
        globalLight2D = globalLight.GetComponent<Light2D>();
        scLight2D = SCLocalLight.GetComponent<Light2D>();

        crossfading = soundEffect.GetComponent<Crossfading>();

        // Handle the text separately. 
        // Only ONE TextMeshPro component should exist. 
        SCTextMeshPro = ScrollHint.GetComponentInChildren<TextMeshProUGUI>(true);
        SCSpriteRenderer = ScrollHint.GetComponentInChildren<SpriteRenderer>(true);

        light2D.intensity = 1;
        light2D.pointLightInnerRadius = minInnerRadius;
        light2D.pointLightOuterRadius = minOuterRadius;

        globalLight2D.intensity = 0;
        scLight2D.intensity = 0;

        crossfading.volume = 0.2f;
        crossfading.pitch = 1;
        crossfading.Play();
        
        backgroundMusicSource.volume = 0;
        backgroundMusicSource.loop = true;

        if (SCTextMeshPro)
        {
            SCTextMeshPro.color = new Color(SCTextMeshPro.color.r, SCTextMeshPro.color.g, SCTextMeshPro.color.b, 0);
            SCSpriteRenderer.color = new Color(SCSpriteRenderer.color.r, SCSpriteRenderer.color.g, SCSpriteRenderer.color.b, 1);
        }
        // if (SCText)
        // {
        //     SCTextMeshPro = SCText.GetComponent<TextMeshProUGUI>();
        //     SCTextMeshPro.color = new Color(SCTextMeshPro.color.r, SCTextMeshPro.color.g, SCTextMeshPro.color.b, 0);
        // }

        if (!ScrollHint.activeSelf)
            ScrollHint.SetActive(true);
        ToggleUI(false);
        coroutine_TSH = StartCoroutine(ToggleScrollHint(true, 2));
    }

    void OnEnable()
    {
        inputActions.Menu.iTopLight.Enable();
    }

    void OnDisable()
    {
        inputActions.Menu.iTopLight.Disable();
    }

    void Update()
    {
        if (!isEnabled) return;

        if (flicker && !freeze)
        {
            random = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = Mathf.Lerp(light2D.intensity, random, flickerSpeed * Time.deltaTime);
        }

        if (freeze)
        {
            return;
        }

        scroll = inputActions.Menu.iTopLight.ReadValue<float>();
        // if (scroll != 0) Debug.Log("Scroll Input: " + scroll.ToString());

        if (showScrollHint && Mathf.Abs(scroll) > 0)
        {
            showScrollHint = false;
            if (coroutine_TSH != null)
            {
                StopCoroutine(coroutine_TSH);
            }
            StartCoroutine(ToggleScrollHint(false, 2));
        }

        innerRadius += scroll * sensitivity / 15;
        outerRadius += scroll * sensitivity / (15 * ((maxInnerRadius - minInnerRadius) / (maxOuterRadius - minOuterRadius)));

        innerRadius = Mathf.Clamp(innerRadius, minInnerRadius, maxInnerRadius);
        outerRadius = Mathf.Clamp(outerRadius, minOuterRadius, maxOuterRadius);

        light2D.pointLightInnerRadius = innerRadius;
        light2D.pointLightOuterRadius = outerRadius;

        crossfading.volume += scroll * sensitivity / (15 * ((maxOuterRadius - minOuterRadius) / 0.8f));
        crossfading.volume = Mathf.Clamp(crossfading.volume, 0.2f, 1f);

        if (innerRadius == maxInnerRadius && outerRadius == maxOuterRadius)
        {
            freeze = true;
            // inputActions.Disable();
            
            StartCoroutine(ActivateGlobalLight());
            StartCoroutine(crossfading.FadeOut(transitionDuration));

            ToggleUI(true);
            
            backgroundMusicSource.volume = 1;
            backgroundMusicSource.PlayDelayed(transitionDuration + 1);
        }
    }

    private IEnumerator ActivateGlobalLight()
    {
        float time = 0;
        float GLInitialIntensity = globalLight2D.intensity;
        float ILInitialIntensity = light2D.intensity;
        while (time < transitionDuration)
        {
            globalLight2D.intensity = Mathf.Lerp(GLInitialIntensity, 1f, time / transitionDuration);
            light2D.intensity = Mathf.Lerp(ILInitialIntensity, 0f, time / transitionDuration);
            light2D.pointLightInnerRadius = Mathf.Lerp(maxInnerRadius, minInnerRadius, time / transitionDuration);
            light2D.pointLightOuterRadius = Mathf.Lerp(maxOuterRadius, minOuterRadius, time / transitionDuration);
            time += Time.deltaTime;
            yield return null;
        }

        globalLight2D.intensity = 1f;
    }

    private IEnumerator ToggleScrollHint(bool status, int duration)
    {
        float time = 0;
        float initialIntensity = scLight2D.intensity;
        float initialA = Mathf.Min(SCTextMeshPro.color.a, SCSpriteRenderer.color.a);
        while (time < duration)
        {
            scLight2D.intensity = Mathf.Lerp(initialIntensity, status ? 1f : 0f, time / duration);
            SCTextMeshPro.color = new Color(SCTextMeshPro.color.r, SCTextMeshPro.color.g, SCTextMeshPro.color.b, Mathf.Lerp(initialA, status ? 1f : 0f, time / duration));
            if (status == false)
            {
                SCSpriteRenderer.color = new Color(SCSpriteRenderer.color.r, SCSpriteRenderer.color.g, SCSpriteRenderer.color.b, Mathf.Lerp(initialA, 0f, time / duration));
            }
            time += Time.deltaTime;
            yield return null;
        }

        scLight2D.intensity = status ? 1f : 0f;

        // if (ScrollHint.activeSelf != status)
        //     ScrollHint.SetActive(status);
    }

    private void ToggleUI(bool status)
    {
        // for (int i = 0; i < UITexts.Length; i++)
        // {
        //     if (UITexts[i].activeSelf != status)
        //         UITexts[i].SetActive(status);
        // }
        if (UICanvas.activeSelf != status)
            UICanvas.SetActive(status);
    }
}
