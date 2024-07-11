using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BasicWeapon;

interface IMolotovAttack
{
    public void SetInit(float duration, int damage);

    public void NewEndobj(EndObj newevent);
    
}

public class MolotovAttack : MonoBehaviour , IMolotovAttack
{
    int _damage;

    EndObj _endObj;

    public float _duration;
    float _durationTimer = 0;

    private void OnDisable()
    {
        _endObj = null;
        _duration = 0;
    }

    public void NewEndobj(EndObj newevent)
    {
        _endObj = null;
        _endObj += newevent;
    }

    public void SetInit(float duration,int damage)
    {
        _damage = damage;
        _duration = duration;
        StartCoroutine(Attacking());
    }


   

    IEnumerator Attacking()
    {        
        yield return new WaitForSeconds(5);
        _endObj?.Invoke();

    }

    private void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Monster enemy = collision.GetComponent<Monster>();
        if (enemy != null && enemy.CanBeHit(gameObject))

        {
            enemy.OnHit(gameObject, _damage);
        }
    }
}
