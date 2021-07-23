using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private ParticleSystem projectileParticle;
    [SerializeField] private float range = 15f;
    private Transform _target;
    

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget()
    {
        var enemies = FindObjectsOfType<Enemy>();
        var minDistance = Mathf.Infinity;
        Transform closestTarget = null;
        foreach (var enemy in enemies)
        {
            var targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < minDistance)
            {
                closestTarget = enemy.transform;
                minDistance = targetDistance;
            }
        }

        _target = closestTarget;
    }
    private void AimWeapon()
    {
        if (ReferenceEquals(_target, null))
            return;
        weapon.LookAt(_target);
        var targetDistance = Vector3.Distance(transform.position, _target.position);
        if(targetDistance < range) Attack(true);
        else Attack(false);
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticle.emission;
        emissionModule.enabled = isActive;
    }
}
