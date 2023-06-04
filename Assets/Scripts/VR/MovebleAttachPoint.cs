using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

public class MovebleAttachPoint : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;
    [HideInInspector] public XRGrabInteractable grabInteractable;
    [HideInInspector] public XRNode node;
    private XRController selectedController;

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.attachTransform = attachPoint;

        grabInteractable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args) {
        var myItems = FindObjectsOfType(typeof(XRDirectInteractor)) as XRDirectInteractor[];
        foreach (XRDirectInteractor item in myItems)
        {
            if (item.IsSelecting(grabInteractable)) {
                selectedController = item.gameObject.GetComponent<XRController>();
                node = selectedController.controllerNode;

                attachPoint.transform.position = selectedController.transform.position;
                attachPoint.transform.rotation = selectedController.transform.rotation;
            }
        } 
    }
}
