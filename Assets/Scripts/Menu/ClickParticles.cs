using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickParticles : MonoBehaviour
{
    private Camera mainCamera;
    private ParticleSystem particle;
    private InputManager inputActions;
    public Vector2 mousePosition;
    public Vector3 worldPosition;

    void Awake()
    {
        inputActions = new InputManager();
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        inputActions.Menu.Click.Enable();
    }

    void OnDisable()
    {
        inputActions.Menu.Click.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // click = inputActions.Menu.Click.ReadValue<float>();
        // if (click != 0)
        // {
        //     // Debug.Log("Click action value: " + click.ToString());
        //     particle.Play();
        // }
    }

    void OnClick()
    {
        // Debug.Log("Click Detected.");
        mousePosition = Mouse.current.position.ReadValue();
        worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        particle.GetComponent<Transform>().position = worldPosition;
        particle.Play();
    }
}
