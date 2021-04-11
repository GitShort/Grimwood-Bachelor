using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] string _name;

    [SerializeField] AudioClip _clip;

    [Range(0f, 1f)]
    [SerializeField] float _volume = 1f;
    [Range(.1f, 3f)]
    [SerializeField] float _pitch = 1f;

    [SerializeField] bool _loop;

    [Range(0f, 1f)]
    [SerializeField] float _spatialBlend = 1f;

    [HideInInspector]
    public AudioSource source;


    public string GetName()
    {
        return _name;
    }

    public AudioClip GetClip()
    {
        return _clip;
    }

    public float GetVolume()
    {
        return _volume;
    }

    public float GetPitch()
    {
        return _pitch;
    }

    public bool GetLoop()
    {
        return _loop;
    }

    public float GetSpatialBlend()
    {
        return _spatialBlend;
    }
}
