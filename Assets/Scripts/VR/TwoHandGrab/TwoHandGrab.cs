using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrab : MonoBehaviour
{
    [SerializeField] private Transform connector;
    private XRSimpleInteractable interact;
    private Transform currentSelector;
    private Quaternion instanceConnectorRotation;


    private void Start() {
        interact = GetComponent<XRSimpleInteractable>();

        interact.selectEntered.AddListener(Selected);
        interact.selectExited.AddListener(Diselected);

        instanceConnectorRotation = connector.localRotation;
    }

    private void Update() {
        if (interact.isSelected) {
            //connector.LookAt(currentSelector); 
            var rotation = Quaternion.LookRotation(currentSelector.position-connector.position);
            connector.localRotation = new Quaternion(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }
    }

    private void Selected(SelectEnterEventArgs args) {
        currentSelector = args.interactorObject.transform;
    }

    private void Diselected(SelectExitEventArgs args) {
        currentSelector = null;

        connector.localRotation = instanceConnectorRotation;
    }
}
