
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public enum MonsterName
{
    Bat,
    Zombie,
    Skellton,
}

//이클래스는 이제 이벤트 실행용
[System.Serializable]
public class MonsterSpawnEvent
{
    //public GameObject obj;
    public int _spawnTime; // 몬스터 스폰 시간
    //public UnityEvent _onSpawnEvent; // 스폰 이벤트        
    public int _spawnCount; // 몬스터 스폰개수
    public SpawnType _spawnType; // 소환 방식
    public MonsterName _monsterName; // 몬스터 종류

    public enum SpawnType
    {
        RandomOutsideScreen,
        Circular
    }

    

}

//딕셔너리 해도 됨. 인스펙터에 확인용도로 클래스 채택
[System.Serializable]
public class NextMonsters
{
    public MonsterName _monsterName; // 몬스터 종류
    //public int _monsterCount; // 몬스터 마리수
    public bool _isSpawn; // 몬스터를 소환용 불값
}

public class MonsterEvent : MonoBehaviour
{
    public Game_Timer _timer;

    /*
      몬스터를 특정 시간에 내보네는 이벤트를 담당함
      여기서 몬스터를 골라넣을수 있음
      특정 시간도 여기서 추가할수 있게 하려고함
      이렇게 하면 스테이지마다 몬스터 이벤트 스크립트를 만들필요없이 값만 설정해주면 될 것 같음
     */
    [Header("Enemy")]
    public GameObject _bat;
    public GameObject _zombie;
    public GameObject _skellton;
    public GameObject _Boss; // 15분게임을 목표로 하고있어서 보스를 3마리로 늘려도 됨.


    ObjectPool _batpool;
    ObjectPool _zombiepool;
    ObjectPool _skelltontpool;
    ObjectPool _bosspool;
    ObjectPool[] _exppool = new ObjectPool[3];        

    //현재 몇마리의 몬스터가 나와있느지 확인
    public List<GameObject> _monsters = new List<GameObject>();

    //다음 순서의 몬스터확인 //  원한다면 인스펙터에서 몇분에 몇마리를 넣어줄지 가능할 것 같음.
    public NextMonsters[] _NextMonsters;
    int _nextmonsterInt = 0;
    //    public List<GameObject> _NextMonsters = new List<GameObject>();

    //몬스터 소환되는 숫자
    int _enemysLengh = 0;

    [Space(5)]
    public List<MonsterSpawnEvent> _events; //스폰 이벤트 목록    

    [SerializeField] float _spawnInterval = 10.0f;// 정기적 소환 간격

    private float _elapsedTime; // 경과 시간

    [Space(5), Header("보스 소환"), SerializeField]
    private bool _isStartBoss;    

    //경험치 확률
    private float[] ExpGem = new float[] { 80,20,1};

    

    public void StartGame()
    {
        _batpool = new ObjectPool(_bat);
        _zombiepool = new ObjectPool(_zombie);
        _skelltontpool = new ObjectPool(_skellton);

        _exppool = new ObjectPool[3];

        _exppool[0] = new ObjectPool(PlayManager.instance._expScripts[0]);
        _exppool[1] = new ObjectPool(PlayManager.instance._expScripts[1]);
        _exppool[2] = new ObjectPool(PlayManager.instance._expScripts[2]);
        

        StartCoroutine( SetMonsterCount());
        StartCoroutine(RegularSpawn());
        foreach (var spawnEvent in _events)
        {
            StartCoroutine(SpawnMonsterAtTime(spawnEvent));
        }
    }
  

