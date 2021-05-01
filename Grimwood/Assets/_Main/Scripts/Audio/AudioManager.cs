using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] _Sounds;

    List<AudioSource> _audioList = new List<AudioSource>();
    int index = 0;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (index < _audioList.Count)
        {
            AudioSource audioRef = _audioList[index];
            if (!audioRef.isPlaying)
            {
                _audioList.Remove(audioRef);
                Destroy(audioRef);
            }
            index++;
        }
        else
        {
            index = 0;
        }
    }

    public void Play(string name, GameObject go)
    {
        Sound s = Array.Find(_Sounds, sound => sound.GetName() == name);
        AudioSource audioCheck = null;
        if (s != null)
        {
            audioCheck = _audioList.Find(source => source.clip == s.GetClip());
        }
        if (s != null && (audioCheck == null || audioCheck.gameObject != go ))
        {
            AudioSource source = go.AddComponent<AudioSource>();
            _audioList.Add(source);
            source.clip = s.GetClip();
            source.volume = s.GetVolume();
            source.pitch = s.GetPitch();
            source.loop = s.GetLoop();
            source.spatialBlend = s.GetSpatialBlend();
            source.maxDistance = s.GetMaxSoundDistance();
            source.rolloffMode = s.GetAudioRolloffMode();
            source.outputAudioMixerGroup = s.GetAudioMixerGroup();
            source.Play();
        }
    }

    public void Stop(string name, GameObject go)
    {
        Sound s = Array.Find(_Sounds, sound => sound.GetName() == name);
        AudioSource audioCheck = null;
        if (s != null)
        {
            audioCheck = _audioList.Find(source => source.clip == s.GetClip());
        }
        if (audioCheck != null && audioCheck.isPlaying)
        {
            audioCheck.Stop();
        }
    }

    public void SetVolume(string name, float value)
    {
        Sound s = Array.Find(_Sounds, sound => sound.GetName() == name);
        AudioSource audioCheck = null;
        if (s != null)
        {
            audioCheck = _audioList.Find(source => source.clip == s.GetClip());
        }
        if (audioCheck != null && audioCheck.isPlaying)
        {
            audioCheck.volume = value;
        }
    }

}
