using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR;

public class RaySlider : MonoBehaviour
{
    public RaySelect raySelect;
    [HideInInspector] public float value = 0;
    
    private bool isSelect = false;

    private void Update() {
        if (isSelect && InputManager.GetBool(XRNode.LeftHand, CommonUsages.gripButton)) {
            var xPoint = transform.parent.InverseTransformPoint(raySelect.mov.ray.point).x;

            transform.localPosition = new Vector3((xPoint < 1 && xPoint > -1 ? xPoint : xPoint / Mathf.Abs(xPoint)), transform.localPosition.y, transform.localPosition.z);
        }

        value = (transform.localPosition.x + 1) / (1 + 1);
    }

    public void sliderSelect() { isSelect = true; }
    public void sliderUnselect() { isSelect = false; }
}
