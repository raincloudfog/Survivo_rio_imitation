using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRocket_Launcher
{     
    public Vector3 SetRandomTargetPosition(Vector3 playerpos);

    public void SetTargetPosition(Vector3 position);

    public void MovetoTargetPosition();

}

public class Multiple_Rocket_Launcher_Weapon : BasicWeapon , IRocket_Launcher
{
    public float _rotationRadius = 5.0f;
    public float _rotationSpeed = 1.0f;
    public float _moveSpeed = 5.0f;
    public float _attackCooldown = 2.0f;
    private float cooldownTimer;
    private Vector3 _targetPosition;

    private float _attackScale;

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
    public void MovetoTargetPosition()
    {
        StartCoroutine(MoveOverTime(transform.position, _targetPosition));
    }

    public Vector3 SetRandomTargetPosition(Vector3 playerpos)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 randpos = playerpos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _rotationRadius;
        return randpos;
    }


    //부르는 순서 1 먼저 떨어질 위치 탐색
    public void SetTargetPosition(Vector3 position)
    {
        float radius = 2.0f; //반지름
        float randomAngle = Random.Range(0, 2 * Mathf.PI); // 0에서 360도 사이의 랜덤 각도
        float randomDistance = Random.Range(-radius, radius); // 지름 내의 랜덤 거리

        float offsetX = Mathf.Cos(randomAngle) * randomDistance;
        float offsetY = Mathf.Sin(randomAngle) * randomDistance;

        _targetPosition = position + new Vector3(offsetX, offsetY, 0);

    }

    private IEnumerator MoveOverTime(Vector3 start , Vector3 end)
    {
        float elapsedTime = 0;

        while(elapsedTime< 1)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime *_moveSpeed;

            //방향 회전 // 2024 07 06
            Vector3 direction = end - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));

            yield return null;
        }

        transform.position = end;

        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, _attackScale);

        foreach (Collider2D  enemy in enemys)
        {
            Monster monster = enemy.GetComponent<Monster>();
            if(monster != null)
            {
                monster.OnHit(_damage);
            }
        }

        _endObj?.Invoke();
        gameObject.SetActive(false);
    }

}
