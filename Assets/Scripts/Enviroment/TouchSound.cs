using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float pitchRange = 0.05f, maxImpulse = 1f;
    [SerializeField] private LayerMask mask;
    private float audioSourcePitch;

    private void Start() {
        audioSourcePitch = audioSource.pitch;
    }

    private void OnCollisionEnter(Collision other) {
        if (((mask & (1 << other.collider.gameObject.layer)) != 0)) {
            if (audioSource) {
                audioSource.pitch = UnityEngine.Random.Range(audioSourcePitch-pitchRange, audioSourcePitch+pitchRange);
                audioSource.PlayOneShot(clip, (other.impulse.magnitude > maxImpulse ? 1 : other.impulse.magnitude / maxImpulse));
            }
        }
    }
}
