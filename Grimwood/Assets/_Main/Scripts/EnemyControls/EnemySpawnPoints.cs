using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoints : MonoBehaviour
{
    [SerializeField] EnemyController _enemyController;
    [SerializeField] float[] _AppearedIntervals;
    [SerializeField] float[] _DisappearedIntervals;
    bool _isDisappeared = false;

    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    // Spawn zones
    [SerializeField] BoxCollider[] _SpawnPoints;
    Vector3[] _SpawnPointsSize;
    Vector3[] _SpawnPointsCenter;

    static readonly System.Random rnd = new System.Random();
    int _chosenSpawnPoint;

    private void Awake()
    {
        _SpawnPointsSize = new Vector3[_SpawnPoints.Length];
        _SpawnPointsCenter = new Vector3[_SpawnPoints.Length];

        for (int i = 0; i < _SpawnPoints.Length; i++)
        {
            _SpawnPointsCenter[i] = _SpawnPoints[i].transform.position;
            _SpawnPointsSize[i].x = _SpawnPoints[i].transform.localScale.x * _SpawnPoints[i].size.x;
            _SpawnPointsSize[i].z = _SpawnPoints[i].transform.localScale.z * _SpawnPoints[i].size.z;
        }
    }

    void Start()
    {
        _isTimerStarted = false;
        _isTimerFinished = false;
    }

    void Update()
    {
        //Debug.Log(_enemyController.GetIsEnemyVisibleToPlayer());
        if (_enemyController.GetShouldDisappear() && _enemyController.gameObject.activeInHierarchy && !_isDisappeared && !_enemyController.GetIsEnemyVisibleToPlayer())
        {
            //Debug.Log("Poof!");
            _isTimerStarted = false;
            _isTimerFinished = false;
            _isDisappeared = true;
            Invoke("EnemyDisappear", 0.3f);
        }
        if (!_enemyController.GetShouldDisappear() && !_enemyController.gameObject.activeInHierarchy && !_enemyController.IsSpawningEnemyVisible())
        {
            _enemyController.gameObject.SetActive(true);
            _isTimerStarted = false;
            _isTimerFinished = false;
            _isDisappeared = false;
        }

        // if true - disappear, if false - appear
        if (!_enemyController.gameObject.activeInHierarchy && !_isTimerFinished)
        {
            EnemyRespawnTimer(_DisappearedIntervals, false);
            //Debug.Log("Appearance timer started");
        }
        if (_enemyController.gameObject.activeInHierarchy && !_isTimerFinished)
        {
            EnemyRespawnTimer(_AppearedIntervals, true);
            //Debug.Log("Disappearance timer started");
        }

        // DEBUGGING
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _enemyController.SetShouldDisappear(true);
        }

        //Debug.Log(_enemyController.IsSpawningEnemyVisible());
    }

    void EnemyDisappear()
    {
        _enemyController.gameObject.SetActive(false);
        _chosenSpawnPoint = rnd.Next(_SpawnPoints.Length);
        //Debug.Log(_chosenSpawnPoint);
        _enemyController.transform.position = GetRandomPosition(_chosenSpawnPoint);
    }

    void EnemyRespawnTimer(float[] Interval, bool appearanceValue)
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
            Debug.Log(Interval[GameManager.instance.GetArtifactCollectedCount()].ToString());
            if (Interval[GameManager.instance.GetArtifactCollectedCount()] <= _timer)
            {
                _enemyController.SetShouldDisappear(appearanceValue);
                _isTimerFinished = true;
            }
        }
    }

    Vector3 GetRandomPosition(int index)
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-_SpawnPointsSize[index].x / 2, _SpawnPointsSize[index].x / 2),
            _SpawnPointsCenter[index].y,
            Random.Range(-_SpawnPointsSize[index].z / 2, _SpawnPointsSize[index].z / 2));

        return new Vector3(_SpawnPointsCenter[index].x + randomPos.x, randomPos.y, _SpawnPointsCenter[index].z + randomPos.z);
    }
}
