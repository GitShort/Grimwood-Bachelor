using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class AttributeStorage
{
    #region General attributes
    [field: Header("General attributes")]
    [SerializeField] float _defaultPlayerMoveSpeed = 3f;
    [SerializeField] Transform _playerHead;
    [SerializeField] Volume _ppVolume;
    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] EnemyController _enemyController;
    [SerializeField] float _castsHeightOffset = 0.75f;
    [SerializeField] Renderer _enemyRenderer;
    [SerializeField] LayerMask _enemyDetectsObj;
    [SerializeField] float _defaultEnemyMoveSpeed = 1.5f;
    #endregion

    #region Behaviors
    [field: Header("Freeze behavior attributes")]
    [SerializeField] float _saturationValueFreeze = -80f;
    [SerializeField] Color _colorFilterValueFreeze;
    [SerializeField] float _frozenPlayerMoveSpeed = 2f;
    [SerializeField] GameObject _freezingParticles;
    [SerializeField] float _changeDurationFreeze;

    [field: Header("Freeze and Blind attributes")]
    [SerializeField] LayerMask _includedLayersAtmosphere;
    [SerializeField] float _atmosphereDistance = 8f;

    [field: Header("Blind behavior attributes")]
    [SerializeField] float _fogDensityValueTarget = 0.3f;
    [SerializeField] float _changeDurationBlind;

    [field: Header("Light behavior attributes")]
    [SerializeField] FlashlightManager _flashlightManager;
    [SerializeField] float _flashlightEnemyMoveSpeedWeak = 0f;
    [SerializeField] float _lampEnemyMoveSpeedWeak = 0.5f;
    [SerializeField] float _flashlightEnemyMoveSpeedStrong = 1.5f;
    [SerializeField] float _lampEnemyMoveSpeedStrong = 1.75f;

    [field: Header("Camera flash behavior attributes")]
    [SerializeField] PhotoCameraManager _photoCamera;

    [field: Header("Seen by player behavior attributes")]
    [SerializeField] float _seenByPlayerEnemyMoveSpeed = 2f;

    [field: Header("Unseen by player behavior attributes")]
    [SerializeField] float _unseenByPlayerEnemyMoveSpeed = 2f;

    [field: Header("Damage seen player behavior attributes")]
    [SerializeField] float _changeDurationSeenDmg;
    [SerializeField] float _vignetteValueSeenDmg = 1f;
    [SerializeField] Color _colorFilterValueSeenDmg;

    #endregion

    #region Attributes return functions

    #region General
    public float GetDefaultPlayerMoveSpeed()
    {
        return _defaultPlayerMoveSpeed;
    }

    public Transform GetPlayerHead()
    {
        return _playerHead;
    }

    public Volume GetPPVolume()
    {
        return _ppVolume;
    }

    public PlayerMovement GetPlayerMove()
    {
        return _playerMove;
    }

    public EnemyController GetEnemyController()
    {
        return _enemyController;
    }

    public float GetCastsHeightOffset()
    {
        return _castsHeightOffset;
    }

    public LayerMask GetEnemyDetectsObj()
    {
        return _enemyDetectsObj;
    }

    public float GetDefaultEnemyMoveSpeed()
    {
        return _defaultEnemyMoveSpeed;
    }

    #endregion

    #region Freeze and Blind behaviors
    public float GetSaturationValueFreeze()
    {
        return _saturationValueFreeze;
    }

    public Color GetColorFilterValueFreeze()
    {
        return _colorFilterValueFreeze;
    }
    public float GetFrozenPlayerMoveSpeed()
    {
        return _frozenPlayerMoveSpeed;
    }

    public GameObject GetFreezingParticles()
    {
        return _freezingParticles;
    }

    public float GetChangeDurationFreeze()
    {
        return _changeDurationFreeze;
    }

    public LayerMask GetIncludedLayersAtmosphere()
    {
        return _includedLayersAtmosphere;
    }

    public float GetAtmosphereDistance()
    {
        return _atmosphereDistance;
    }

    public float GetFogDensityValueTarget()
    {
        return _fogDensityValueTarget;
    }

    public float GetChangeDurationBlind()
    {
        return _changeDurationBlind;
    }

    #endregion

    #region Light behaviors

    public FlashlightManager GetFlashlightManager()
    {
        return _flashlightManager;
    }

    public float GetFlashlightEnemyMoveSpeedWeak()
    {
        return _flashlightEnemyMoveSpeedWeak;
    }

    public float GetFlashlightEnemyMoveSpeedStrong()
    {
        return _flashlightEnemyMoveSpeedStrong;
    }

    public float GetLampEnemyMoveSpeedWeak()
    {
        return _lampEnemyMoveSpeedWeak;
    }

    public float GetLampEnemyMoveSpeedStrong()
    {
        return _lampEnemyMoveSpeedStrong;
    }

    #endregion

    #region Photo Camera behavior

    public PhotoCameraManager GetPhotoCameraManager()
    {
        return _photoCamera;
    }

    public Renderer GetEnemyObjectRenderer()
    {
        return _enemyRenderer;
    }

    #endregion

    #region Is seen by player behaviors

    public float GetSeenByPlayerEnemyMoveSpeed()
    {
        return _seenByPlayerEnemyMoveSpeed;
    }

    public float GetUnseenByPlayerEnemyMoveSpeed()
    {
        return _unseenByPlayerEnemyMoveSpeed;
    }

    public float GetChangeDurationSeenDmg()
    {
        return _changeDurationSeenDmg;
    }

    public float GetVignetteValueSeenDmg()
    {
        return _vignetteValueSeenDmg;
    }

    public Color GetColorFilterValueSeenDmg()
    {
        return _colorFilterValueSeenDmg;
    }

    #endregion

    #endregion
}
