using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickSound : MonoBehaviour
{
    public GameObject SoundEffect;
    private AudioSource[] sources;
    private EventTrigger eventTrigger;
    // Start is called before the first frame update
    void Start()
    {
        sources = SoundEffect.GetComponents<AudioSource>();

        // Automatically sets up sound effect
        if (!gameObject.TryGetComponent<EventTrigger>(out eventTrigger))
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDownEvent = new()
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDownEvent.callback.AddListener((BaseEventData eventData) => 
            {
                OnButtonPress();
                // Debug.Log($"Pressed: {gameObject.name}");
            });

            EventTrigger.Entry pointerUpEvent = new()
            {
                eventID = EventTriggerType.PointerUp
            };
            pointerUpEvent.callback.AddListener((BaseEventData eventData) => 
            {
                OnButtonRelease();
                // Debug.Log($"Released: {gameObject.name}");
            });

            eventTrigger.triggers.Add(pointerUpEvent);
            eventTrigger.triggers.Add(pointerDownEvent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress()
    {
        sources[0].Play();
    }

    public void OnButtonRelease()
    {
        sources[1].Play();
    }
}
