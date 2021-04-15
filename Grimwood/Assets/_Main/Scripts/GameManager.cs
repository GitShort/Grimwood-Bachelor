﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;
using TMPro;

public class GameManager : MonoBehaviour
{
    bool _isGeneratorOn;

    public static GameManager instance;

    bool _isPlayerAlive;
    bool _playerLoseSoundPlayed;

    int _artifactsCount;
    int _artifactCollectedCount;
    List<string> _ArtifactNames = new List<string>();
    [SerializeField] Artifact[] _artifactObject;
    bool _artifactNamesAssigned;

    static readonly System.Random rnd = new System.Random();
    bool _soundPlayed;
    int _chosenSound = 0;
    [SerializeField] float _intervalBetweenSounds = 5f;
    [SerializeField] GameObject[] _SoundSpots;
    int _chosenSpot;

    [SerializeField] string[] _EnvironmentSounds;

    //Player lose screen
    bool _isTimerStarted;
    bool _isTimerFinished;
    float _timer;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        RenderSettings.fog = true;
    }

    void Start()
    {
        _artifactCollectedCount = 0;
        _artifactsCount = _artifactObject.Length;
        _isGeneratorOn = false;
        _isPlayerAlive = true;
        _soundPlayed = false;
        _artifactNamesAssigned = false;
        _playerLoseSoundPlayed = false;
        AudioManager.instance.Play("Environment", this.gameObject);
    }

    void Update()
    {
        if (_isPlayerAlive)
        {
            if (!_soundPlayed)
            {
                _soundPlayed = true;
                Invoke("PlayEnvironmentSound", _intervalBetweenSounds);
            }

            // ARTIFACT COLLECTION DEBUGGING
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                //_artifactCollectedCount++;
                //Debug.Log("Collected artifacts: " + _artifactCollectedCount);
                foreach (string item in _ArtifactNames)
                {
                    Debug.Log(item);
                }
            }

            if (!_artifactNamesAssigned)
            {
                int _artifactNamesCount = 0;
                _artifactNamesAssigned = true;
                foreach (string item in _ArtifactNames)
                {
                    if (_artifactObject != null)
                    {
                        _artifactObject[_artifactNamesCount].GetComponentInChildren<TextMeshPro>().text = item;
                    }
                    _artifactNamesCount++;
                }
            }
            Debug.Log("Collected artifacts: " + _artifactCollectedCount);
        }

        if (!_isPlayerAlive)
        {
            PlayerLose();
            if (!_playerLoseSoundPlayed)
            {
                PlayEnvironmentSound();
                _playerLoseSoundPlayed = true;
            }
        }
    }

    void PlayEnvironmentSound()
    {
        if (_EnvironmentSounds != null && _SoundSpots != null)
        {
            _chosenSound = rnd.Next(_EnvironmentSounds.Length);
            _chosenSpot = rnd.Next(_SoundSpots.Length);
            AudioManager.instance.Play(_EnvironmentSounds[_chosenSound], _SoundSpots[_chosenSpot]);
            _soundPlayed = false;
        }
    }

    void PlayerLose()
    {
        if (!_isTimerStarted && !_isTimerFinished)
        {
            _timer = 0f;
            _isTimerStarted = true;
        }
        if (_isTimerStarted && !_isTimerFinished)
        {
            _timer += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.SmoothStep(RenderSettings.fogDensity, 1f, _timer/2f);
            //Debug.Log(_timer);
            if (2f <= _timer)
            {
                _isTimerFinished = true;
                //Debug.Log("Stopped");
            }
        }
    }

    public bool GetIsGeneratorOn()
    {
        return _isGeneratorOn;
    }

    public void SetIsGeneratorOn(bool value)
    {
        _isGeneratorOn = value;
    }

    public bool GetIsPlayerAlive()
    {
        return _isPlayerAlive;
    }

    public void SetIsPlayerAlive(bool value)
    {
        _isPlayerAlive = value;
    }

    public int GetArtifactsCount()
    {
        return _artifactsCount;
    }

    public int GetArtifactCollectedCount()
    {
        return _artifactCollectedCount;
    }

    public void AddArtifactCollectedCount()
    {
        _artifactCollectedCount++;
    }

    public void AddArtifactNames(string name)
    {
        _ArtifactNames.Add(name);
    }
}
