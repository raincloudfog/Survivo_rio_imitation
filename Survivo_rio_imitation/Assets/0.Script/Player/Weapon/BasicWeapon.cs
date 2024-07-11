using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߿� ������Ʈ Ǯ�� �̿��� �� ���� ������ �� ������Ʈ Ǯ���� �Ѱ��ֱ� ����.
public delegate void EndObj();

public class BasicWeapon : MonoBehaviour, IWeapon
{
    public Transform _playerPoint;
    public Animator _anim;
    Vector3 _attackCollider = Vector3.zero;

    protected int _damage;
    protected float _speed;

    

    public EndObj _endObj;

    public virtual void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            _anim.Play("Attack");
        }*/
    }

    public void WeaponSettingInit(int Damage, float speed)
    {
        _damage = Damage;

    }

    

    //���� ���� Ȯ��
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_playerPoint.position, Vector3.one * 0.5f);
        Gizmos.color = Color.red;
        Vector3 testVec = new Vector3(_attackCollider.x, _attackCollider.y * 0.5f, 0);
        Gizmos.DrawCube(_playerPoint.position + new Vector3(_attackCollider.x * 0.5f, 0, 0), testVec);
    }*/

    public Animator GetAnim()
    {
        return _anim;
    }

    public void NewEndobj(EndObj newevent)
    {
        _endObj = null;
        _endObj += newevent;
    }
}
