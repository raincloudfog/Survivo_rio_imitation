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
        //�����Ǿ����� �������� ��ġ�� ���� ������������ �Ŷ� �Ǵ�.
        //MovetoTargetPosition();
    }

    public void OnDisable()
    {
        StopCoroutine(MoveOverTime(transform.position, _targetPosition));        
    }

    //�θ��� ���� 2 ��ġ �ľ������� �������� �̵�
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


    //�θ��� ���� 1 ���� ������ ��ġ Ž��
    public void SetTargetPosition(Vector3 position)
    {
        float radius = 2.0f; //������
        float randomAngle = Random.Range(0, 2 * Mathf.PI); // 0���� 360�� ������ ���� ����
        float randomDistance = Random.Range(-radius, radius); // ���� ���� ���� �Ÿ�

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

            //���� ȸ�� // 2024 07 06
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
