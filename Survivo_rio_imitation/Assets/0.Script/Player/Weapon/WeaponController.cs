
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public enum eWeaponType
{
    Sword,
    Tutelar,
    Drill,
    Shild,
    Multiple_Rocket_Launcher,
    Firebomb,
    Lightning
}

public enum Accessory
{
    Magnet,
    Book,
    Dumbbell,
    Milk,
    Meat,
    Magazine,
    Oil

}

public class WeaponController : MonoBehaviour
{
    // 나중에 오브젝트 풀로 연결 해줄것
    public Dictionary<eWeaponType, GameObject> _weapons = new Dictionary<eWeaponType, GameObject>();

    public GameObject _sword;
    public GameObject _defenderObj;
    public GameObject _drill;
    public GameObject _barrier;
    public GameObject _rocket;

    public GameObject _cocktail;
    public GameObject _cocktailAttack;

    public GameObject _lightning;

    public delegate IEnumerator WeaponActive(int Number);

    private float _speedPercent, _durtationPercent, _damagePercent, _scalePercent, _coolTimePercent;
    private int _attackCount;

    public float ScalePercent { get { return _scalePercent; } }

    [SerializeField]
    List<GameObject> TestObject = new List<GameObject>();

    //무기 갯수
    private int _weaponCount = 0;
    public WeaponActive[] _weaponActives = new WeaponActive[7];

    private float[] _cooltimes = new float[7] { 0, 0, 0, 0, 0, 0,0 };

    //몬스터 체크
    Vector3 _nearEnemyPos;

    public void LevelUpCooltime(float percent)
    {
        _coolTimePercent = _coolTimePercent + percent;
    }
    public void LevelUpDamage(float percent)
    {
        _damagePercent = _damagePercent + percent;
    }
    public void LevelUpAttackCount(int count)
    {
        _attackCount = _attackCount + count;
    }
    public void LevelUpscale(float percent)
    {
        _scalePercent = _scalePercent + percent;
    }

    private void Start()
    {
        //테스트
        int num = _weaponCount;
        /*_weaponActives[num] = SwordWeapon;
        _weaponActives[1] = DefenderWeapon;
        _weaponActives[2] = DrillWeaponRogic;
        _weaponActives[3] = BarrierWeaponRogic;
        _weaponActives[4] = Multiple_Rocket_Launcher;

        _weaponActives[5] = Molotov_cocktail_Weapon;
        _weaponActives[6] = Lightning;*/

        /*StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));
        StartCoroutine(_weaponActives[num](num++));*/
        //_weaponCount++;
    }

    private void Update()
    {
        if (PlayManager.Instance.IsGameStop)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _scalePercent += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _coolTimePercent += 0.1f;
        }

        Collider2D FindEnemy = Physics2D.OverlapCircle(transform.position, 50);

        if(FindEnemy != null && FindEnemy.CompareTag("Enemy"))
        {
            _nearEnemyPos  = FindEnemy.transform.position;
        }


