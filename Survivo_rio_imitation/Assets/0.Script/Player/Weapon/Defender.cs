using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IDefender
{
    public void DefenderWeapon();
}

public class Defender : BasicWeapon
{

    float _timer = 0;
    [SerializeField]
    float _attackDelay = 0.5f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        //���͸��� �ǰ� �����̸� �ֱ� Ȥ�� ���Ⱑ �ѹ����� �������ϱ�...

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

/*
 ����
 private Dictionary<GameObject, float> _lastHitTimes = new Dictionary<GameObject, float>(); // ���⺰ ������ �ǰ� �ð�
    public float hitDelay = 0.5f; // �ǰ� ������

    // ���⿡ ���� �ǰ� ���� ���� Ȯ��
    public bool CanBeHit(GameObject weapon)
    {
        // �ش� Ű�� ��ųʸ��� ������ �߰��ϰ� �ʱ�ȭ
        if (!_lastHitTimes.ContainsKey(weapon))
        {
            _lastHitTimes[weapon] = -Mathf.Infinity;
        }
        // ���� �ð��� ������ �ǰ� �ð� + �ǰ� �����̺��� ũ�ų� ������ Ȯ��
        return Time.time >= _lastHitTimes[weapon] + hitDelay;
    }

    // ���⿡ ���� �ǰ� ó��
    public void OnHit(GameObject weapon)
    {
        _lastHitTimes[weapon] = Time.time;
        // ���⿡ ������ ó�� �ڵ带 �߰��ϼ���
        Debug.Log("Enemy hit by " + weapon.name);
    }


���� 
Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemy.CanBeHit(gameObject))
            {
                // ���͸� ������ �� ���� ���� ����
                enemy.OnHit(gameObject);
                // �߰��� ���� ���� ó�� �ڵ带 ���⿡ �߰��ϼ���
                Debug.Log("Hit enemy with " + gameObject.name);
 */