using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface IMolotov_cocktail
{
    

    public void SetTargetPosition(Vector3 position);

    public void MovetoTargetPosition(float duration);

    public void Init(GameObject obj, EndObj _event);

}

public class Molotov_cocktail : BasicWeapon, IMolotov_cocktail
{
    public float _rotationRadius = 5.0f;
    public float _rotationSpeed = 1.0f;
    public float _moveSpeed = 5.0f;
    public float _attackCooldown = 2.0f;
    private float cooldownTimer;
    private Vector3 _targetPosition;

    private float _attackScale;

    private float _duration;

    GameObject _molotovAttack;

    

    private void Start()
    {

    }

    public void OnEnable()
    {
        //생성되었을때 안한이유 위치가 아직 안정해져있을 거라 판단.
        //MovetoTargetPosition();
    }

    public void OnDisable()
    {
        StopCoroutine(MoveOverTime(transform.position, _targetPosition));
    }

    

    //부르는 순서 2 위치 파악했으니 그쪽으로 이동
    public void MovetoTargetPosition(float duration)
    {        
        _duration = duration;
        StartCoroutine(MoveOverTime(transform.position, _targetPosition));
    }   

    //부르는 순서 1 먼저 떨어질 위치 탐색
    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    private IEnumerator MoveOverTime(Vector3 start, Vector3 end)
    {        
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime * _moveSpeed;
            yield return null;
        }

        transform.position = end;
        GameObject obj = _molotovAttack;
        obj.SetActive(true);
        obj.transform.position = transform.position;
        IMolotovAttack Imolotov = obj.GetComponent<IMolotovAttack>();        

        Imolotov.SetInit(_duration, _damage);

        _endObj?.Invoke();
        gameObject.SetActive(false);
    }

    public void Init(GameObject obj, EndObj _event)
    {
        _molotovAttack = obj;
        IMolotovAttack Imolotov = obj.GetComponent<IMolotovAttack>();
        Imolotov.NewEndobj(_event);
    }
}
