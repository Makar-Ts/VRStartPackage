using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualiserLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Vector2 hightRange;
    [SerializeField] private float len;
    [Range(6, 11)]
    [SerializeField] private int quality = 6;
    [SerializeField] private float sizeScale = 5f;
    [SerializeField] private float updateVar;
    [Space (15)]
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;
    private AudioSource source;
    private int spectrumCount;

    void Start() {
        source = GetComponent<AudioSource>();

        spectrumCount = (int)Mathf.Pow(2, quality);
        line.positionCount = spectrumCount;
        line.useWorldSpace = false;

        for (int i = 0; i < spectrumCount; i++)
        {
            line.SetPosition(i, new Vector3(0, 0, len/spectrumCount*i));
        }
    }

    void Update() {
        float[] spectrum = new float[(int)Mathf.Pow(2, quality+2)];

        source.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        Gradient gr = new Gradient();
        List<GradientColorKey> keys = new List<GradientColorKey>();
        float numSums = 0f;

        for (int i = 0; i < spectrumCount; i++)
        {
            float num = spectrum[i] * sizeScale;
            numSums += num;

            var pos = line.GetPosition(i);

            float power = Mathf.Lerp(pos.y, Mathf.Lerp(hightRange.x, hightRange.y, (num > 1 ? 1 : num)), updateVar);

            line.SetPosition(i, new Vector3(pos.x, power, pos.z));

            if (i%(spectrumCount/8) == 0) {
                var med = numSums / (spectrumCount/8);

                var color = Color.Lerp(minColor, maxColor, (med > 1 ? 1 : med));
                keys.Add(new GradientColorKey(color, 1/spectrumCount*i));

                numSums = 0;
            }
        }

        gr.colorKeys = keys.ToArray();
        line.colorGradient = gr;
    }
}
