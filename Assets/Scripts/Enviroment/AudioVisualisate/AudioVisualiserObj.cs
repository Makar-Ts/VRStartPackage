using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualiserObj : MonoBehaviour
{
    [SerializeField] private GameObject[] visualisers;
    [SerializeField] private Vector2 scaleRange;
    [SerializeField] private float sizeScale = 5f;
    [SerializeField] private float updateVar;
    [Space (15)]
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    [Serializable]
    private struct Plank {
        public Transform transform;
        public MeshRenderer renderer;

        public Plank(Transform transform_new, MeshRenderer renderer_new) {
            transform = transform_new;
            renderer = renderer_new;
        }
    }

    private List<Plank> planks = new List<Plank>();
    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();

        for (int i = 0; i < visualisers.Length; i++)
        {
            planks.Add(new Plank(visualisers[i].transform, visualisers[i].GetComponent<MeshRenderer>()));
        }
    }

    void Update() {
        float[] spectrum = new float[256];

        source.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 0; i < planks.Count; i++)
        {
            float num = spectrum[(int)(spectrum.Length/visualisers.Length*i)] * sizeScale;

            float power = Mathf.Lerp(planks[i].transform.localScale.x, Mathf.Lerp(scaleRange.x, scaleRange.y, (num > 1 ? 1 : num)), updateVar);

            planks[i].transform.localScale = new Vector3(power, planks[i].transform.localScale.y, planks[i].transform.localScale.z);

            var color = Color.Lerp(minColor, maxColor, (num > 1 ? 1 : num));
            planks[i].renderer.material.color = color;
            planks[i].renderer.material.SetColor("_EmissionColor", color);
        }
    }
}
