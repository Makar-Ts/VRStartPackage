using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwoTimesActivate : MonoBehaviour
{
    private bool _enabled = false;
    [SerializeField] private UnityEvent onActivate, onDisable;

    public void Toggle() {
        _enabled = !_enabled;

        if (_enabled) {
            onActivate.Invoke();
        } else {
            onDisable.Invoke();
        }
    }
}
