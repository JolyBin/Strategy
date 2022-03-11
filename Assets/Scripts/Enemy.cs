using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}
public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;


    public int Health;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public float AttaclPeriod = 1f;
    public int Damage = 1;

    public NavMeshAgent NavMeshAgent;

    Building _targetBuilding;
    Unit _targetUnit;
    float _timer;
    Healthbar _healthbar;
    int _maxHealth;

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthbar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        _maxHealth = Health;
        SetState(EnemyState.Idle);
        _healthbar = GetComponentInChildren<Healthbar>();
    }

    void Update()
    {
        if(CurrentEnemyState == EnemyState.Idle)
        {
            FindClosesBuilding();
            if(_targetBuilding)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosesUnit();
        }
        else if(CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosesUnit();
            if (!_targetBuilding)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (_targetUnit)
            {
                NavMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if(_targetUnit)
            {
                NavMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttaclPeriod)
                {
                    _targetUnit.TakeDamage(Damage);
                    _timer = 0;
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosesBuilding();
            if (_targetBuilding)
            {
                NavMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosesBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();
        Building closestBuilding = null;

        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }
        _targetBuilding = closestBuilding;
    }

    public void FindClosesUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        Unit closesUnit = null;

        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closesUnit = allUnits[i];
            }
        }
        if(minDistance < DistanceToFollow)
        {
            _targetUnit = closesUnit;
            SetState(EnemyState.WalkToUnit);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
}
