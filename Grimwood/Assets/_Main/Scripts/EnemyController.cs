using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _attackDistance = 2f;

    NavMeshAgent _agent;
    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_player.position);
        Debug.Log(_agent.remainingDistance);
        if (_agent.remainingDistance < _attackDistance)
        {
            Debug.Log("CloseEnoughToAttack");
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
}
