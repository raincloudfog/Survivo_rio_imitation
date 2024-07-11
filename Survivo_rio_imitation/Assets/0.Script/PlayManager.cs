using GameItem;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    public Player _player;
    public WeaponController _weaponController;

    [SerializeField] StageUI _stageUI;
    public LavelUpUI _lavelUpUI;

    //경험치
    public GameObject[] _expScripts;

    public MonsterEvent _monsterEvent;

    public bool IsGameStop =false;


    public float ScreenWidth()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        return cameraWidth;
    }

    public float Screenheight()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        
        return cameraHeight;
    }

    //아이템 정보
    public List<Item> _items;

    public TextAsset _itemsjson;

    //자석 효과
    public int Magnet = 1;

    public List<Item> Items
    {
        get
        {
            return _items;
        }
    }

    public float ScalePercent
    {
        get
        {
            return _weaponController.ScalePercent;
        }
    }

    public int LEVEL
    {
        get
        {
            return _player.LEVEL;
        }
    }

    public Weapon Sword
    {
        get
        {
            return _lavelUpUI._itemEvent
                .Sword;
        }
    }

    public Weapon Defender
    {
        get
        {
            return _lavelUpUI._itemEvent
                .Defender;
        }
    }

    public Weapon Drill
    {
        get
        {
            return _lavelUpUI._itemEvent
                .Drill;
        }
    }

    public Weapon Barrier
    {
        get
        {
            return _lavelUpUI._itemEvent.Barrier;
        }
    }

    public Weapon Rocket
    {
        get
        {
            return _lavelUpUI._itemEvent.Rocket;
        }
    }

    public Weapon Firebomb
    {
        get
        {
            return _lavelUpUI._itemEvent.Firebomb;
        }
    }

    public Weapon Lightning
    {
        get
        {
            return _lavelUpUI._itemEvent.Lightning;
        }
    }

    //몬스터 죽일시
    public void Killmonster() => _stageUI.KillMonster();
    //돈 얻을시
    public void GetCoint(int num) => _stageUI.GetCoin(num);

    public void ExpObjectOn(int number) => _stageUI.ExpObjectOn(number);

    public void AddHp() => _player.AddHp();

   

    public void LevelUp()
    {
        _stageUI.LevelUp();

        for (int i = 0; i < _expScripts.Length; i++)
        {
            _expScripts[i].GetComponent<ExpScript>().LevelUpExp(_player.LEVEL);
        }
        _stageUI._levelTxt.text = _player.LEVEL.ToString();        
        _lavelUpUI.LevelUP();
    }

    public void LevelObjOff()
    {
        _stageUI.LevelObjOff();
    }

    public void LevelUpEnd() => _lavelUpUI.LevelUpEnd();

    public void MaxHPPlus(float percent) => _player.MaxHpPlus(percent);

    public void HpRegenUp(int num) => _player._hpRegen += num;

    public void StartGame()
    {
        
    }



    //레벨업 체크용
    IEnumerator CheckLevelUP()
    {
        while (_lavelUpUI.isItemFullUP() == false)
        {
            LevelUp();
            yield return new WaitForSeconds(0.1f);
            _lavelUpUI._levelUpItem[0].CheckLevelUP();

        }

        Debug.Log("아이템 풀강화함.");
    }


        public void Start()
    {
        
            ItemList itemList = JsonUtility.FromJson<ItemList>(_itemsjson.text);
            _items = itemList.items;

        Debug.Log(_items.Count);

        SettingInstance();

        _monsterEvent.StartGame();

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))

        {
            _lavelUpUI.LevelUP();
        }
        if (Input.GetKeyDown(KeyCode.I))

        {
            StartCoroutine(CheckLevelUP());
        }
    }

    public void DestroyManager()
    {
        Destroy(gameObject);
    }
}
