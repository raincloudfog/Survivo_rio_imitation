using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] int _damage, _hp;
    [SerializeField] float _moveSpeed;

    [SerializeField] Player _testPlayer;

    // 무기별 마지막 피격 시간
    private Dictionary<GameObject, float> _lastHitTimes = new Dictionary<GameObject, float>();

    [SerializeField] float _hitDelay = 0.5f; // 피격 딜레이

    //죽은 후 이벤트
    EndObj _endobj = null;

    //무기에 대해 피격 가능 여부 확인
    public bool CanBeHit(GameObject weapon)
    {

        //해당 키가 없으면 추가하고 초기화 // 딕셔너리는 add를 따로 안해도 추가된다고 하는데 확인 중
        if (!_lastHitTimes.ContainsKey(weapon))
        {
            _lastHitTimes[weapon] = -Mathf.Infinity;
        }
        //현재 시간이 마지막 피격 시간 + 피격 딜레이보다 크거나 같은지 확인
        return Time.time >= _lastHitTimes[weapon] + _hitDelay;
        //Time.time이 어느정도 까지 늘어날지 알아볼것

    }

    public void OnHit(GameObject weapon , int damage)
    {
        _lastHitTimes[weapon] = Time.time;
        _hp -= damage;
        if(_hp <= 0)
        {
            _endobj?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void OnHit(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            _endobj?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if(PlayManager.Instance.IsGameStop)
        {
            return;
        }

        Vector3 dir = (PlayManager.Instance._player.transform.position - transform.position).normalized;
        transform.position = transform.position + (dir * _moveSpeed * Time.deltaTime);

        // Increase the range to ensure monsters don't disappear too quickly
        float playermonsterRange = PlayManager.Instance.Screenheight() * 3;
        float sqrRange = playermonsterRange * playermonsterRange;

        if (Vector3.SqrMagnitude(PlayManager.Instance._player.transform.position - transform.position) >= sqrRange)
        {
            ClearMonster();
        }
    }

    public void newEndObj(EndObj newevent)
    {
        _endobj = null;
        _endobj += newevent;

    }

    public void ClearMonster()
    {
        _endobj?.Invoke();
    }
}
