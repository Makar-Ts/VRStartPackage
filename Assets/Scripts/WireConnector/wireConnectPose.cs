using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class wireConnectPose : MonoBehaviour
{
    public LayerMask mask;
    [SerializeField] private bool doParent = false;
    [HideInInspector] public GameObject connectedObject;
    private Transform connectedObjectPrevParent;
    public UnityEvent connectObject = new UnityEvent(), disconnectObject = new UnityEvent();

    private void OnTriggerStay(Collider other) {
        if (( mask & (1 << other.gameObject.layer)) != 0) {
            bool isGrabbed = other.gameObject.GetComponent<XRGrabInteractable>().isSelected;

            if (!isGrabbed && connectedObject == null) {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;

                connectedObject = other.gameObject;
                connectObject.Invoke();
                
                if (doParent) { 
                    //connectedObjectPrevParent = connectedObject.transform.parent;
                    connectedObject.transform.parent = transform; 

                    print("currentParent");

                    connectedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

                if (TryGetComponent<offColliders>(out offColliders col)) {
                    col.Off();
                }
            } else if (!isGrabbed && connectedObject == other.gameObject) {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;

                if (TryGetComponent<offColliders>(out offColliders col)) {
                    col.Off();
                }
            } else if (isGrabbed && other.gameObject == connectedObject) { 
                if (doParent) { 
                    connectedObject.transform.parent = null;  
                    print("prevParent");

                    connectedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                
                connectedObject = null;
                disconnectObject.Invoke();
            } 
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (( mask & (1 << other.gameObject.layer)) != 0) {
            bool isGrabbed = other.gameObject.GetComponent<XRGrabInteractable>().isSelected;

            if (isGrabbed) {
                if (TryGetComponent<offColliders>(out offColliders col)) {
                    col.Off();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (( mask & (1 << other.gameObject.layer)) != 0) {
            bool isGrabbed = other.gameObject.GetComponent<XRGrabInteractable>().isSelected;

            if (isGrabbed) {
                if (TryGetComponent<offColliders>(out offColliders col)) {
                    col.On();
                }

                if (connectedObject == other.gameObject) {
                    if (doParent) { 
                        connectedObject.transform.parent = null;    
                        print("prevParent");

                        connectedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }

                    connectedObject = null;
                    disconnectObject.Invoke();
                }
            } else {
                if (!doParent) {    
                    other.transform.position = transform.position;
                    other.transform.rotation = transform.rotation;
                }
            }
        }
    }
}
