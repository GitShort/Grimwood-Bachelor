using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    private float _clickValue = 1f;
    [SerializeField] float _increment;
    private Slider _slider;
    private float _maxSliderValue;
    private float _minSliderValue;
    private float _valueLimit;

    [SerializeField] AudioMixer _mixer;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _maxSliderValue = _slider.maxValue;
        _minSliderValue = _slider.minValue;

    }

    void Update()
    {
        _slider.value = _clickValue;
    }

    public void Increase()
    {
        if (_clickValue < _maxSliderValue)
        {
            _valueLimit = _maxSliderValue - _increment;
            if (_clickValue < _valueLimit)
            {
                _clickValue = _clickValue + _increment;
            }
            else
            {
                _clickValue = _maxSliderValue;
            }
        }
    }

    public void Decrease()
    {
        if (_clickValue > _minSliderValue)
        {
            if (_clickValue > _increment)
            {
                _clickValue = _clickValue - _increment;
            }
            else
            {
                _clickValue = _minSliderValue;
            }
        }
    }

    public void SetVolumeLevel(float sliderValue)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
    }

}
