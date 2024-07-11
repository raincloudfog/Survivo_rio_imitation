using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//구버전 
/*public class SwordWeapon : Weapon, ISword
{

    [Space(20)]
    //벡터
    [Header("공격 범위"),SerializeField]
    Vector3 _attackCollider = Vector3.zero;
    //콜라이더
    [SerializeField]
    BoxCollider2D _boxCollider;

    //위치
    [Header("플레이어 위치"), SerializeField]
    Transform _playerPoint;

    [Header("현재 공격 횟수"),SerializeField]
    //인트값 
    int count = 0;

    // 불값
    //애니메이션 실행 되고 있는지 
    private bool _isActive= false;
    Coroutine _nextActive = null;

    //원본수정변수
    protected float WeaponDuration
    {
        get
        {
            float Getfloat = Mathf.Clamp(_weaponDuration, _weaponDuration, _weaponCoolTime);    
            return Getfloat;
        }
    }

    /// <summary>
    /// 공격 실행
    /// </summary>
    /// <param name="weaponCooltime">쿨타임 -퍼센트 감소</param>
    /// <param name="weaponSpeed">무기 스피드 +퍼센트 증가</param>
    /// <param name="weaponScale">무기 크기 +퍼센트 증가</param>
    /// <param name="weaponDuration">무기 지속시간 +퍼센트 증가</param>
    /// <param name="_attackCount">무기 공격 횟수 추가</param>
    public override void ActiveWeapon(float weaponCooltime, float weaponSpeed, float weaponScale, float weaponDuration,float Damage ,int attackCount)
    {
        if(_isActive)
        {
            return;
        }

        _weaponDuration = _originWeaponDuration + (_originWeaponDuration * weaponDuration);
        _weaponCoolTime = _originWeaponCoolTime + (_originWeaponCoolTime * weaponCooltime) ;
        _weaponScale = _originWeaponScale  + (_originWeaponScale * weaponScale);
        _weaponSpeed = _originWeaponSpeed + (_originWeaponSpeed * weaponSpeed);
        _damage = _originDamage + (int)(_originDamage * Damage);
        _attackCount = _originAttackCount + attackCount;

        transform.localScale = _weaponScale;
        _anim.speed = _weaponSpeed;
        StartCoroutine(Attack(_attackCount));
        _cooltimer = 0;
        

    }

    

    public override void Init(Player player)
    {
        _player = player;
    }

    public void WeaponMovement(Vector3 playerVec)
    {
        
    }

    IEnumerator Attack(  int _attackCount)
    {
        //yield return !_isActive;

        Debug.Log("공격횟수" + _attackCount);
        Vector3 OffsetPos = new Vector3(_attackCollider.x * 0.5f * transform.localScale.x, 0, 0);

        transform.position = _playerPoint.position + OffsetPos;
        _isActive = true;

        while (count <= _attackCount)
        {
            yield return new WaitForEndOfFrame();
            

            //애니메이션 실행 하면서 콜리더 켜주기 // 원한다면 스케일을 증가시키는 것도 고려 가능

            _anim.Play("Attack");
            yield return StartCoroutine(WaitForAnimationEvent());
            _boxCollider.enabled = false;

            OffsetPos.x *= -1;
            Debug.Log("OfffsetPos :" + OffsetPos);
            transform.position = _playerPoint.position + OffsetPos;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            Debug.Log("transform.localScale : " + transform.localScale);
            Debug.Log("transform.position : " + transform.position);

            Debug.Log( count++);//이건 없애면 ++연산자 없어지니 확인 잘할것
            Debug.Log("count / _attackCount : " + count + " / " + _attackCount
                + (count < _attackCount) + " count < _attackCount ");
        }
        count = 0;
        _isActive = false;

        Debug.Log(gameObject.name + "코루틴 끝남");

    }

    IEnumerator WaitForAnimationEvent()
    {
        while (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)//_isActive
        {
            Debug.Log("_anim.GetCurrentAnimatorStateInfo(0).IsName(\"Idle\") : " + _anim.GetCurrentAnimatorStateInfo(0).IsName("idle"));
            yield return null;
        }
    }

    

    public void AnimationOnCollider()
    {
        _boxCollider.enabled = true;

    }
    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("적 공격");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 WeaponStartPoint = _attackCollider * 0.5f;
        transform.localScale = WeaponStartPoint;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_playerPoint.position,Vector3.one * 0.5f);
        Gizmos.color = Color.red;
        Vector3 testVec = new Vector3(_attackCollider.x, _attackCollider.y * 0.5f,0);
        Gizmos.DrawCube(_playerPoint.position + new Vector3(_attackCollider.x * 0.5f, 0,0), testVec);
    }
}
*/

interface IWeapon
{
    public void WeaponSettingInit(int Damage, float speed);

    public Animator GetAnim();

    public void NewEndobj(EndObj newevent);

}

interface ISword
{
    public void FollowPlayer(Transform point, Vector3 offset);
}

public class SwordWeapon: BasicWeapon, ISword
{
    Transform playerpoint = null;
    Vector3 offset = Vector3.zero;

    public void FollowPlayer(Transform point, Vector3 offset)
    {
        playerpoint = point;
        this.offset = offset;
    }

    private void LateUpdate()
    {
       if(playerpoint != null)
        {
            transform.position = playerpoint.position + offset;
        }
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            Monster enemy = collision.GetComponent<Monster>();
            if (enemy != null)
            {                
                enemy.OnHit(_damage);
            }
        }
        
    }



}