        for (int i = 0; i < _cooltimes.Length; i++)
        {
            _cooltimes[i] += Time.deltaTime;
        }
    }

    //현재 무기가 몇개가 있는지! + 현재 무기들이 전부 6성까지 갔는지!! 확인
    public bool IsItemFull()
    {
        if(_weaponCount >= 6)
        {
            return true;
        }

        return false;
    }

    public bool IsItemLankFull()
    {
        

        return false;
    }

    public void AddWeapon(eWeaponType type)
    {
        if (_weaponCount >= 6)
        {
            return;
        }        

        switch (type)
        {
            case eWeaponType.Sword:
                _weaponActives[_weaponCount] = SwordWeapon;
                break;
            case eWeaponType.Tutelar:
                _weaponActives[_weaponCount] = DefenderWeapon;
                break;
            case eWeaponType.Drill:
                _weaponActives[_weaponCount] = DrillWeaponRogic;
                break;
            case eWeaponType.Shild:
                _weaponActives[_weaponCount] = BarrierWeaponRogic;
                break;
            case eWeaponType.Multiple_Rocket_Launcher:
                _weaponActives[_weaponCount] = Multiple_Rocket_Launcher;
                break;
            case eWeaponType.Firebomb:
                _weaponActives[_weaponCount] = Molotov_cocktail_Weapon;
                break;
            case eWeaponType.Lightning:
                _weaponActives[_weaponCount] = Lightning;
                break;
            default:
                break;
        }

        //무기 실행 해주고 
        StartCoroutine( _weaponActives[_weaponCount](_weaponCount++));

    }

    #region 검 공격
    public IEnumerator SwordWeapon(int Number)
    {


        Debug.Log("검 실행 그리고 번호 확인 " + Number);
        //Weapon weapon = new Weapon();
        //weapon.Init(1, 1, 1, 2, 20, new Vector3(2, 1, 1));

        //오브젝트 풀 생성
        ObjectPool pool = new ObjectPool(_sword);


        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Sword;

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            // Debug.Log("반복문 실행 그리고 쿨타임 확인 _cooltimes[Number] / _cool" + _cooltimes[Number] + " / " +cool);
            //Debug.Log("쿨타임 확인 " + weapon._originWeaponCoolTime + " / " + cool);


            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;

            //Debug.Log("쿨타임 완료 실행 공격 횟수 확인" + weapon._originAttackCount + _attackCount + " / " + weapon._originAttackCount
            //+ " / " + _attackCount);

            int _negative_positive_Num = 1;

            float posx = _sword.transform.localScale.x * 0.5f;

            Vector3 OffsetPos = new Vector3(_sword.transform.localScale.x * 0.5f, 0, 0);

            int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);

            for (int i = 0; i < weapon._originAttackCount + _attackCount; i++)
            {
                GameObject sword = pool.GetValue();
                sword.SetActive(true);

                IWeapon IWeapon = sword.GetComponent<IWeapon>();
                Animator weaponAnim = IWeapon.GetAnim();

                IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);
                sword.transform.position = transform.position + (OffsetPos + (OffsetPos * _scalePercent));

                ISword isword = sword.GetComponent<ISword>();
                isword.FollowPlayer(transform, OffsetPos);

                float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
                    (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent)) * _negative_positive_Num;
                float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                    (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent)) * _negative_positive_Num;

                //Debug.Log(Mathf.Abs(sword.transform.localScale.x));
                //Debug.Log(_negative_positive_Num);
                //Debug.Log((Mathf.Abs(sword.transform.localScale.x) * _scalePercent));
                sword.transform.localScale =
                    new Vector3(scalex,
                    scaley, 1);

                weaponAnim.Play("Attack");
                //Debug.Log(weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") + "weaponAnim.GetCurrentAnimatorStateInfo(0).IsName(\"Idle\")");

                yield return new WaitForSeconds(0.7f);
                OffsetPos *= -1;
                _negative_positive_Num *= -1;
                pool.ReturnObj(sword);               
            }

            //sword.SetActive(false);
            //sword.GetComponent<SpriteRenderer>().sprite = null;

        }

    }
    #endregion

    #region 공통 함수 혹은 코루틴 



    //애니메이션 
    IEnumerator WaitForAnimationEvent(Animator _anim)
    {
        Debug.Log(_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") + "_anim.GetCurrentAnimatorStateInfo(0).IsName(\"Idle\")");

        while (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)//_isActive
        {
            // Debug.Log("_anim.GetCurrentAnimatorStateInfo(0).IsName(\"Idle\") : " + _anim.GetCurrentAnimatorStateInfo(0).IsName("idle"));
            yield return null;
        }
        Debug.Log(_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") + "_anim.GetCurrentAnimatorStateInfo(0).IsName(\"Idle\")");
    }

    #endregion

    #region 수호자

    IEnumerator DefenderWeapon(int Number)
    {
        Debug.Log("디펜더 실행 그리고 번호 확인 " + Number);
        /*Weapon weapon = new Weapon();
        weapon.Init(1, 7, 10, 2, 10, new Vector3(1, 1, 1));*/

        ObjectPool pool = new ObjectPool(_defenderObj);


        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Defender;

            float orbitDistance = 1.5f; //중심 오브젝트로부터의 거리
            float orbistSpeed = 30.0f; //회전 속도 (도/ 초)

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;
            //Debug.Log("디펜더 쿨타임 다되었으니 발동");

            float _duration = weapon._originWeaponDuration + (weapon._originWeaponDuration * _durtationPercent);
            int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);

            GameObject[] objs = new GameObject[weapon._originAttackCount + _attackCount];
            float[] currentAngles = new float[weapon._originAttackCount + _attackCount];

            for (int i = 0; i < objs.Length; i++)
            {
                //앵글 설정
                float angle = i * (360f / weapon._originAttackCount + _attackCount);
                currentAngles[i] = angle;

                //거리 설정후 오브젝트 생성
                Vector3 Pos = DefenderCaculatePosition(transform, angle, orbitDistance);
                objs[i] = pool.GetValue();
                objs[i].transform.position = Pos;

               // Debug.Log("디펜더 생성 + " + objs[i].name);
                TestObject.Add(objs[i]);
                //유닛 기본값 설정
                IWeapon IWeapon = objs[i].GetComponent<IWeapon>();
                IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);

                float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
                    (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent));
                float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                    (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent));


                objs[i].transform.localScale =
                    new Vector3(scalex,
                    scaley, 1);

            }

            for (float duration = 0; duration < _duration;)
            {
                duration += Time.deltaTime;

                for (int i = 0; i < objs.Length; i++)
                {
                    currentAngles[i] += orbistSpeed * Time.deltaTime;
                    Vector3 position = DefenderCaculatePosition(transform, currentAngles[i], orbitDistance);
                    objs[i].transform.position = position;
                }


                yield return new WaitForEndOfFrame();

            }

            for (int i = 0; i < objs.Length; i++)
            {
                pool.ReturnObj(objs[i]);
                //Destroy(objs[i]);
                objs[i] = null;
            }
        }
    }

    //회전 각도 구하기
    Vector3 DefenderCaculatePosition(Transform playerpoint, float angle, float orbitDistance)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = playerpoint.position.x + Mathf.Cos(radians) * orbitDistance;
        float y = playerpoint.position.y + Mathf.Sin(radians) * orbitDistance;
        return new Vector3(x, y, playerpoint.position.z);
    }

    #endregion

    #region 드릴 무기
    IEnumerator DrillWeaponRogic(int Number)
    {
        /*Weapon weapon = new Weapon();
        weapon.Init(1, 5, 5, 2, 10, new Vector3(1, 1, 1));*/

        ObjectPool pool = new ObjectPool(_drill);

        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Drill;

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;


            float _duration = weapon._originWeaponDuration + (weapon._originWeaponDuration * _durtationPercent);

            GameObject[] objs = new GameObject[weapon._originAttackCount + _attackCount];
            float[] currentAngles = new float[weapon._originAttackCount + _attackCount];

            for (int i = 0; i < objs.Length; i++)
            {                
                //거리 설정후 오브젝트 생성
                objs[i] = pool.GetValue();
                objs[i].transform.position = transform.position;
                
                //유닛 기본값 설정
                IWeapon IWeapon = objs[i].GetComponent<IWeapon>();
                int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);

                IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);

                float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
                    (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent));
                float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                    (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent));


                objs[i].transform.localScale =
                    new Vector3(scalex,
                    scaley, 1);

            }

            /*for (float duration = 0; duration < _duration;)
            {
                duration += Time.deltaTime;               
                yield return new WaitForEndOfFrame();

            }*/
            yield return new WaitForSeconds(_duration);

            for (int i = 0; i < objs.Length; i++)
            {
                pool.ReturnObj(objs[i]);
            }

        }
    }

    #endregion

    #region Barrier 무기
    IEnumerator BarrierWeaponRogic(int Number)
    {
        /*Weapon weapon = new Weapon();
        weapon.Init(1, 5, 5, 2, 10, new Vector3(1, 1, 1));
*/

        GameObject berrier = new GameObject();
        berrier.transform.position = transform.position;


        //거리 설정후 오브젝트 생성
        berrier = Instantiate(_barrier, transform.position, Quaternion.identity);
        berrier.SetActive(false);
        //유닛 기본값 설정
        IWeapon IWeapon = berrier.GetComponent<IWeapon>();

        //플레이어 따라오게 하기
        Iberrier iberrier = berrier.GetComponent<Iberrier>();
        iberrier.FollowPlayer(transform);

        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Barrier;

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;


            berrier.SetActive(true);

            float _duration = weapon._originWeaponDuration + (weapon._originWeaponDuration * _durtationPercent);
            int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);


            IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);

            float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
                (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent));
            float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent));


            berrier.transform.localScale =
                new Vector3(scalex,
                scaley, 1);


            for (float duration = 0; duration < _duration;)
            {
                duration += Time.deltaTime;
                yield return new WaitForEndOfFrame();

            }
            berrier.SetActive(false);

        }
    }

    #endregion


    #region Bomber 무기
    IEnumerator Multiple_Rocket_Launcher(int Number)
    {
        /*Weapon weapon = new Weapon();
        weapon.Init(1, 5, 5, 10, 10, new Vector3(0.5f, 0.5f, 1));*/

        Vector3 targetPosition = Vector3.zero;

        ObjectPool pool = new ObjectPool(_rocket);


        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Rocket;

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            targetPosition = RotateAroundPlayer(weapon._originWeaponSpeed + (weapon._originWeaponSpeed * _speedPercent));

            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;


            float _duration = weapon._originWeaponDuration + (weapon._originWeaponDuration * _durtationPercent);


            GameObject[] Rockets = new GameObject[weapon._originAttackCount + _attackCount];


            for (int i = 0; i < Rockets.Length; i++)
            {
                int num = i;
                //거리 설정후 오브젝트 생성
                Rockets[i] = pool.GetValue();
                Rockets[i].transform.position = transform.position;

                int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);


                IWeapon IWeapon = Rockets[i].GetComponent<IWeapon>();
                IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);
                IWeapon.NewEndobj(() => pool.ReturnObj(Rockets[num]));


                float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
               (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent));
                float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                    (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent));


                Rockets[i].transform.localScale =
                    new Vector3(scalex,
                    scaley, 1);

                //로켓 인터페이스를 가져온다
                IRocket_Launcher IRocket = Rockets[i].GetComponent<IRocket_Launcher>();
                //Vector3 randpos = IRocket.SetRandomTargetPosition(transform.position);
                IRocket.SetTargetPosition(targetPosition);
                IRocket.MovetoTargetPosition();
                yield return new WaitForSeconds(0.25f);
            }


            yield return new WaitForSeconds(1f);


        }
    }
    
    private Vector3 RotateAroundPlayer(float speed)
    {
        float _rotationRadius = 5.0f;
        float _rotationSpeed = speed;

        float angle = _rotationSpeed * Time.time;
        Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * _rotationRadius; 
        Vector3 RocketPoint = transform.position + offset;

        return RocketPoint;

    }

    #endregion

    #region 화염병 무기
    IEnumerator Molotov_cocktail_Weapon(int Number)
    {
       /* Weapon weapon = new Weapon();
        weapon.Init(1, 5, 5, 2, 10, new Vector3(1, 1, 1));*/

        Vector3 targetPosition = Vector3.zero;

        //오브젝트 풀 생성
        ObjectPool pool = new ObjectPool(_cocktail);
        ObjectPool attackpool = new ObjectPool(_cocktailAttack);


        //방법 1. 무한 루프 2. 쿨타임 마다 코루틴 부르는 방식 
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Firebomb;


            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);


            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;


            float _duration = weapon._originWeaponDuration + (weapon._originWeaponDuration * _durtationPercent);
            int _damage = weapon._originDamage + (int)(weapon._originDamage * _damagePercent);


            GameObject[] cocktails = new GameObject[weapon._originAttackCount + _attackCount];
            GameObject[] cooktails_attack = new GameObject[weapon._originAttackCount + _attackCount];

            for (int i = 0; i < weapon._originAttackCount + _attackCount; i++)
            {
                int num = i;

                targetPosition = Molotov_cocktail_RandomPos();

                cooktails_attack[num] = attackpool.GetValue();
                cooktails_attack[num].SetActive(false);

                //거리 설정후 오브젝트 생성
                cocktails[num] = pool.GetValue();
                cocktails[num].transform.position = transform.position;

                IWeapon IWeapon = cocktails[num].GetComponent<IWeapon>();
                IWeapon.WeaponSettingInit(_damage, weapon._originWeaponSpeed);
                IWeapon.NewEndobj(() => pool.ReturnObj(cocktails[num]));

                float scalex = (Mathf.Abs(_sword.transform.localScale.x) +
               (Mathf.Abs(_sword.transform.localScale.x) * _scalePercent));
                float scaley = (Mathf.Abs(_sword.transform.localScale.y) +
                    (Mathf.Abs(_sword.transform.localScale.y) * _scalePercent));


                cocktails[num].transform.localScale =
                    new Vector3(scalex,
                    scaley, 1);

                //로켓 인터페이스를 가져온다
                IMolotov_cocktail Icocktail = cocktails[i].GetComponent<IMolotov_cocktail>();
                //칵테일 이 가지고 있을 칵테일 공격 스크립트에 함수 전달 ( 오브젝트 풀으로 돌아오는 코드)
                Icocktail.Init(cooktails_attack[num], () => attackpool.ReturnObj(cooktails_attack[num]));
                Icocktail.SetTargetPosition(targetPosition);                

                Icocktail.MovetoTargetPosition(_duration);
            }


            for (float duration = 0; duration < _duration;)
            {
                duration += Time.deltaTime;
                yield return new WaitForEndOfFrame();

            }

        }
    }

    Vector3 Molotov_cocktail_RandomPos()
    {
        //이걸 이제 distance 길이라고봐야함.나중에는 바꿀수 있게끔 할것
        float _rotationRadius = 2.0f;
        //float _rotationSpeed = 1.0f;

        //float angle = _rotationSpeed * Time.time;
        float angle = Random.Range(0, 360);
        Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * _rotationRadius;

        Vector3 randpos = transform.position + offset;

        return randpos;
    }
    #endregion


    #region 번개 공격
    IEnumerator Lightning(int Number)
    {
        /*Weapon weapon = new Weapon();
        weapon.Init(1, 1, 5, 1, 70, new Vector3(0.5f, 1, 1));*/

        ObjectPool pool = new ObjectPool(_lightning);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (PlayManager.Instance.IsGameStop)
            {
                continue;
            }

            Weapon weapon = PlayManager.Instance.Lightning;

            float calculateCool = weapon._originWeaponCoolTime - (weapon._originWeaponCoolTime * _coolTimePercent);
            float cool = Mathf.Clamp(calculateCool, 0, weapon._originWeaponCoolTime);

            if (_cooltimes[Number] < cool)
            {
                continue;
            }

            _cooltimes[Number] = 0;


            GameObject[] lightnings = new GameObject[weapon._originAttackCount + _attackCount];


            Collider2D[] findEnemys = Physics2D.OverlapCircleAll(transform.position, 7,LayerMask.GetMask("Enemy"));

            //Debug.Log("_attackCount + weapon._originAttackCoun 번개공격 확인" + (_attackCount + weapon._originAttackCount));
            for (int i = 0; i < _attackCount + weapon._originAttackCount; i++)
            {
                lightnings[i] = pool.GetValue();
                lightnings[i].SetActive(true);
                //이렇게하면 가까운 순으로 공격함 , 정말로 랜덤한 위치로할거면코드 수정
                //Debug.Log(findEnemys.Length + "감지 된 몬스터");
                if (findEnemys.Length >= i + 1)
                {
                    lightnings[i].transform.position = findEnemys[i].transform.position;

                }
                else
                {
                    break;
                }
                IWeapon Iweapon = lightnings[i].GetComponent<IWeapon>();
                Iweapon.WeaponSettingInit((int)(weapon._originDamage + (weapon._originDamage * _damagePercent)), 1);

                ILightning lightning = lightnings[i].GetComponent<ILightning>();
                lightning.AcvtiveWeapon();
            }
        }

    }

    #endregion
}
