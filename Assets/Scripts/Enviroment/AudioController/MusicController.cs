using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public Clip[] clips;
    [Space(15)]
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private TextMeshPro text;
    private AudioSource source;
    private int playedSongIndex = 0;
    private bool isOnPause = false;

    private void Start() {
        source = GetComponent<AudioSource>();
        source.clip = clips[0].clip;
        if (playOnAwake) source.Play();

        text.text = clips[0].name;

        StartCoroutine("CheckFinishAudio");
    }

    public void Play() {
        if (source.isPlaying) {
            source.Pause();
            isOnPause = true;
        } else {
            source.UnPause();
            isOnPause = false;
        }
    }

    public void ChangeSong(int step) {
        var index = playedSongIndex+step;

        if (index < 0) {
            source.clip = clips[(clips.Length)+index].clip;
            playedSongIndex = (clips.Length)+index;
        } else if (index > clips.Length-1) {
            source.clip = clips[index-(clips.Length)].clip;
            playedSongIndex = index-(clips.Length);
        } else {
            source.clip = clips[index].clip;
            playedSongIndex = index;
        }

        text.text = clips[playedSongIndex].name;

        source.Play();
    }

    IEnumerator CheckFinishAudio()
    {
            while(isOnPause || source.isPlaying) {
                yield return new WaitForSeconds(0.2f);
            }

            ChangeSong(1);
            
            StartCoroutine("CheckFinishAudio");
    }
}

[Serializable]
public class Clip {
    public AudioClip clip;
    public string name;
}