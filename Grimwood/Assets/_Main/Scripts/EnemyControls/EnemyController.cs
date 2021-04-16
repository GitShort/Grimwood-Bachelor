﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _playerHead;
    [SerializeField] float _attackDistance = 2f;
    [SerializeField] Transform _energyGenPos;
    [SerializeField] Transform _photoCamera;
    GeneratorManager _energyGen;

    NavMeshAgent _agent;
    Animator _anim;

    [SerializeField] float _castsHeightOffset = 0.75f;
    [SerializeField] LayerMask _includedLayers;
    [SerializeField] LayerMask _doorIncludedLayers;
    [SerializeField] Renderer _renderer;
    [SerializeField] float _seenByPlayerDistance = 15f;
    Vector3 castsPosition;
    RaycastHit hit;

    bool _isEnemyVisibleToPlayer = false;
    bool _isAppearingEnemyVisibleToPlayer = false;

    bool _shouldDisappear = false;
    bool _didAttack = false;
    bool _isPlayerHit = false;

    //Dictionary<string, IEnemyBehavior> enemyBehaviors;

    bool _goToGenerator = false;
    // For detection if enemy has entered bounds of a light source
    bool _nearLightSource = false;

    bool _nearDoor = false;

    [SerializeField] Camera _playerCamera;

    // for debugging lights
    [SerializeField] GameObject debugCube;

    private void Awake()
    {
        //enemyBehaviors = new Dictionary<string, IEnemyBehavior>();
    }

    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameManager.instance.GetIsPlayerAlive() || GameManager.instance.GetIsPaused())
        {
            //Debug.Log(" Getispaused " + GameManager.instance.GetIsPaused());

            _agent.isStopped = true;
        }

        //Debug.Log(_agent.remainingDistance);
        //_agent.SetDestination(_playerHead.position);
        if (!_goToGenerator)
        {
            _agent.SetDestination(_playerHead.position); // move towards player
            DamagePlayer();
        }
        else if (GameManager.instance.GetIsGeneratorOn() && _goToGenerator)
        {
            _agent.SetDestination(_energyGenPos.position); // if the lights are on move towards generator
            EnergyGeneratorAction();
        }


        //_agent.SetDestination(_player.position);
        //Debug.DrawLine(_agent.destination, new Vector3(_agent.destination.x, _agent.destination.y + 1f, _agent.destination.z), Color.red);
        castsPosition = new Vector3(transform.position.x, transform.position.y + _castsHeightOffset, transform.position.z);
        //Debug.Log(_agent.remainingDistance);
        PlayAnimations();
        IsEnemyVisible();
        DetectClosedDoor();

    }

    void PlayAnimations()
    {
        if (_agent.remainingDistance < _attackDistance && !_agent.pathPending)
        {
            //Debug.Log("CloseEnoughToAttack");
            if (!_didAttack && !_agent.isStopped && GameManager.instance.GetIsPlayerAlive())
            {
                _anim.SetBool("isAttacking", true);
                _didAttack = true;
                AudioManager.instance.Play("EnemyAttack", this.gameObject);
                Invoke("AttackSound", 1f);
            }
            else if (!_didAttack && !GameManager.instance.GetIsPlayerAlive())
            {
                _anim.SetBool("isAttacking", false);
            }
        }
        else
            _anim.SetBool("isAttacking", false);

        if (_agent.velocity != Vector3.zero)
        {
            _anim.SetBool("isRunning", true);
        }
        else
            _anim.SetBool("isRunning", false);
    }

    void AttackSound()
    {
        _didAttack = false;

    }

    void DamagePlayer()
    {
        if (_agent.remainingDistance < _attackDistance && _didAttack && !_isPlayerHit)
        {
            Debug.Log("Player hit");
            _isPlayerHit = true;
            GameManager.instance.SetIsPlayerAlive(false);
        }
    }

    // function that checks if enemy is 'seen' by the player's camera
    bool IsEnemyVisible()
    {
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            //Debug.DrawLine(castsPosition, _player.position);

            if (_renderer.isVisible && hit.collider.gameObject.CompareTag("Player") && hit.distance < _seenByPlayerDistance)
            {
                //Debug.Log("Visible");
                return _isEnemyVisibleToPlayer = true;
            }
            else
            {
                //Debug.Log("Hidden");
                return _isEnemyVisibleToPlayer = false;
            }

        }
        else
            return _isEnemyVisibleToPlayer = false;
    }

    public bool IsSpawningEnemyVisible()
    {
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            Debug.DrawLine(castsPosition, _playerCamera.transform.position);
            Vector3 screenPoint = _playerCamera.WorldToViewportPoint(this.gameObject.transform.position);
            if (screenPoint.z > -1f && screenPoint.x > -1f && screenPoint.x < 2.5f && screenPoint.y > -1f && screenPoint.y < 2.5f && hit.collider.gameObject.CompareTag("Player"))
            {
                return _isAppearingEnemyVisibleToPlayer = true;
            }
            else
            {
                return _isAppearingEnemyVisibleToPlayer = false;
            }
        }
        else
        {
            Vector3 screenPoint = _playerCamera.WorldToViewportPoint(this.gameObject.transform.position);
            if (screenPoint.z > -1f && screenPoint.x > -1f && screenPoint.x < 2.5f && screenPoint.y > -1f && screenPoint.y < 2.5f)
            {
                return _isAppearingEnemyVisibleToPlayer = true;
            }
            else
            {
                return _isAppearingEnemyVisibleToPlayer = false;
            }
        }
    }

    // checks if monster is near the energy generator
    void EnergyGeneratorAction()
    {
        if (Physics.Linecast(castsPosition, _energyGenPos.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            //Debug.DrawLine(castsPosition, _player.position);

            if (hit.collider.gameObject.CompareTag("Generator"))
            {
                _energyGen = hit.collider.GetComponent<GeneratorManager>();
                //hit.collider.gameObject.GetComponent<GeneratorManager>();
                Invoke("HitEnergyGenerator", 1.5f);
            }
        }
    }

    void HitEnergyGenerator()
    {
        _energyGen.SetIsEnemyHittingGenerator(true);
        Debug.Log("Monster has hit the generator!");
    }

    void DetectClosedDoor()
    {
        if (Physics.Raycast(castsPosition, transform.TransformDirection(Vector3.forward), out hit, 1.25f, _doorIncludedLayers))
        {
            Debug.Log(Mathf.Abs(hit.collider.gameObject.transform.parent.rotation.y));
            if (Mathf.Abs(hit.collider.gameObject.transform.parent.rotation.y) <= 0.05f || Mathf.Abs(hit.collider.gameObject.transform.parent.rotation.y) >= 0.67f)
            {
                _agent.isStopped = true;
                _nearDoor = true;
            }
            Debug.DrawRay(castsPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            if(GameManager.instance.GetIsPlayerAlive() && !GameManager.instance.GetIsPaused())
                _agent.isStopped = false;
            _nearDoor = false;
        }
    }

    private void OnDisable()
    {
        _nearLightSource = false;
        _isEnemyVisibleToPlayer = false;
    }

    public bool GetIsEnemyVisibleToPlayer()
    {
        return _isEnemyVisibleToPlayer;
    }

    public void SetShouldDisappear(bool value)
    {
        _shouldDisappear = value;
    }

    public bool GetShouldDisappear()
    {
        return _shouldDisappear;
    }

    public bool GetNearLightSource()
    {
        return _nearLightSource;
    }

    public void SetNearLightSource(bool value)
    {
        _nearLightSource = value;
    }

    public void SetGoToGenerator(bool value)
    {
        _goToGenerator = value;
    }

    public bool GetNearDoor()
    {
        return _nearDoor;
    }
}
