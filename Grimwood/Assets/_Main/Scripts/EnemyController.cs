using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _attackDistance = 2f;
    [SerializeField] Transform _energyGenerator;

    NavMeshAgent _agent;
    Animator _anim;

    [SerializeField] float linecastHeightOffset = 0.75f;
    [SerializeField] LayerMask _includedLayers;
    [SerializeField] Renderer _renderer;
    Vector3 linecastPosition;
    RaycastHit hit;

    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
        
    }

    void Update()
    {
        if (!GameManager.IsGeneratorOn) 
        {
            _agent.SetDestination(_player.position); // move towards player
            IsEnemyVisible();
        }
        else if (GameManager.IsGeneratorOn) // TODO -> only switch movement to light generator if the monster is afraid of light
        {
            _agent.SetDestination(_energyGenerator.position); // if the lights are on move towards generator
            EnergyGeneratorAction();
        }

        //_agent.SetDestination(_player.position);
        Debug.DrawLine(_agent.destination, new Vector3(_agent.destination.x, _agent.destination.y + 1f, _agent.destination.z), Color.red);
        linecastPosition = new Vector3(transform.position.x, transform.position.y + linecastHeightOffset, transform.position.z);
        //Debug.Log(_agent.remainingDistance);
        PlayAnimations();
        //IsEnemyVisible();
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

    // method that checks if enemy is 'seen' by the player's camera
    void IsEnemyVisible()
    {
        if (Physics.Linecast(linecastPosition, _player.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawLine(linecastPosition, _player.position);

            if (_renderer.isVisible && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Visible");
                _agent.speed = 2f;
            }
            else
            {
                Debug.Log("Hidden");
                _agent.speed = 3f;
            }
                
        }
    }

    void EnergyGeneratorAction()
    {
        if (Physics.Linecast(linecastPosition, _energyGenerator.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
        {

            Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.CompareTag("Generator"))
            {
                Invoke("HitEnergyGenerator", 1.5f);
            }
        }
    }

    void HitEnergyGenerator()
    {
        GameManager.IsMonsterHittingGenerator = true;
        Debug.Log("Monster has hit the generator!");
    }
}
