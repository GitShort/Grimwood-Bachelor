using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoints : MonoBehaviour
{
    [SerializeField] EnemyController _enemyController;
    [SerializeField] float[] _TimesBetweenRespawn;

    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    float _timerSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _isTimerStarted = false;
        _isTimerFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController.GetShouldDisappear() && _enemyController.gameObject.activeInHierarchy)
        {
            Invoke("EnemyDisappear", 0.3f);
            Debug.Log("Poof!");
            _isTimerStarted = false;
            _isTimerFinished = false;
        }
        if (!_enemyController.GetShouldDisappear() && !_enemyController.gameObject.activeInHierarchy)
        {
            _enemyController.gameObject.SetActive(true);
            _isTimerStarted = false;
            _isTimerFinished = false;
        }


        EnemyRespawnTimer(0);
    }

    void EnemyDisappear()
    {
        _enemyController.gameObject.SetActive(false);
    }

    void EnemyRespawnTimer(int index)
    {
        if (!_enemyController.gameObject.activeInHierarchy)
        {
            if (!_isTimerStarted && !_isTimerFinished)
            {
                _timer = 0f;
                _isTimerStarted = true;
            }
            if (_isTimerStarted && !_isTimerFinished)
            {
                Debug.Log(_timer);
                _timer += Time.deltaTime;
                if (_TimesBetweenRespawn[index] <= _timer)
                {
                    _enemyController.SetShouldDisappear(false);
                    _isTimerFinished = true;
                }
            }
        }
    }
}
