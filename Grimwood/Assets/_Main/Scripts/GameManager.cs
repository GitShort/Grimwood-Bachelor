using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    bool _isGeneratorOn;

    public static GameManager instance;

    bool _isPlayerAlive;
    int collectiblesCount; // think of a new name for collectibe hints

    static readonly System.Random rnd = new System.Random();
    bool _soundPlayed;
    int _chosenSound = 0;
    [SerializeField] float _intervalBetweenSounds = 5f;
    [SerializeField] GameObject[] _SoundSpots;
    int _chosenSpot;

    [SerializeField] string[] _EnvironmentSounds;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        RenderSettings.fog = true;
    }

    void Start()
    {
        collectiblesCount = 0;
        _isGeneratorOn = false;
        _isPlayerAlive = false;
        _soundPlayed = false;
        AudioManager.instance.Play("Environment", this.gameObject);
    }

    void Update()
    {
        if (!_soundPlayed)
        {
            _soundPlayed = true;
            Invoke("PlayEnvironmentSound", _intervalBetweenSounds);
        }
    }

    void PlayEnvironmentSound()
    {
        Debug.Log("SCREAM");
        _chosenSound = rnd.Next(_EnvironmentSounds.Length);
        _chosenSpot = rnd.Next(_SoundSpots.Length);
        AudioManager.instance.Play(_EnvironmentSounds[_chosenSound], _SoundSpots[_chosenSpot]);
        _soundPlayed = false;
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
}
