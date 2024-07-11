using GameItem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ItemEvent : MonoBehaviour
{
    public EndObj _restart;
    //public UnityEvent _restart;

    public Weapons _sword = new Weapons();
    public Weapons _defender = new Weapons();
    public Weapons _drill = new Weapons();
    public Weapons _barrier = new Weapons();
    public Weapons _rocket = new Weapons();
    public Weapons _firebomb = new Weapons();
    public  Weapons _lightning = new Weapons();

    public Item _magnet = new Item();
    public Item _book = new Item();
    public Item _dumbbell = new Item();
    public Item _milk = new Item();
    public Item _meat = new Item();
    public Item _magazine = new Item();
    public Item _oil = new Item();




    //현재 얻은 무기
    public List<Weapons> _playerWeaponList = new List<Weapons>();
    public List<Item> _playerAccesoryList = new List<Item>();
    
    //현재 얻은 무기 번호
    public List<int> _playerweaponNumberList = new List<int>();
    //현재 얻은 악세서리 번호
    public List<int> _playerAccesoryNumberList = new List<int>();

    public Weapon Sword
    {
        get
        {
            return _sword._weapon;
        }
    }

    public Weapon Defender
    {
        get
        {
            return _defender._weapon;
        }
    }

    public Weapon Drill
    {
        get
        {
            return _drill._weapon;
        }
    }

    public Weapon Barrier
    {
        get
        {
            return _barrier._weapon;
        }
    }

    public Weapon Rocket
    {
        get
        {
            return _rocket._weapon;
        }
    }

    public Weapon Firebomb
    {
        get
        {
            return _firebomb._weapon;
        }
    }

    public Weapon Lightning
    {
        get
        {
            return _lightning._weapon;
        }
    }


    private void Awake()
    {
        _restart = () => { PlayManager.Instance.IsGameStop = false; };
    }

    //현재 무기가 몇개가 있는지! + 현재 무기들이 전부 6성까지 갔는지!! 확인
    public bool IsWeaponFull()
    {
        if (_playerWeaponList.Count >= 6)
        {
            return true;
        }

        return false;
    }

    //현재 장신구가 다차있는지 확인
    public bool IsAccesoryFull()
    {
        if (_playerAccesoryNumberList.Count >= 6)
        {
            return true;
        }

        return false;
    }

    public bool istypeWeapon(int num)
    {
        switch (num)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                return true;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
                return false;            
        }

        return false;
    }

    //무기들이 6성인지 확인
    public bool IsItemLankFull()
    {
        if(IsWeaponFull() == false)
        {
            return false;
        }

        //6성이 몇개인지 확인 // 나중에는 6성 말고도 각성무기 인지도 확인
        int num = 0;

        foreach (Weapons weapons in _playerWeaponList)
        {
            if (weapons.IsFullLank == true) num++;
        }

        foreach (Item item in _playerAccesoryList)
        {
            if (item.IsFullLank == true) num++;
        }

        //아이템이 전부 풀강화인지 확인
        if(num >= 12)
        {
            return true;
        }

        return false;
    }

    //현재 가지고 있는 무기 번호
    public List<int> GetWeaponNumber()
    {
        //현재 강화가 남아있는 아이템만 주기

        List<int> getList = new List<int>();

        foreach (int item in _playerweaponNumberList)
        {
            if(IsItemFullUp(item) == false)
            {
                getList.Add(item);
            }
        }        

        return getList;
    }

    public List<int> GetAccesoryNumber()
    {

        //현재 강화가 남아있는 아이템만 주기

        List<int> getList = new List<int>();
        
        foreach (int item in _playerAccesoryNumberList)
        {
            if (IsItemFullUp(item) == false)
            {
                getList.Add(item);
            }
        }

        return getList;
    }



    //지금 해당 번호의 무기가 6성인지확인
    public bool IsItemFullUp(int number)
    {
        switch (number)
        {
            case 0: 
                return _sword.IsFullLank;
            case 1:
                return _defender.IsFullLank ;
            case 2: 
                return _drill.IsFullLank;
            case 3: 
                return _barrier.IsFullLank;
            case 4:
                return _rocket.IsFullLank;
            case 5:
                return _firebomb.IsFullLank;
            case 6:
                return _lightning.IsFullLank;            
            case 7:
                return _magnet.IsFullLank;
            case 8:
                return _book.IsFullLank;
            case 9:
                return _dumbbell.IsFullLank;
            case 10:
                return _milk.IsFullLank;
            case 11:
                return _meat.IsFullLank;
            case 12:
                return _magazine.IsFullLank;
            case 13:
                return _oil.IsFullLank;
            default:
                break;
        }

        return false;
    }

    //골드 
    public void GetGold()
    {
        Debug.Log("골드를 얻었습니다.");
    }

    //회복 아이템
    public void GetHeal()
    {
        PlayManager.Instance.AddHp();

    }

    

    public void LavelUPSword()
    {
        switch (_sword.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Sword);
                _playerweaponNumberList.Add(0);
                _playerWeaponList.Add(_sword);
                break;
            case 2:
                _sword._weapon._originAttackCount++;
                _sword._weapon._originDamage += 10;
                break;

            case 3:                               
            case 4:
                _sword._weapon._originDamage += 10;
                break;
            case 5:
                _sword._weapon._originAttackCount++;
                _sword._weapon._originDamage += 10;
                break;
            case 6:
                _sword._weapon._originDamage += 20;
                _sword.IsFullLank = true;
                break;
            default:
                break;
        }

        _sword.lank++;
        _restart?.Invoke();
    }

    public void LavelUpDefender()
    {
        switch (_defender.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Tutelar);
                _playerWeaponList.Add(_defender);
                _playerweaponNumberList.Add(1);
                break;
            case 2:
                _defender._weapon._originAttackCount++;
                _defender._weapon._originWeaponDuration += 0.5f;
                _defender._weapon._originDamage += 10;
                break;

            case 3:
                _defender._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _defender._weapon._originDamage += 10;
                _defender._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _defender._weapon._originAttackCount++;
                _defender._weapon._originWeaponDuration += 0.5f;
                _defender._weapon._originDamage += 10;
                break;
            case 6:
                _defender._weapon._originDamage += 20;
                _defender._weapon._originWeaponDuration += 1f;
                _defender.IsFullLank = true;
                break;
            default:
                break;
        }

        _defender.lank++;
        _restart?.Invoke();
    }

    public void LavelUpDrill()
    {
        switch (_drill.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Drill);
                _playerWeaponList.Add(_drill);
                _playerweaponNumberList.Add(2);
                break;
            case 2:
                _drill._weapon._originAttackCount++;
                _drill._weapon._originWeaponDuration += 0.5f;
                _drill._weapon._originDamage += 10;
                break;

            case 3:
                _drill._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _drill._weapon._originDamage += 10;
                _drill._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _drill._weapon._originAttackCount++;
                _drill._weapon._originWeaponDuration += 0.5f;
                _drill._weapon._originDamage += 10;
                break;
            case 6:
                _drill._weapon._originDamage += 20;
                _drill._weapon._originWeaponDuration += 1f;
                _drill.IsFullLank = true;
                break;
            default:
                break;
        }

        _drill.lank++;
        _restart?.Invoke();
    }

    public void LavelUpBarrier()
    {
        switch (_barrier.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Shild);
                _playerWeaponList.Add(_barrier);
                _playerweaponNumberList.Add(3);
                break;
            case 2:
                _barrier._weapon._originAttackCount++;
                _barrier._weapon._originWeaponDuration += 0.5f;
                _barrier._weapon._originDamage += 5;
                break;

            case 3:
                _barrier._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _barrier._weapon._originDamage += 5;
                _barrier._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _barrier._weapon._originAttackCount++;
                _barrier._weapon._originWeaponDuration += 0.5f;
                _barrier._weapon._originDamage += 5;
                break;
            case 6:
                _barrier._weapon._originDamage += 10;
                _barrier._weapon._originWeaponDuration += 1f;
                _barrier.IsFullLank = true;
                break;
            default:
                break;
        }

        _barrier.lank++;
        _restart?.Invoke();
    }

    public void LavelUpRocket()
    {
        switch (_rocket.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Multiple_Rocket_Launcher);
                _playerWeaponList.Add(_rocket);

                _playerweaponNumberList.Add(4);
                break;
            case 2:
                _rocket._weapon._originAttackCount++;
                _rocket._weapon._originWeaponDuration += 0.5f;
                _rocket._weapon._originDamage += 10;
                break;

            case 3:
                _rocket._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _rocket._weapon._originDamage += 10;
                _rocket._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _rocket._weapon._originAttackCount++;
                _rocket._weapon._originWeaponDuration += 0.5f;
                _rocket._weapon._originDamage += 10;
                break;
            case 6:
                _rocket._weapon._originDamage += 20;
                _rocket._weapon._originWeaponDuration += 1f;
                _rocket.IsFullLank = true;
                break;
            default:
                break;
        }

        _rocket.lank++;
        _restart?.Invoke();
    }

    public void LavelUpFirebomb()
    {
        switch (_firebomb.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Firebomb);
                _playerWeaponList.Add(_firebomb);
                _playerweaponNumberList.Add(5);
                break;
            case 2:
                _firebomb._weapon._originAttackCount++;
                _firebomb._weapon._originWeaponDuration += 0.5f;
                _firebomb._weapon._originDamage += 10;
                break;

            case 3:
                _firebomb._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _firebomb._weapon._originDamage += 10;
                _firebomb._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _firebomb._weapon._originAttackCount++;
                _firebomb._weapon._originWeaponDuration += 0.5f;
                _firebomb._weapon._originDamage += 10;
                break;
            case 6:
                _firebomb._weapon._originDamage += 20;
                _firebomb._weapon._originWeaponDuration += 1f;
                _firebomb.IsFullLank = true;
                break;
            default:
                break;
        }

        _firebomb.lank++;
        _restart?.Invoke();
    }

    public void LavelUpLightning()
    {
        switch (_lightning.lank)
        {
            case 1:
                PlayManager.instance._weaponController.AddWeapon(eWeaponType.Lightning);
                _playerWeaponList.Add(_lightning);
                _playerweaponNumberList.Add(6);
                break;
            case 2:
                _lightning._weapon._originAttackCount++;
                _lightning._weapon._originWeaponDuration += 0.5f;
                _lightning._weapon._originDamage += 10;
                break;

            case 3:
                _lightning._weapon._originWeaponDuration += 0.5f;
                break;
            case 4:
                _lightning._weapon._originDamage += 10;
                _lightning._weapon._originWeaponDuration += 0.5f;
                break;
            case 5:
                _lightning._weapon._originAttackCount++;
                _lightning._weapon._originWeaponDuration += 0.5f;
                _lightning._weapon._originDamage += 10;
                break;
            case 6:
                _lightning._weapon._originDamage += 20;
                _lightning._weapon._originWeaponDuration += 1f;
                _lightning.IsFullLank = true;
                break;
            default:
                break;
        }

        _lightning.lank++;
        _restart?.Invoke();
    }

    

    public void LavelUpMagnet()
    {
        if (_magnet.lank == 1)
        {
            _playerAccesoryNumberList.Add(7);
            _playerAccesoryList.Add(_magnet);
        }
        if (_magnet.lank <= 5)
        {
            PlayManager.instance.Magnet++;

        }
        else
        {
            _magnet.IsFullLank = true;

        }       

        _magnet.lank++;
        _restart?.Invoke();
    }

    public void LavelUpBook()
    {
        if (_book.lank == 1)
        {
            _playerAccesoryNumberList.Add(8);
            _playerAccesoryList.Add(_book);
        }
        if (_book.lank <= 5)
        {
            PlayManager.Instance._weaponController.LevelUpCooltime(0.1f);
        }
        else
        {
            _book.IsFullLank = true;
        }


        _book.lank++;
        _restart?.Invoke();

    }

    public void LevelUpDamage()
    {
        if (_dumbbell.lank == 1)
        {
            _playerAccesoryNumberList.Add(9);
            _playerAccesoryList.Add(_dumbbell);
        }
        if (_dumbbell.lank <= 5)
        {
            PlayManager.Instance._weaponController.LevelUpDamage(0.1f);
        }
        else
        {
            _dumbbell.IsFullLank = true;

        }

        _dumbbell.lank++;
        _restart?.Invoke();
    }

    //체력 재생
    public void LavelUpMilk()
    {
        if (_milk.lank == 1)
        {
            _playerAccesoryNumberList.Add(10);
            _playerAccesoryList.Add(_milk);
        }
        if (_milk.lank <= 5)
        {
            PlayManager.Instance.HpRegenUp(1);
        }
        else
        {
            _milk.IsFullLank = true;

        }

        _milk.lank++;
        _restart?.Invoke();
    }

    public void LavelUpMeat()
    {
        if (_meat.lank == 1)
        {
            _playerAccesoryNumberList.Add(11);
            _playerAccesoryList.Add(_meat);
        }
        if (_meat.lank <= 5)
        {
            PlayManager.Instance.MaxHPPlus(0.1f);

        }
        else
        {
            _meat.IsFullLank = true;

        }

        _meat.lank++;
        _restart?.Invoke();
    }

    public void LavelUpMagazine()
    {
        if (_magazine.lank == 1)
        {
            _playerAccesoryNumberList.Add(12);
            _playerAccesoryList.Add(_magazine);
        }
        if (_magazine.lank <= 3)
        {
            PlayManager.Instance._weaponController.LevelUpAttackCount(1);
        }
        else
        {
            _magazine.IsFullLank = true;

        }       

        _magazine.lank++;
        _restart?.Invoke();
    }

    public void LavelUpOil()
    {
        if (_oil.lank == 1)
        {
            _playerAccesoryNumberList.Add(13);
            _playerAccesoryList.Add(_oil);
        }
        if (_oil.lank <= 3)
        {
            PlayManager.Instance._weaponController.LevelUpscale(0.2f);
        }
        else
        {
            _oil.IsFullLank = true;
        }

        _oil.lank++;
        _restart?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//이클래스는 이제 이벤트 실행용
[System.Serializable]
public class ItemEventScript
{
    public UnityEvent _onEvent; // 스폰 이벤트               
}

[System.Serializable]

public class Weapons
{
    //무기가 담겨있는 그릇
    public eWeaponType _weaponType;
    public int lank = 1;
    public Weapon _weapon;
    public bool IsFullLank = false;
}


namespace GameItem
{


    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public string[] description;
        public string type;
        public int lank;
        public bool IsFullLank;
    }
}