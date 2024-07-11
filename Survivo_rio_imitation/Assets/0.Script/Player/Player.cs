using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{



    [Header("Components"), Space(5)]
    public PlayerMovement _movement;
    public Rigidbody2D _rigid;
    public VariableJoystick _joystick;
    public SpriteRenderer _spriteRenderer;
    public Animator _anim;

    [Header("Stat"), Space(5)]

    [SerializeField]
    int _speed, _hp,  _exp, _level, _expbar, _maxHp;
    public int _hpRegen;
    private Vector2 moveVec;

    int _Nextexp = 100;

    

    public int LEVEL
    {
        get
        {
            return _level;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayManager.Instance._player = this;
        StartCoroutine(HpRegenRogic());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            AddExp(500);
        }
    }

    public void Init()
    {

    }


    private void FixedUpdate()
    {
        if (PlayManager.Instance.IsGameStop)
        {
            return;
        }

        //1. 인풋
        float x = _joystick.Horizontal;
        float y = _joystick.Vertical;

        //2. 움직임
        moveVec = new Vector3(x, y, 0) * _speed * Time.deltaTime;
        _rigid.MovePosition(_rigid.position + moveVec);

        if (moveVec.sqrMagnitude == 0) return;

        //3. 회전
        if (x > 0)
        {
            _spriteRenderer.flipY = true;
        }
        else
        {
            _spriteRenderer.flipY = false;
        }
    }

    public void MaxHpPlus(float percent)
    {
        int hp = Mathf.FloorToInt(_maxHp * percent);
        _maxHp += hp;
    }

    private IEnumerator HpRegenRogic() {

        while(true)
        {
            yield return new WaitForSeconds(2);
            _hp = (_hp + _hpRegen) >= _maxHp ? _maxHp : _hp + _hpRegen;  
        } 
    }

    public void AddHp() { _hp = (_hp+ 30) > 100 ? 100 : _hp + 30; }   

    

    public void AddExp(int exp)
    {
        _exp += exp;

        ExpObjectOn();

        if(_exp >= _Nextexp)
        {
            StartCoroutine(AddExpCoroutine());
        }
    }

    private IEnumerator AddExpCoroutine()
    {

        while (_exp >= _Nextexp)
        {
            _level++;
            _exp -= _Nextexp;
            _Nextexp = Mathf.FloorToInt(_Nextexp * 1.2f);
            PlayManager.instance.LevelUp();

            // 무기 고르는 코루틴 대기
            yield return StartCoroutine(GetWeapon());
            PlayManager.instance.LevelObjOff();

        }
        ExpObjectOn();
    }


    void ExpObjectOn()
    {
        //현재 경험치가 몇퍼센트인지 계산
        float currentPercentage = (float)_exp / (float)_Nextexp;

        //경험치 바의 총 개수
        int expObjectCount = 10;

        //현재 퍼센트에 맞는 경험치 바의 인덱스를 게산
        int expObjectIndex = Mathf.FloorToInt(currentPercentage * expObjectCount);
        int clampIndex = Mathf.Clamp(expObjectIndex, 0, 10);
        PlayManager.instance.ExpObjectOn(clampIndex);

        /*//경험치 의 10%에 도달했는지 확인
        int threshold = Mathf.FloorToInt(_Nextexp * 0.1f);
        int checkexp = exp;

        //
        while (checkexp >= threshold)
        {
            PlayManager.instance.ExpObjectOn();
            checkexp -= threshold;
        }*/
    }

    IEnumerator AddExpRogic()
    {

        Debug.Log("경험치 코루틴 로직");

        //이건 레벨업시
        while (_exp >= _Nextexp)
        {
            _level++;
            //레벨업 하고 남아있는 경험치 
            _exp -= _Nextexp;
            _Nextexp = Mathf.FloorToInt(_Nextexp * 1.2f);
            //경험치 바 꽉 채워놓고 기다리기
            PlayManager.instance.LevelUp();

            //잠시 무기 고르는 코루틴 기다리고 나서 다시 시작
            yield return StartCoroutine(GetWeapon());
        }
        PlayManager.instance.LevelObjOff();

        ExpObjectOn();
    }

    IEnumerator GetWeapon()
    {


        PlayManager.Instance.IsGameStop = true;
        Time.timeScale = 0;

        while(PlayManager.Instance.IsGameStop == true)
        {
            yield return null;
        }

        PlayManager.Instance.IsGameStop = false;
        Time.timeScale = 1;        

    }

    private void LateUpdate()
    {
        
    }
}
