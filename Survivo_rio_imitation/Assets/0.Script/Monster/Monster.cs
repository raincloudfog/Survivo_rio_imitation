using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] int _damage, _hp;
    [SerializeField] float _moveSpeed;

    [SerializeField] Player _testPlayer;

    // ���⺰ ������ �ǰ� �ð�
    private Dictionary<GameObject, float> _lastHitTimes = new Dictionary<GameObject, float>();

    [SerializeField] float _hitDelay = 0.5f; // �ǰ� ������

    //���� �� �̺�Ʈ
    EndObj _endobj = null;

    //���⿡ ���� �ǰ� ���� ���� Ȯ��
    public bool CanBeHit(GameObject weapon)
    {

        //�ش� Ű�� ������ �߰��ϰ� �ʱ�ȭ // ��ųʸ��� add�� ���� ���ص� �߰��ȴٰ� �ϴµ� Ȯ�� ��
        if (!_lastHitTimes.ContainsKey(weapon))
        {
            _lastHitTimes[weapon] = -Mathf.Infinity;
        }
        //���� �ð��� ������ �ǰ� �ð� + �ǰ� �����̺��� ũ�ų� ������ Ȯ��
        return Time.time >= _lastHitTimes[weapon] + _hitDelay;
        //Time.time�� ������� ���� �þ�� �˾ƺ���

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
