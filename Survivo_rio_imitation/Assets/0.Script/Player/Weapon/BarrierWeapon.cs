using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Iberrier
{
    public void FollowPlayer(Transform point);
}

public class BarrierWeapon : BasicWeapon, Iberrier
{
    Transform _point = null;

    public void FollowPlayer(Transform point)
    {
        _point = point;
    }

    public void LateUpdate()
    {
        if( _point != null )
        {
            transform.position = _point.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Monster enemy = collision.GetComponent<Monster>();
            if (enemy != null && enemy.CanBeHit(gameObject))
            {
                enemy.OnHit(gameObject,_damage);
            }
        }
    }
}
