using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}
public class Knight : Unit
{
    public UnitState CurrentUnitState;

    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public float AttaclPeriod = 1f;
    public int Damage = 1;

    Enemy _targeEnemy;
    float _timer;

    protected override void Start()
    {
        base.Start();
        SetState(UnitState.Idle);
    }

    void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosesEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
            FindClosesEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (_targeEnemy)
            {
                NavMeshAgent.SetDestination(_targeEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targeEnemy.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (_targeEnemy)
            {
                NavMeshAgent.SetDestination(_targeEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targeEnemy.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > AttaclPeriod)
                {
                    _targeEnemy.TakeDamage(Damage);
                    _timer = 0;
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;
        if (CurrentUnitState == UnitState.Idle)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {

        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosesEnemy()
    {
        Enemy[] allEnemys = FindObjectsOfType<Enemy>();
        Enemy closesEnemy = null;

        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allEnemys.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemys[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closesEnemy = allEnemys[i];
            }
        }

        if(minDistance < DistanceToFollow)
        {
            _targeEnemy = closesEnemy;
            SetState(UnitState.WalkToEnemy);
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
