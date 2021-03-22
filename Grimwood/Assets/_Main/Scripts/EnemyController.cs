using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _playerHead;
    [SerializeField] float _attackDistance = 2f;
    [SerializeField] Transform _energyGenPos;
    [SerializeField] Transform _photoCamera;
    GeneratorManager _energyGen;

    NavMeshAgent _agent;
    Animator _anim;
    PlayerManager _playerManage;

    [SerializeField] float _defaultMoveSpeed = 3f;

    [SerializeField] float _castsHeightOffset = 0.75f;
    [SerializeField] LayerMask _includedLayers;
    [SerializeField] Renderer _renderer;
    Vector3 castsPosition;
    RaycastHit hit;

    // For flashlight detection raycasts
    [SerializeField] float _flashlightMoveSpeed = 3f;
    bool isLitUp = false;
    //

    // Frost atmosphere emittion 
    [SerializeField] LayerMask _includedLayersFrost;
    [SerializeField] float _FrostDistance = 8f;

    bool _isCollidingWithFlashlight = false;
    bool _isCollidingWithCamera = false;


    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameManager.GetIsGeneratorOn()) 
        {
            _agent.SetDestination(_playerHead.position); // move towards player
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
        IsEnemyVisibleCamera();

        
        //EmitAtmosphere(0); // freezing
        EmitAtmosphere(1); // fog

        //debugging purposes for collision with photo camera
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _isCollidingWithCamera = false;
        }
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
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
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
                _agent.speed = _defaultMoveSpeed;
            }
                
        }
    }

    // function that checks if enemy is 'seen' by the photo camera
    void IsEnemyVisibleCamera()
    {
        if (Physics.Linecast(castsPosition, _photoCamera.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            //Debug.DrawLine(castsPosition, _player.position);

            if (_renderer.isVisible && hit.collider.gameObject.CompareTag("PhotoCamera"))
            {
                CollisionWithCamera();
                Debug.Log("Visible");
            }
        }
    }

    void CollisionWithCamera()
    {
        if (_isCollidingWithCamera)
        {
            Debug.Log("Took a picture");
            _isCollidingWithCamera = false;
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

    // Flashlight behavior
    void CollisionWithFlashlight()
    {
        if (_isCollidingWithFlashlight && !isLitUp)
        {
            _anim.Play("GetHitLight1");
            _agent.speed = _flashlightMoveSpeed;
            //_anim.SetBool("isNearFlashlight", true);
            Debug.Log("Hit by flashlight");
            isLitUp = true;
        }
        else if (!_isCollidingWithFlashlight && isLitUp)
        {
            _agent.speed = _defaultMoveSpeed;
            //_anim.SetBool("isNearFlashlight", false);
            Debug.Log("No longer hit by flashlight");
            isLitUp = false;
        }

    }

    /// <summary>
    /// Frost atmosphere/ Thicker fog atmosphere behavior function
    /// </summary>
    /// <param name="state"> 0 is freezing, 1 is fog </param>
    void EmitAtmosphere(int state)
    {
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayersFrost, QueryTriggerInteraction.Ignore))
        {
            _playerManage = hit.collider.GetComponentInParent<PlayerManager>();
            if (hit.distance <= _FrostDistance)
            {
                if (state == 0)
                {
                    _playerManage.SetIsFreezing(true);
                    Debug.Log("Player has entered FROST aura");
                }
                else if (state == 1)
                {
                    _playerManage.SetIsBlinded(true);
                    Debug.Log("BLINDED");
                }
            }
            else
            {
                if (state == 0)
                {
                    _playerManage.SetIsFreezing(false);
                    Debug.Log("Player has left FROST aura");
                }
                else if (state == 1)
                {
                    _playerManage.SetIsBlinded(false);
                    Debug.Log("NOT BLINDED");
                }

            }
        }
    }


    // 
    public bool GetIsCollidingWithFlashlight()
    {
        return _isCollidingWithFlashlight;
    }

    public void SetIsCollidingWithFlashlight(bool value)
    {
        _isCollidingWithFlashlight = value;
    }

    public bool GetIsCollidingWithCamera()
    {
        return _isCollidingWithCamera;
    }

    public void SetIsCollidingWithCamera(bool value)
    {
        _isCollidingWithCamera = value;
    }
}
