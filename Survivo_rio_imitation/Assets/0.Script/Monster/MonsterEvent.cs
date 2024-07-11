
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public enum MonsterName
{
    Bat,
    Zombie,
    Skellton,
}

//��Ŭ������ ���� �̺�Ʈ �����
[System.Serializable]
public class MonsterSpawnEvent
{
    //public GameObject obj;
    public int _spawnTime; // ���� ���� �ð�
    //public UnityEvent _onSpawnEvent; // ���� �̺�Ʈ        
    public int _spawnCount; // ���� ��������
    public SpawnType _spawnType; // ��ȯ ���
    public MonsterName _monsterName; // ���� ����

    public enum SpawnType
    {
        RandomOutsideScreen,
        Circular
    }

    

}

//��ųʸ� �ص� ��. �ν����Ϳ� Ȯ�ο뵵�� Ŭ���� ä��
[System.Serializable]
public class NextMonsters
{
    public MonsterName _monsterName; // ���� ����
    //public int _monsterCount; // ���� ������
    public bool _isSpawn; // ���͸� ��ȯ�� �Ұ�
}

public class MonsterEvent : MonoBehaviour
{
    public Game_Timer _timer;

    /*
      ���͸� Ư�� �ð��� �����״� �̺�Ʈ�� �����
      ���⼭ ���͸� �������� ����
      Ư�� �ð��� ���⼭ �߰��Ҽ� �ְ� �Ϸ�����
      �̷��� �ϸ� ������������ ���� �̺�Ʈ ��ũ��Ʈ�� �����ʿ���� ���� �������ָ� �� �� ����
     */
    [Header("Enemy")]
    public GameObject _bat;
    public GameObject _zombie;
    public GameObject _skellton;
    public GameObject _Boss; // 15�а����� ��ǥ�� �ϰ��־ ������ 3������ �÷��� ��.


    ObjectPool _batpool;
    ObjectPool _zombiepool;
    ObjectPool _skelltontpool;
    ObjectPool _bosspool;
    ObjectPool[] _exppool = new ObjectPool[3];        

    //���� ����� ���Ͱ� �����ִ��� Ȯ��
    public List<GameObject> _monsters = new List<GameObject>();

    //���� ������ ����Ȯ�� //  ���Ѵٸ� �ν����Ϳ��� ��п� ����� �־����� ������ �� ����.
    public NextMonsters[] _NextMonsters;
    int _nextmonsterInt = 0;
    //    public List<GameObject> _NextMonsters = new List<GameObject>();

    //���� ��ȯ�Ǵ� ����
    int _enemysLengh = 0;

    [Space(5)]
    public List<MonsterSpawnEvent> _events; //���� �̺�Ʈ ���    

    [SerializeField] float _spawnInterval = 10.0f;// ������ ��ȯ ����

    private float _elapsedTime; // ��� �ð�

    [Space(5), Header("���� ��ȯ"), SerializeField]
    private bool _isStartBoss;    

    //����ġ Ȯ��
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
  

    //���� ���� ���ϱ�
    private IEnumerator SetMonsterCount()
    {
        _enemysLengh = 30;
        NextMonster(true, false, false);

        //���� ���� �� �� ������ ���� ���ϱ�
        //�̰͵� �̺�Ʈ�� �� �� ������ �� ����.
        while (_timer.GetMinutes() < 1)
        {
            yield return new WaitForEndOfFrame();
        }
  
        //���� ���� ������ �� ������
        NextMonster(true, false, false);


        while (_timer.GetMinutes() < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        
        while (_timer.GetMinutes() < 3)
        {
            yield return new WaitForEndOfFrame();
        }

        //���� ���� ������ �� ������
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

    // Ư�� �ð� ���� ���� ��ȯ �̺�Ʈ
    private IEnumerator SpawnMonsterAtTime(MonsterSpawnEvent monsterSpawnEvent)
    {
        // Ư�� �ð��� �� ������ ���

        while (_timer.GetMinutes() < monsterSpawnEvent._spawnTime)
        {
            yield return new WaitForEndOfFrame();
        }

        if (_timer.GetMinutes() >= monsterSpawnEvent._spawnTime)
        {
            // �̺�Ʈ ����
            // ���� ����
                SpawnMonster( monsterSpawnEvent._spawnCount, monsterSpawnEvent);
        }
    }


    //�Ϲ������� ���� ��ȯ
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
        //���� ���� ���ְ� Ŭ����
        for (int i = 0; i < _monsters.Count; i++)
        {
            _monsters[i].GetComponent<Monster>().ClearMonster();            
        }
        _monsters.Clear();

        //���� ��ȯ

        Vector3 spawnPosition = Vector3.zero;

       
        spawnPosition = GetRandomOutsideScreenPosition();


        GameObject enemy = _bosspool.GetValue();
        _monsters.Add(enemy);
        Monster M_enemy = enemy.GetComponent<Monster>();



        int num = Choose(ExpGem);
        GameObject gem = _exppool[num].GetValue();
        gem.GetComponent<ExpScript>().Init(PlayManager.Instance.LEVEL, () => _exppool[num].ReturnObj(gem));
        gem.SetActive(false);

        //���ٽĿ��� �߰�ȣ�� �ϸ鿩������ �Լ��� ���� �� �ִ�!!
        M_enemy.newEndObj(() => { _bosspool.ReturnObj(enemy); DeathMonster(enemy, gem); _isStartBoss = false; });

        enemy.SetActive(true);
        enemy.transform.localPosition = spawnPosition;
        


        while (_isStartBoss)
        {
            yield return new WaitForEndOfFrame();

           
        }
    }

    //���� ��ȯ // ������
    private void SpawnMonster(MonsterSpawnEvent.SpawnType spawnType, int totalCount)
    {
        //���� ������ ������ ���ڰ� 150 �̻� �̸� ���̻� ���� ����.
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

        Debug.Log("���� ��ȯ �Լ�");

        GameObject enemy = _batpool.GetValue();
        _monsters.Add(enemy);

        Monster M_enemy = enemy.GetComponent<Monster>();
        //M_enemy.newEndObj(() => _batpool.ReturnObj(enemy),() => DeathMonster(enemy));

        enemy.SetActive(true);
        enemy.transform.localPosition = spawnPosition;
            
    }

    //���� ��ȯ
    private void SpawnMonster(
         int totalCount, MonsterSpawnEvent spawnevent = null)
    {
        //���� ������ ������ ���ڰ� _enemysLengh �̻� �̸� ���̻� ���� ����.
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
        //���� ������ ������ ���ڰ� _enemysLengh �̻� �̸� ���̻� ���� ����.
        if (_monsters.Count >= _enemysLengh)
        {
            return;
        }

        GameObject enemy = null;
        Monster monster = null;


        //�ش� ����ġ �ֱ�
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

    //���Ͱ� ������ ����Ʈ���� ����
    private void DeathMonster(GameObject obj, GameObject gem)
    {
        
        _monsters.Remove(obj);
        gem.transform.position = obj.transform.position;
        gem.SetActive(true);
    }

    //���� ����
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


    // ���� ��ȯ
    /*private Vector3 GetRandomCircularPosition(int totalCount, int index)
    {
        Vector3 point = PlayManager.Instance._player.transform.position;

        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        float radius = cameraWidth - 0.5f; // ���� ������
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
        float radius = cameraWidth - 0.5f; // ���� ������

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
