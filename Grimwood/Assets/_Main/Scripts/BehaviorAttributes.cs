using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class BehaviorAttributes
{
    //[SerializeField] EnemyController _enemyContr;

    [field: Header("Freezing Behavior attributes")]
    [SerializeField] float _saturationValueFreeze = -80f;
    [SerializeField] Color _colorFilterValueFreeze;
    [SerializeField] float _frozenMoveSpeed = 2f;
    [SerializeField] GameObject _freezingParticles;
    [SerializeField] float _changeDurationFreeze;
    [SerializeField] float _defaultMoveSpeed = 3f;
    [SerializeField] Volume _ppVolume;

    [SerializeField] Transform _playerHead;
    [SerializeField] LayerMask _includedLayersAtmosphere;
    [SerializeField] float _atmosphereDistance = 8f;

    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] EnemyController _enemy;

    public float GetSaturationValueFreeze()
    {
        return _saturationValueFreeze;
    }

    public Color GetColorFilterValueFreeze()
    {
        return _colorFilterValueFreeze;
    }
    public float GetFrozenMoveSpeed()
    {
        return _frozenMoveSpeed;
    }

    public GameObject GetFreezingParticles()
    {
        return _freezingParticles;
    }

    public float GetChangeDurationFreeze()
    {
        return _changeDurationFreeze;
    }

    public Transform GetPlayerHead()
    {
        return _playerHead;
    }

    public LayerMask GetIncludedLayersAtmosphere()
    {
        return _includedLayersAtmosphere;
    }

    public float GetAtmosphereDistance()
    {
        return _atmosphereDistance;
    }

    public float GetDefaultMoveSpeed()
    {
        return _defaultMoveSpeed;
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
        return _enemy;
    }

}