    //몬스터 숫자 정하기
    private IEnumerator SetMonsterCount()
    {
        _enemysLengh = 30;
        NextMonster(true, false, false);

        //몬스터 제한 수 와 나오는 몬스터 정하기
        //이것도 이벤트로 할 수 있으면 할 듯함.
        while (_timer.GetMinutes() < 1)
        {
            yield return new WaitForEndOfFrame();
        }
  
        //무슨 몬스터 나오게 할 것인지
        NextMonster(true, false, false);


        while (_timer.GetMinutes() < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        
        while (_timer.GetMinutes() < 3)
        {
            yield return new WaitForEndOfFrame();
        }

        //무슨 몬스터 나오게 할 것인지
        NextMonster(true, true, false);

        while (_timer.GetMinutes() < 4)
        {
            yield return new WaitForEndOfFrame();
        }
        _enemysLengh = 50;
        while (_timer.GetMinutes() < 5)
        {
            yield return new WaitForEndOfFrame();
        }
        
        _isStartBoss = true;        
        StartCoroutine(StartBossFight());
    }

    public void NextMonster(bool a, bool b , bool c)
    {
        _NextMonsters[0]._isSpawn = a;
        _NextMonsters[1]._isSpawn = b;
        _NextMonsters[2]._isSpawn = c;        
    }

    // 특정 시간 마다 몬스터 소환 이벤트
    private IEnumerator SpawnMonsterAtTime(MonsterSpawnEvent monsterSpawnEvent)
    {
        // 특정 시간이 될 때까지 대기

        while (_timer.GetMinutes() < monsterSpawnEvent._spawnTime)
        {
            yield return new WaitForEndOfFrame();
        }

        if (_timer.GetMinutes() >= monsterSpawnEvent._spawnTime)
        {
            // 이벤트 실행
            // 몬스터 스폰
                SpawnMonster( monsterSpawnEvent._spawnCount, monsterSpawnEvent);
        }
    }


    //일반적으로 몬스터 소환
    private IEnumerator RegularSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(_spawnInterval);            

            if (_isStartBoss)
            {

            }

            for (int i = 0; i < _enemysLengh; i++)
            {
                SpawnMonster(1);
            }
        }
    }

    private IEnumerator StartBossFight()
    {
        //몬스터 전부 없애고 클리어
        for (int i = 0; i < _monsters.Count; i++)
        {
            _monsters[i].GetComponent<Monster>().ClearMonster();            
        }
        _monsters.Clear();

        //보스 소환

        Vector3 spawnPosition = Vector3.zero;

       
        spawnPosition = GetRandomOutsideScreenPosition();


        GameObject enemy = _bosspool.GetValue();
        _monsters.Add(enemy);
        Monster M_enemy = enemy.GetComponent<Monster>();



        int num = Choose(ExpGem);
        GameObject gem = _exppool[num].GetValue();
        gem.GetComponent<ExpScript>().Init(PlayManager.Instance.LEVEL, () => _exppool[num].ReturnObj(gem));
        gem.SetActive(false);

        //람다식에서 중괄호를 하면여러개의 함수를 넣을 수 있다!!
        M_enemy.newEndObj(() => { _bosspool.ReturnObj(enemy); DeathMonster(enemy, gem); _isStartBoss = false; });

        enemy.SetActive(true);
        enemy.transform.localPosition = spawnPosition;
        


        while (_isStartBoss)
        {
            yield return new WaitForEndOfFrame();

           
        }
    }

    //몬스터 소환 // 구버전
    private void SpawnMonster(MonsterSpawnEvent.SpawnType spawnType, int totalCount)
    {
        //현재 생성된 몬스터의 숫자가 150 이상 이면 더이상 생성 안함.
        if(_monsters.Count >= 150)
        {
            return;
        }

        Vector3 spawnPosition = Vector3.zero;

        switch (spawnType)
        {
            case MonsterSpawnEvent.SpawnType.RandomOutsideScreen:
                spawnPosition = GetRandomOutsideScreenPosition();
                break;
            case MonsterSpawnEvent.SpawnType.Circular:
                SpawnMonstersInCircle(totalCount);
                return;                
            default:
                break;
        }

        Debug.Log("몬스터 소환 함수");

        GameObject enemy = _batpool.GetValue();
        _monsters.Add(enemy);

        Monster M_enemy = enemy.GetComponent<Monster>();
        //M_enemy.newEndObj(() => _batpool.ReturnObj(enemy),() => DeathMonster(enemy));

        enemy.SetActive(true);
        enemy.transform.localPosition = spawnPosition;
            
    }

    //몬스터 소환
    private void SpawnMonster(
         int totalCount, MonsterSpawnEvent spawnevent = null)
    {
        //현재 생성된 몬스터의 숫자가 _enemysLengh 이상 이면 더이상 생성 안함.
        if (_monsters.Count >= _enemysLengh)
        {
            return;
        }

        Vector3 spawnPosition = Vector3.zero;

        if(spawnevent != null)
        {
            if(spawnevent._spawnType == MonsterSpawnEvent.SpawnType.Circular)
            {
                SpawnMonstersInCircle(totalCount);
                return;
            }            
        }

        spawnPosition = GetRandomOutsideScreenPosition();

        if (_NextMonsters[_nextmonsterInt]._isSpawn)
        {
            int num = _nextmonsterInt >= 4 ? _nextmonsterInt++ : 0;
            GetMonster(_NextMonsters[num], spawnPosition);
        }
    }

    public void GetMonster(NextMonsters nextmonster,Vector3 point)
    {
        //현재 생성된 몬스터의 숫자가 _enemysLengh 이상 이면 더이상 생성 안함.
        if (_monsters.Count >= _enemysLengh)
        {
            return;
        }

        GameObject enemy = null;
        Monster monster = null;


        //해당 경험치 주기
        int num = Choose(ExpGem);
        GameObject gem = _exppool[num].GetValue();
        gem.GetComponent<ExpScript>().Init(PlayManager.Instance.LEVEL, () => _exppool[num].ReturnObj(gem));
        gem.SetActive(false);
        switch (nextmonster._monsterName)
        {
            case MonsterName.Bat:
                enemy = _batpool.GetValue();
                 monster = enemy.GetComponent<Monster>();
                monster.newEndObj(() => { _batpool.ReturnObj(enemy); DeathMonster(enemy,gem); });
                break;
            case MonsterName.Zombie:
                enemy = _zombiepool.GetValue();
                monster = enemy.GetComponent<Monster>();
                monster.newEndObj(() => { _zombiepool.ReturnObj(enemy); DeathMonster(enemy, gem); });
                break;
            case MonsterName.Skellton:
                enemy = _skelltontpool.GetValue();
                monster = enemy.GetComponent<Monster>();
                monster.newEndObj(() => { _skelltontpool.ReturnObj(enemy); DeathMonster(enemy, gem); });
                break;
            default:
                break;
                
        }

        _monsters.Add(monster.gameObject);
        enemy.SetActive(true);
        enemy.transform.localPosition = point;
    }

    //몬스터가 죽으면 리스트에서 제외
    private void DeathMonster(GameObject obj, GameObject gem)
    {
        
        _monsters.Remove(obj);
        gem.transform.position = obj.transform.position;
        gem.SetActive(true);
    }

    //범위 랜덤
    private Vector3 GetRandomOutsideScreenPosition()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        // Adjust these values to control how far off-screen the monsters spawn
        float spawnBuffer = 5.0f;

        float randomX = Random.Range(-cameraWidth / 2 - spawnBuffer, cameraWidth / 2 + spawnBuffer);
        float randomY = Random.Range(-cameraHeight / 2 - spawnBuffer, cameraHeight / 2 + spawnBuffer);

        if (Random.Range(0, 2) == 0)
        {
            randomX = randomX < 0 ? randomX - mainCamera.orthographicSize : randomX + mainCamera.orthographicSize;
        }
        else
        {
            randomY = randomY < 0 ? randomY - mainCamera.orthographicSize : randomY + mainCamera.orthographicSize;
        }

        return new Vector3(randomX, randomY, 0);
    }


    // 원형 소환
    /*private Vector3 GetRandomCircularPosition(int totalCount, int index)
    {
        Vector3 point = PlayManager.Instance._player.transform.position;

        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        float radius = cameraWidth - 0.5f; // 원형 반지름
        float angle = (2 * Mathf.PI / totalCount) * index;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        return new Vector3 (x, y, 0);
    }*/

    private void SpawnMonstersInCircle(int count)
    {
        Vector3 point = PlayManager.Instance._player.transform.position;

        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        float radius = cameraWidth - 0.5f; // 원형 반지름

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            float x = Mathf.Cos(angle)* radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = new Vector3(point.x + x , point.y  + y);

            GameObject enemy = _batpool.GetValue();
            _monsters.Add(enemy);

            int num = Choose(ExpGem);
            GameObject gem = _exppool[num].GetValue();
            gem.GetComponent<ExpScript>().Init(PlayManager.Instance.LEVEL, () => _exppool[num].ReturnObj(gem));
            gem.SetActive(false);

            Monster M_enemy = enemy.GetComponent<Monster>();
            M_enemy.newEndObj(() => { _batpool.ReturnObj(enemy); DeathMonster(enemy, gem); });

            enemy.SetActive(true);
            enemy.transform.localPosition = spawnPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale += 1;
        }
    }

    public int Choose(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
