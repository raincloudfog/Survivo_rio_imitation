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

        //1. ��ǲ
        float x = _joystick.Horizontal;
        float y = _joystick.Vertical;

        //2. ������
        moveVec = new Vector3(x, y, 0) * _speed * Time.deltaTime;
        _rigid.MovePosition(_rigid.position + moveVec);

        if (moveVec.sqrMagnitude == 0) return;

        //3. ȸ��
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

            // ���� ���� �ڷ�ƾ ���
            yield return StartCoroutine(GetWeapon());
            PlayManager.instance.LevelObjOff();

        }
        ExpObjectOn();
    }


    void ExpObjectOn()
    {
        //���� ����ġ�� ���ۼ�Ʈ���� ���
        float currentPercentage = (float)_exp / (float)_Nextexp;

        //����ġ ���� �� ����
        int expObjectCount = 10;

        //���� �ۼ�Ʈ�� �´� ����ġ ���� �ε����� �Ի�
        int expObjectIndex = Mathf.FloorToInt(currentPercentage * expObjectCount);
        int clampIndex = Mathf.Clamp(expObjectIndex, 0, 10);
        PlayManager.instance.ExpObjectOn(clampIndex);

        /*//����ġ �� 10%�� �����ߴ��� Ȯ��
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

        Debug.Log("����ġ �ڷ�ƾ ����");

        //�̰� ��������
        while (_exp >= _Nextexp)
        {
            _level++;
            //������ �ϰ� �����ִ� ����ġ 
            _exp -= _Nextexp;
            _Nextexp = Mathf.FloorToInt(_Nextexp * 1.2f);
            //����ġ �� �� ä������ ��ٸ���
            PlayManager.instance.LevelUp();

            //��� ���� ���� �ڷ�ƾ ��ٸ��� ���� �ٽ� ����
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
