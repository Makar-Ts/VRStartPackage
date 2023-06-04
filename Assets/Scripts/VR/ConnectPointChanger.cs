using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;
using System;

public class ConnectPointChanger : MonoBehaviour
{
    [Serializable]
    class ConnectPoint {
        public Transform connectPoint;
        public XRNode node;
    }

    [SerializeField] private ConnectPoint[] connectPoints;
    [HideInInspector] public XRGrabInteractable grabInteractable;
    [HideInInspector] public XRNode node;

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelected);
        grabInteractable.selectExited.AddListener(OnExited);
    }

    private void OnSelected(SelectEnterEventArgs args) {
        var myItems = FindObjectsOfType(typeof(XRDirectInteractor)) as XRDirectInteractor[];
        foreach (XRDirectInteractor item in myItems)
        {
            if (item.IsSelecting(grabInteractable)) {
                var selectedController = item.gameObject.GetComponent<XRController>();
                node = selectedController.controllerNode;

                foreach (var point in connectPoints)
                {
                    if (point.node == node) {
                        grabInteractable.attachTransform = point.connectPoint;
                        break;
                    }
                }
                break;
            }
        } 
    }

    private void OnExited(SelectExitEventArgs args) {
        grabInteractable.attachTransform = null;
    }
}
