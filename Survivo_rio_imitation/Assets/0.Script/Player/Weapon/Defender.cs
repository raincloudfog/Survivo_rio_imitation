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
        //몬스터마다 피격 딜레이를 주기 혹은 무기가 한번씩만 때리게하기...

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
 몬스터
 private Dictionary<GameObject, float> _lastHitTimes = new Dictionary<GameObject, float>(); // 무기별 마지막 피격 시간
    public float hitDelay = 0.5f; // 피격 딜레이

    // 무기에 대해 피격 가능 여부 확인
    public bool CanBeHit(GameObject weapon)
    {
        // 해당 키가 딕셔너리에 없으면 추가하고 초기화
        if (!_lastHitTimes.ContainsKey(weapon))
        {
            _lastHitTimes[weapon] = -Mathf.Infinity;
        }
        // 현재 시간이 마지막 피격 시간 + 피격 딜레이보다 크거나 같은지 확인
        return Time.time >= _lastHitTimes[weapon] + hitDelay;
    }

    // 무기에 대해 피격 처리
    public void OnHit(GameObject weapon)
    {
        _lastHitTimes[weapon] = Time.time;
        // 여기에 데미지 처리 코드를 추가하세요
        Debug.Log("Enemy hit by " + weapon.name);
    }


무기 
Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemy.CanBeHit(gameObject))
            {
                // 몬스터를 공격할 수 있을 때만 공격
                enemy.OnHit(gameObject);
                // 추가로 무기 공격 처리 코드를 여기에 추가하세요
                Debug.Log("Hit enemy with " + gameObject.name);
 */