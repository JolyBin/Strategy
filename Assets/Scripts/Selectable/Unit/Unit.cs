using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public int Health;
    public NavMeshAgent NavMeshAgent;
    public GameObject EndPoint;


    int _maxHealth;
    Healthbar _healthbar;

    protected override void Start()
    {
        base.Start();
        _maxHealth = Health;
        _healthbar = GetComponentInChildren<Healthbar>();
        EndPoint.transform.parent = null;
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);
        EndPoint.transform.position = point;
    }



    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthbar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        FindObjectOfType<Management>()?.Unselect(this);
        if(EndPoint)
        {
            Destroy(EndPoint);
        }
    }
}
