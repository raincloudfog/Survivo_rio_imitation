using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//구버전 
/*public abstract class Weapon : MonoBehaviour
{
    public Sprite[] _weaponImg;
    public Animator _anim;

    [Header("기본 설정 값")]

    [SerializeField]
    protected Vector3 _originWeaponScale;

    [SerializeField]
    protected float _originWeaponSpeed,_originWeaponDuration, _originWeaponCoolTime;
    [SerializeField]
    protected int _originAttackCount , _originDamage;

    protected float _weaponSpeed,  _weaponDuration ;
    protected int _attackCount, _damage;
    protected Vector3 _weaponScale;

    public float _cooltimer, _weaponCoolTime;

    protected Player _player;

    abstract public void Init(Player player);

    abstract public void ActiveWeapon(float weaponCooltime, float weaponSpeed, float weaponScale, float weaponDuration, float Damage, int attackCoun);

    public virtual void Awake()
    {
        //변수 값 설정
        _weaponSpeed = _originWeaponSpeed;
        _weaponScale = _originWeaponScale;
        _weaponDuration = _originWeaponDuration;
        _weaponCoolTime = _originWeaponCoolTime;
        _attackCount = _originAttackCount;
        _damage = _originDamage;
    }

}
*/

[System.Serializable]
public class Weapon
{
    public float _originWeaponSpeed, _originWeaponDuration, _originWeaponCoolTime;
    public int _originAttackCount, _originDamage;
    public Vector3  _originWeaponScale;

    public void Init(float speed , float duration ,float coolTime, int count , int damage, Vector3 scale)
    {
        _originWeaponSpeed = speed;
        _originWeaponDuration = duration;
        _originWeaponCoolTime = coolTime;
        _originAttackCount = count;
        _originDamage = damage;
        _originWeaponScale = scale;
    }

}