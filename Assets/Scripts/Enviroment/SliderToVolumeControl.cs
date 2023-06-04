using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderToVolumeControl : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private void Update() {
        source.volume = (transform.localPosition.x + 1) / 2;
    }
}
