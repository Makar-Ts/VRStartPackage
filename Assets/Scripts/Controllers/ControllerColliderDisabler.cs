using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerColliderDisabler : MonoBehaviour
{
    private Collider collider;
    private XRDirectInteractor interactor;
    private bool is_update = true;

    private void Start() {
        collider = GetComponent<Collider>();
        interactor = transform.parent.parent.parent.GetComponent<XRDirectInteractor>();

        //print(gameObject.ToString() + " interact " + interactor.ToString());
    }

    private void Update() {
        if (interactor.hasSelection) {
            collider.enabled = false;
            is_update = false;
        } else {
            if (!is_update) {
                is_update = true;
                StartCoroutine(waiter());
            }
        }
    }

    public void ColliderDisable() {

    }

    IEnumerator waiter() {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = true;
    }
}
