using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool _isGeneratorOn;

    public static GameManager instance;

    bool _isPlayerAlive;
    bool _playerLoseSoundPlayed;

    int _artifactsCount;
    List<string> _ArtifactNames = new List<string>();
    [SerializeField] Artifact[] _artifactObject;
    List<string> _CollectedArtifacts = new List<string>(); // Used to check collected artifacts in-game through pause menu
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

    bool _isPaused;
    bool _isFinished;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _CollectedArtifacts = new List<string>();
        RenderSettings.fog = true;
    }

    void Start()
    {
        _artifactsCount = _artifactObject.Length;
        _isGeneratorOn = false;
        _isPlayerAlive = true;
        _soundPlayed = false;
        _artifactNamesAssigned = false;
        _playerLoseSoundPlayed = false;
        _isPaused = false;
        _isFinished = false;
        
        if(!SceneManager.GetActiveScene().name.Equals("Main Menu"))
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
            //Debug.Log("Collected artifacts: " + _CollectedArtifacts.Count);
        }

        if (!_isPlayerAlive || _isFinished)
        {
            BlurBackground();
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

    void BlurBackground()
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

    public List<string> GetCollectedArtifacts()
    {
        return _CollectedArtifacts;
    }

    public int GetCollectedArtifactCount()
    {
        return _CollectedArtifacts.Count;
    }

    public void AddCollectedArtifact(string name)
    {
        _CollectedArtifacts.Add(name);
    }

    public void AddArtifactNames(string name)
    {
        _ArtifactNames.Add(name);
    }

    public bool GetIsPaused()
    {
        return _isPaused;
    }

    public void SetIsPaused(bool value)
    {
        _isPaused = value;
    }

    public void SetIsFinished(bool value)
    {
        _isFinished = value;
    }

    public bool GetIsFinished()
    {
        return _isFinished;
    }
}
