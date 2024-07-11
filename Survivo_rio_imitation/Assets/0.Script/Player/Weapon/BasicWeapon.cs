using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나중에 오브젝트 풀을 이용할 시 공격 마무리 후 오브젝트 풀에게 넘겨주기 위함.
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

    

    //공격 범위 확인
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
