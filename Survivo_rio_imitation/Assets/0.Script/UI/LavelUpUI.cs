using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Item = GameItem.Item;



public class LavelUpUI : MonoBehaviour
{
    Item gold;
    Item Heal;

    //해당 함수가 가지고 있는 
    public GameObject _lavelUPUIobj;

    //버튼들이 있는 스크립트
    public LevelUpItem[] _levelUpItem;
    //아이템 이벤트들이 담긴 스크립트( 무기들의 기본 스텟 , 버튼 누를시 발동되는 함수)
    public ItemEvent _itemEvent;

    //나중에 옮길 코드
    public List<Item> items;

    public TextAsset _itemsjson;

    float[] _itemChances = new float[] { 10, 10, 10, 10, 10, 10, 10 };

    public bool isItemFullUP() => _itemEvent.IsItemLankFull();

    private void Awake()
    {
        gold = new Item();
        Heal = new Item();
        gold.name = "Gold";
        gold.description = new string[] { "getgold" };

        Heal.name = "Heal";
        Heal.description = new string[] { "Heal" };


    }

    private void OnEnable()
    {
        
    }   

    // Start is called before the first frame update
    void Start()
    {

    }

    public void LevelUP()
    {
        _lavelUPUIobj.SetActive(true);
        List<int> Numbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6 ,7,8,9,10,11,12,13};        
        int Count = 0;
        

        int num = 0;        

        
        //무기가 6개가 아닐경우 
        //7개의 무기중 혹시 강화 다된 무기는 제외
        //SettingList(ref Numbers);
        Numbers = FilterAvailableItems(Numbers);

        if (_itemEvent.IsItemLankFull())
        {
            Count = 3;
        }
        /* 구버전 코드
         * //아이템이 꽉찼을 경우 
        else if (_itemEvent.IsWeaponFull() && _itemEvent.IsAccesoryFull())
        {
            //무기중에서 강화 다된무기는 제외하고 가져오기
            FullItemNumber = _itemEvent.GetWeaponNumber();
            FullItemNumber.AddRange(_itemEvent.GetAccesoryNumber());
            //무기를 6개 가지고 있지만 
            //강화를 못한 아이템의 수가 3이상이면 3 
            //아이템들이 점부 강화 되었는지 확인하고 만약 전부 강화 안되었다면 
            Count = FullItemNumber.Count >= 3 ? 3 : FullItemNumber.Count <= 2 ? FullItemNumber.Count : 3;

        } */       
        else
        {
            //무기를 5개를 가지고있고 전부 강화 했다면 남은 아이템은 2개여야됨.

            Count = Numbers.Count >= 3 ? 3 : Numbers.Count;
        }
        Debug.Log("넘버즈  카운트  : " + Count);

