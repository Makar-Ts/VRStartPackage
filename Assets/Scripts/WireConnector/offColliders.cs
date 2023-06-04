using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class offColliders : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;

    public void Off() {
        foreach (var item in colliders) {
            item.isTrigger = true;
        }
    }

    public void On() {
        foreach (var item in colliders) {
            item.isTrigger = false;
        }
    }
}
