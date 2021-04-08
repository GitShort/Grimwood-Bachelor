using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoints : MonoBehaviour
{
    [SerializeField] EnemyController _enemyController;
    [SerializeField] float[] _TimesBetweenRespawn;
    bool _isDisappeared = false;

    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    float _timerSpeed = 2f;

    // Spawn zones
    [SerializeField] BoxCollider _SpawnPoints;
    Vector3 _SpawnPointsSize;
    Vector3 _SpawnPointsCenter;

    private void Awake()
    {
        _SpawnPointsCenter = _SpawnPoints.transform.position;

        _SpawnPointsSize.x = _SpawnPoints.transform.localScale.x * _SpawnPoints.size.x;
        _SpawnPointsSize.z = _SpawnPoints.transform.localScale.z * _SpawnPoints.size.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        _isTimerStarted = false;
        _isTimerFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController.GetShouldDisappear() && _enemyController.gameObject.activeInHierarchy && !_isDisappeared)
        {
            Debug.Log("Poof!");
            _isTimerStarted = false;
            _isTimerFinished = false;
            _isDisappeared = true;
            Invoke("EnemyDisappear", 0.3f);
        }
        if (!_enemyController.GetShouldDisappear() && !_enemyController.gameObject.activeInHierarchy)
        {
            _enemyController.gameObject.SetActive(true);
            _isTimerStarted = false;
            _isTimerFinished = false;
            _isDisappeared = false;
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _enemyController.SetShouldDisappear(true);
        }

        EnemyRespawnTimer(0);
    }

    void EnemyDisappear()
    {
        _enemyController.gameObject.SetActive(false);
        _enemyController.transform.position = GetRandomPosition();
        Debug.Log(GetRandomPosition());
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
                //Debug.Log(_timer);
                _timer += Time.deltaTime;
                if (_TimesBetweenRespawn[index] <= _timer)
                {
                    _enemyController.SetShouldDisappear(false);
                    _isTimerFinished = true;
                }
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-_SpawnPointsSize.x / 2, _SpawnPointsSize.x / 2),
            _SpawnPointsCenter.y,
            Random.Range(-_SpawnPointsSize.z / 2, _SpawnPointsSize.z / 2));

        return new Vector3(_SpawnPointsCenter.x + randomPos.x, randomPos.y, _SpawnPointsCenter.z + randomPos.z);
    }
}