        for (int i = 0; i < Count; i++)
        {
            //나중에 아이템이 곂쳐서 나오지 않도록 조치 해야됨. 2024-07-07

            _levelUpItem[i].gameObject.SetActive(true);

            //이미 아이템이 다 차있고 랭크도 전부 최대일경우
            if (_itemEvent.IsItemLankFull())
            {
                Debug.Log("혹시 오브젝트 문제인지 확인 " +i  +" " + _levelUpItem[i]);
                int randint = Random.Range(14, 16);
                Item _endItem = PlayManager.Instance.Items[randint];                
                Debug.Log("아이템 다차있는 상태에서 랜덤 번호 : " + randint);
                _levelUpItem[i].Getitem(_endItem, GetAction(randint));
                continue;
            }


            /* 과거 코드
             * //아이템이 꽉찼을 경우
            if (_itemEvent.IsWeaponFull() && _itemEvent.IsAccesoryFull())
            {
                Debug.Log(FullItemNumber.Count + " 아이템이 꽉찼을 경우 아이템 개수 " +
                    Count + "현재 반복문 이 몇번 반복해야하는지 확인");
                num = ChooseInt(ref FullItemNumber);

            }
            else
            {

            }*/
            num = ChooseInt(ref Numbers);


            //int num = Choose(_itemChances); // 랜덤 확률로 똑같은 수가 나올 수 있어서 수정함.
            Debug.Log(num + " num 확인");
            Item _item = PlayManager.Instance.Items[num];
            UnityAction ev = GetAction(num);
            _levelUpItem[i].Getitem(_item, GetAction(num));
        }

    }

    //필터링시킨 아이템번호
    List<int> FilterAvailableItems(List<int> Numbers)
    {
        List<int> filteredItems = new List<int>();

        List<int> checkWeapon = _itemEvent.GetWeaponNumber();
        List<int> checkAccssory = _itemEvent.GetAccesoryNumber();

        foreach (int itemID in Numbers)
        {
            //해당타입이 무엇인지
            //혹시 무기나 악세서리 개수가 다 차있는지 확인
            bool isWeapon = _itemEvent.istypeWeapon(itemID);
            bool isWeaponFull = _itemEvent.IsWeaponFull();
            bool isAccessoryFull = _itemEvent.IsAccesoryFull();
            bool isitemFullUP = _itemEvent.IsItemFullUp(itemID);

            // 무기와 악세서리가 최대 갯수에 도달했는지 확인
            //무기인지 확인 그리고 무기가 6개인지 확인하고 
            //아이템이 강화가 다안되어있는지 확인 만약 강화가 안되어있다면 줘야하니깐.
           

            //무기나 악세서리가 꽉차있는 경우
            //무기나 악세서리가 장착하고 있는 것인지 확인.
            //위의 if문에서 아이템 강화는 체크 했음.
            if(((isWeaponFull && checkWeapon.Contains(itemID))
                || (isWeaponFull && checkAccssory.Contains(itemID)))
                 && !isitemFullUP)
            {
                filteredItems.Add(itemID);

            }else if((!isAccessoryFull&& !isitemFullUP && !isWeapon)
                || (!isWeaponFull  && !isitemFullUP&& isWeapon))
            {
                //악세서리가 다 안차있고 장착 하고 있는 악세서리인지 확인

                //무기가 다 안차있고 장착 하고 있는 무기인지 확인

                filteredItems.Add(itemID);

            }
            else
            {
                Debug.Log("isWeapon / isWeaponFull / isAccessoryFull / isitemFullUP  / itemID" +
                   isWeapon + " /  " + isWeaponFull + " /  " + isAccessoryFull + " /  " + isitemFullUP + " /  "
                   + itemID);

            }
        }

        if(filteredItems.Count < 0) 
        {

        }



        return filteredItems;
    }

    //불필요한 아이템 정리
    void SettingList(ref List<int> ints)
    {
        for (int i = 0; i < ints.Count; i++)
        {
            //만약 무기랑 악세서리 둘다 6개가 아님
            //근대 지금 줄려고하는 아이템이 만약 강화 완료되있는 상태라면 지우기
            if (_itemEvent.IsItemFullUp(ints[i]) == true)
            {
                ints.Remove(ints[i]);
                continue;
            }

            //아이템 다 찼음
            //해당 아이템이 지금 장착하고 있는 아이템중 하나라면!!

            //이유발견 else if를 사용해서 장신구를 다찼는지 확인하다보니
            //먼저 6개가 된 아이템에서만 삭제가 되는것
            if (_itemEvent.IsWeaponFull() == true) // 6개 이상
            {
                //여기서 장비 아이템인지확인필요 할듯함.
                if (_itemEvent.istypeWeapon(ints[i]) == true)
                {
                    //아이템 장비 중임.
                    //그런데 아이템이 강화 끝난상태라면?//그때는 삭제 
                    //혹은 아이템이 꽉차있는데 그곳에 있는 아이템이 아니라면? 삭제                    
                    List<int> list = _itemEvent.GetWeaponNumber();
                    if ((list.Contains(ints[i]) && _itemEvent.IsItemFullUp(ints[i]) == true)
                          || list.Contains(ints[i]) == false)
                    {
                        Debug.Log("아이템이 풀업인지 확인 해당 번호 : " + ints[i]);

                        ints.Remove(ints[i]);
                        //삭제하고 contine를 안하면 바로 다음 숫자 즉 0 1 2 에서 1을 빼면 2가 1의 위치로 내려옴
                        continue;
                    }
                }
                //무기가 아니므로 넘어감
                          
            }

            //장신구 다 찼음
            //해당 아이템이 지금 장착하고 있는 아이템중 하나라면!!
            if (_itemEvent.IsAccesoryFull() == true)
            {
                if (_itemEvent.istypeWeapon(ints[i]) == false)
                {
                    //아이템 장비 중임.
                    //그런데 아이템이 강화 끝난상태라면?//그때는 삭제
                    List<int> list = _itemEvent.GetAccesoryNumber();
                    if ((list.Contains(ints[i]) && _itemEvent.IsItemFullUp(ints[i]) == true)
                          || list.Contains(ints[i]) == false)
                    {
                        Debug.Log("아이템이 풀업인지 확인 해당 번호 : " + ints[i]);

                        ints.Remove(ints[i]);
                        continue;
                    }
                }
            }   
        }
    }

    int ChooseInt(ref List<int> ints)
    {
        //리스트에서 해당 무기들 중 하나 고르고 다음 선택지에서는 해당 무기는 안나오도록함.        

        int number = Random.Range(0, ints.Count);        
        int element = ints[number];
        ints.RemoveAt(number);
        


        return element;
    }

    public UnityAction GetAction(int num)
    {
        //현재는 무기가 랜덤으로 나오는 상황 즉 같은 무기가 나올수도있고 무기를 다업그레이드 해도 나올수 있음.               



        switch (num)
        {
            case 0:
                return _itemEvent.LavelUPSword;
                
            case 1:
                return _itemEvent.LavelUpDefender;

            case 2:
                return _itemEvent.LavelUpDrill;

            case 3:
                return _itemEvent.LavelUpBarrier;

            case 4:
                return _itemEvent.LavelUpRocket;

            case 5:
                return _itemEvent.LavelUpFirebomb;

            case 6:
                return _itemEvent.LavelUpLightning;
            case 7:
                return _itemEvent.LavelUpMagnet;
            case 8:
                return _itemEvent.LavelUpBook;
            case 9:
                return _itemEvent.LevelUpDamage;
            case 10:
                return _itemEvent.LavelUpMilk;
            case 11:
                return _itemEvent.LavelUpMeat;
            case 12:
                return _itemEvent.LavelUpMagazine;

            case 13:
                return _itemEvent.LavelUpOil;
            case 14:
                 return _itemEvent.GetGold;
            case 15:
                 return _itemEvent.GetHeal;

            default:
                break;
        }

        return null;
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
        return probs.Length -1;
    }

    public void LevelUpEnd()
    {
        for (int i = 0; i < _levelUpItem.Length; i++)
        {
            _levelUpItem[i].gameObject.SetActive(false);
        }
        _lavelUPUIobj.SetActive(false);
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
