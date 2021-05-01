using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorCameraFlash : IEnemyBehavior
{
    // Called attributes
    PhotoCameraManager _photoCamera;
    Renderer _enemyRenderer;
    LayerMask _enemyDetectsObj;
    EnemyController _enemyController;
    float _castsHeightOffset = 0.75f;
    Animator _anim;
    NavMeshAgent _agent;
    float _defaultMoveSpeed;

    // Local attributes
    bool _isCollidingWithCamera = false;
    bool _isEnemyPictured = false;
    Vector3 castsPosition;
    RaycastHit hit;

    public BehaviorCameraFlash(AttributeStorage attributes)
    {
        _photoCamera = attributes.GetPhotoCameraManager();
        _enemyRenderer = attributes.GetEnemyObjectRenderer();
        _enemyDetectsObj = attributes.GetEnemyDetectsObj();
        _enemyController = attributes.GetEnemyController();
        _castsHeightOffset = attributes.GetCastsHeightOffset();
        _defaultMoveSpeed = attributes.GetDefaultEnemyMoveSpeed();
        _anim = _enemyController.GetComponent<Animator>();
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
    }

    public void Behavior()
    {
        //Debug.Log(_photoCamera.GetEnemyIsInPicture());
        if (_isCollidingWithCamera && _photoCamera.GetEnemyIsInPicture() && !_isEnemyPictured)
        {
            Debug.Log("Took a picture");
            _isEnemyPictured = true;
            _enemyController.SetShouldDisappear(true);
            _anim.Play("GetHitLight1");
            _agent.speed = 0f;
        }
        else if (_isEnemyPictured && !_photoCamera.GetEnemyIsInPicture())
        {
            //Debug.Log("YES");
            _isEnemyPictured = false;
            _agent.speed = _defaultMoveSpeed;
            //_enemyController.SetShouldDisappear(false);
        }
    }

    public void CallBehavior()
    {
        castsPosition = new Vector3(_enemyController.transform.position.x, _enemyController.transform.position.y + _castsHeightOffset, _enemyController.transform.position.z);
        if (Physics.Linecast(castsPosition, _photoCamera.transform.position, out hit, _enemyDetectsObj, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            //Debug.DrawLine(castsPosition, _photoCamera.transform.position);

            if (_enemyRenderer.isVisible && hit.collider.gameObject.name.Equals("PhotoCamera"))
            {
                _isCollidingWithCamera = true;
                //Debug.Log("Visible");
            }
            else
            {
                _isCollidingWithCamera = false;
                //Debug.Log("NonVisible");
            }
        }
        else
        {
            _isCollidingWithCamera = false;
            //Debug.Log("NonVisible");
        }
    }

    public void DebugFunction()
    {
        Debug.Log("CAMERA behavior WORKS");
    }

    public string BehaviorMessage()
    {
        return "Runs from camera";
    }
}
