using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _attackDistance = 2f;
    [SerializeField] Transform _energyGenPos;
    GeneratorManager _energyGen;

    NavMeshAgent _agent;
    Animator _anim;

    [SerializeField] float _castsHeightOffset = 0.75f;
    [SerializeField] LayerMask _includedLayers;
    [SerializeField] Renderer _renderer;
    Vector3 castsPosition;
    RaycastHit hit;

    // For flashlight detection raycasts
    [SerializeField] float _lightDetectionDistance = 10f;
    [SerializeField] LayerMask _lightDetectionLayers;
    bool isLitUp = false;
    //

    bool IsEnemyCollidingWithFlashlight = false;


    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameManager.GetIsGeneratorOn()) 
        {
            _agent.SetDestination(_player.position); // move towards player
        }
        else if (GameManager.GetIsGeneratorOn()) // TODO -> only switch movement to light generator if the monster is afraid of light
        {
            _agent.SetDestination(_energyGenPos.position); // if the lights are on move towards generator
            EnergyGeneratorAction();
        }

        //_agent.SetDestination(_player.position);
        //Debug.DrawLine(_agent.destination, new Vector3(_agent.destination.x, _agent.destination.y + 1f, _agent.destination.z), Color.red);
        castsPosition = new Vector3(transform.position.x, transform.position.y + _castsHeightOffset, transform.position.z);
        //Debug.Log(_agent.remainingDistance);
        PlayAnimations();
        //IsEnemyVisible();
        CollisionWithFlashlight();
    }

    void PlayAnimations()
    {
        if (_agent.remainingDistance < _attackDistance)
        {
            //Debug.Log("CloseEnoughToAttack");
            _anim.SetBool("isAttacking", true);
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

    // function that checks if enemy is 'seen' by the player's camera
    void IsEnemyVisible()
    {
        if (Physics.Linecast(castsPosition, _player.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            //Debug.DrawLine(castsPosition, _player.position);

            if (_renderer.isVisible && hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Visible");
                _agent.speed = 2f;
            }
            else
            {
                //Debug.Log("Hidden");
                _agent.speed = 3f;
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

    void CollisionWithFlashlight()
    {
        if (IsEnemyCollidingWithFlashlight && !isLitUp)
        {
            _anim.Play("GetHitLight1");
            _agent.speed = 0f;
            //_anim.SetBool("isNearFlashlight", true);
            Debug.Log("Hit by flashlight");
            isLitUp = true;
        }
        else if (!IsEnemyCollidingWithFlashlight && isLitUp)
        {
            _agent.speed = 2f;
            //_anim.SetBool("isNearFlashlight", false);
            Debug.Log("No longer hit by flashlight");
            isLitUp = false;
        }

    }

    public bool GetIsEnemyCollidingWithFlashlight()
    {
        return IsEnemyCollidingWithFlashlight;
    }

    public void SetIsEnemyCollidingWithFlashlight(bool value)
    {
        IsEnemyCollidingWithFlashlight = value;
    }
}
