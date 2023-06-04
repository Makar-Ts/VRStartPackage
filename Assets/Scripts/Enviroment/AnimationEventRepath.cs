using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventRepath : MonoBehaviour
{
    [SerializeField] private UnityEvent[] events;

    public void Repath(int eventID) { events[eventID].Invoke(); }
}
