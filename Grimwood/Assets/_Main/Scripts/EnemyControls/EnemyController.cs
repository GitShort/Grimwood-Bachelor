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

    [SerializeField] float _castsHeightOffset = 0.75f;
    [SerializeField] LayerMask _includedLayers;
    [SerializeField] Renderer _renderer;
    [SerializeField] float _seenByPlayerDistance = 15f;
    Vector3 castsPosition;
    RaycastHit hit;

    bool _isEnemyVisibleToPlayer = false;

    bool _shouldDisappear = false;

    Dictionary<string, IEnemyBehavior> enemyBehaviors;

    bool _goToGenerator = false;
    // For detection if enemy has entered bounds of a light source
    bool _nearLightSource = false;


    [SerializeField] Camera _playerCamera;

    // for debugging lights
    [SerializeField] GameObject debugCube;

    private void Awake()
    {
        enemyBehaviors = new Dictionary<string, IEnemyBehavior>();
    }

    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        //_agent.SetDestination(_playerHead.position);
        if (!_goToGenerator)
        {
            _agent.SetDestination(_playerHead.position); // move towards player
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


        foreach (string key in enemyBehaviors.Keys)
        {
            enemyBehaviors[key].CallBehavior();
            enemyBehaviors[key].Behavior();
        }

        // for light debugging
        if (_nearLightSource)
        {
            debugCube.SetActive(true);
        }
        else if (!_nearLightSource)
        {
            debugCube.SetActive(false);
        }

    }

    public void AddBehavior(IEnemyBehavior behavior, string key)
    {
        enemyBehaviors.Add(key, behavior);
        behavior.DebugFunction();
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
    public bool IsEnemyVisible()
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
        castsPosition = new Vector3(transform.position.x, transform.position.y + _castsHeightOffset, transform.position.z);
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            Debug.DrawLine(castsPosition, _playerCamera.transform.position);
            Vector3 screenPoint = _playerCamera.WorldToViewportPoint(this.gameObject.transform.position);
            if (screenPoint.z > -1f && screenPoint.x > -1f && screenPoint.x < 2.5f && screenPoint.y > -1f && screenPoint.y < 2.5f && hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Vector3 screenPoint = _playerCamera.WorldToViewportPoint(this.gameObject.transform.position);
            if (screenPoint.z > -1f && screenPoint.x > -1f && screenPoint.x < 2.5f && screenPoint.y > -1f && screenPoint.y < 2.5f)
            {
                return true;
            }
            else
            {
                return false;
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

    private void OnDisable()
    {
        _nearLightSource = false;
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
}
