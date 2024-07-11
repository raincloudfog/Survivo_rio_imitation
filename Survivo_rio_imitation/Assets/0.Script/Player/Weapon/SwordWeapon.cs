using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ 
/*public class SwordWeapon : Weapon, ISword
{

    [Space(20)]
    //����
    [Header("���� ����"),SerializeField]
    Vector3 _attackCollider = Vector3.zero;
    //�ݶ��̴�
    [SerializeField]
    BoxCollider2D _boxCollider;

    //��ġ
    [Header("�÷��̾� ��ġ"), SerializeField]
    Transform _playerPoint;

    [Header("���� ���� Ƚ��"),SerializeField]
    //��Ʈ�� 
    int count = 0;

    // �Ұ�
    //�ִϸ��̼� ���� �ǰ� �ִ��� 
    private bool _isActive= false;
    Coroutine _nextActive = null;

    //������������
    protected float WeaponDuration
    {
        get
        {
            float Getfloat = Mathf.Clamp(_weaponDuration, _weaponDuration, _weaponCoolTime);    
            return Getfloat;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="weaponCooltime">��Ÿ�� -�ۼ�Ʈ ����</param>
    /// <param name="weaponSpeed">���� ���ǵ� +�ۼ�Ʈ ����</param>
    /// <param name="weaponScale">���� ũ�� +�ۼ�Ʈ ����</param>
    /// <param name="weaponDuration">���� ���ӽð� +�ۼ�Ʈ ����</param>
    /// <param name="_attackCount">���� ���� Ƚ�� �߰�</param>
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

        Debug.Log("����Ƚ��" + _attackCount);
        Vector3 OffsetPos = new Vector3(_attackCollider.x * 0.5f * transform.localScale.x, 0, 0);

        transform.position = _playerPoint.position + OffsetPos;
        _isActive = true;

        while (count <= _attackCount)
        {
            yield return new WaitForEndOfFrame();
            

            //�ִϸ��̼� ���� �ϸ鼭 �ݸ��� ���ֱ� // ���Ѵٸ� �������� ������Ű�� �͵� ��� ����

            _anim.Play("Attack");
            yield return StartCoroutine(WaitForAnimationEvent());
            _boxCollider.enabled = false;

            OffsetPos.x *= -1;
            Debug.Log("OfffsetPos :" + OffsetPos);
            transform.position = _playerPoint.position + OffsetPos;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            Debug.Log("transform.localScale : " + transform.localScale);
            Debug.Log("transform.position : " + transform.position);

            Debug.Log( count++);//�̰� ���ָ� ++������ �������� Ȯ�� ���Ұ�
            Debug.Log("count / _attackCount : " + count + " / " + _attackCount
                + (count < _attackCount) + " count < _attackCount ");
        }
        count = 0;
        _isActive = false;

        Debug.Log(gameObject.name + "�ڷ�ƾ ����");

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
            Debug.Log("�� ����");
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