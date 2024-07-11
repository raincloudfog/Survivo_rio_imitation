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

    //�ش� �Լ��� ������ �ִ� 
    public GameObject _lavelUPUIobj;

    //��ư���� �ִ� ��ũ��Ʈ
    public LevelUpItem[] _levelUpItem;
    //������ �̺�Ʈ���� ��� ��ũ��Ʈ( ������� �⺻ ���� , ��ư ������ �ߵ��Ǵ� �Լ�)
    public ItemEvent _itemEvent;

    //���߿� �ű� �ڵ�
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

        
        //���Ⱑ 6���� �ƴҰ�� 
        //7���� ������ Ȥ�� ��ȭ �ٵ� ����� ����
        //SettingList(ref Numbers);
        Numbers = FilterAvailableItems(Numbers);

        if (_itemEvent.IsItemLankFull())
        {
            Count = 3;
        }
        /* ������ �ڵ�
         * //�������� ��á�� ��� 
        else if (_itemEvent.IsWeaponFull() && _itemEvent.IsAccesoryFull())
        {
            //�����߿��� ��ȭ �ٵȹ���� �����ϰ� ��������
            FullItemNumber = _itemEvent.GetWeaponNumber();
            FullItemNumber.AddRange(_itemEvent.GetAccesoryNumber());
            //���⸦ 6�� ������ ������ 
            //��ȭ�� ���� �������� ���� 3�̻��̸� 3 
            //�����۵��� ���� ��ȭ �Ǿ����� Ȯ���ϰ� ���� ���� ��ȭ �ȵǾ��ٸ� 
            Count = FullItemNumber.Count >= 3 ? 3 : FullItemNumber.Count <= 2 ? FullItemNumber.Count : 3;

        } */       
        else
        {
            //���⸦ 5���� �������ְ� ���� ��ȭ �ߴٸ� ���� �������� 2�����ߵ�.

            Count = Numbers.Count >= 3 ? 3 : Numbers.Count;
        }
        Debug.Log("�ѹ���  ī��Ʈ  : " + Count);

        for (int i = 0; i < Count; i++)
        {
            //���߿� �������� ���ļ� ������ �ʵ��� ��ġ �ؾߵ�. 2024-07-07

            _levelUpItem[i].gameObject.SetActive(true);

            //�̹� �������� �� ���ְ� ��ũ�� ���� �ִ��ϰ��
            if (_itemEvent.IsItemLankFull())
            {
                Debug.Log("Ȥ�� ������Ʈ �������� Ȯ�� " +i  +" " + _levelUpItem[i]);
                int randint = Random.Range(14, 16);
                Item _endItem = PlayManager.Instance.Items[randint];                
                Debug.Log("������ �����ִ� ���¿��� ���� ��ȣ : " + randint);
                _levelUpItem[i].Getitem(_endItem, GetAction(randint));
                continue;
            }


            /* ���� �ڵ�
             * //�������� ��á�� ���
            if (_itemEvent.IsWeaponFull() && _itemEvent.IsAccesoryFull())
            {
                Debug.Log(FullItemNumber.Count + " �������� ��á�� ��� ������ ���� " +
                    Count + "���� �ݺ��� �� ��� �ݺ��ؾ��ϴ��� Ȯ��");
                num = ChooseInt(ref FullItemNumber);

            }
            else
            {

            }*/
            num = ChooseInt(ref Numbers);


            //int num = Choose(_itemChances); // ���� Ȯ���� �Ȱ��� ���� ���� �� �־ ������.
            Debug.Log(num + " num Ȯ��");
            Item _item = PlayManager.Instance.Items[num];
            UnityAction ev = GetAction(num);
            _levelUpItem[i].Getitem(_item, GetAction(num));
        }

    }

    //���͸���Ų �����۹�ȣ
    List<int> FilterAvailableItems(List<int> Numbers)
    {
        List<int> filteredItems = new List<int>();

        List<int> checkWeapon = _itemEvent.GetWeaponNumber();
        List<int> checkAccssory = _itemEvent.GetAccesoryNumber();

        foreach (int itemID in Numbers)
        {
            //�ش�Ÿ���� ��������
            //Ȥ�� ���⳪ �Ǽ����� ������ �� ���ִ��� Ȯ��
            bool isWeapon = _itemEvent.istypeWeapon(itemID);
            bool isWeaponFull = _itemEvent.IsWeaponFull();
            bool isAccessoryFull = _itemEvent.IsAccesoryFull();
            bool isitemFullUP = _itemEvent.IsItemFullUp(itemID);

            // ����� �Ǽ������� �ִ� ������ �����ߴ��� Ȯ��
            //�������� Ȯ�� �׸��� ���Ⱑ 6������ Ȯ���ϰ� 
            //�������� ��ȭ�� �پȵǾ��ִ��� Ȯ�� ���� ��ȭ�� �ȵǾ��ִٸ� ����ϴϱ�.
           

            //���⳪ �Ǽ������� �����ִ� ���
            //���⳪ �Ǽ������� �����ϰ� �ִ� ������ Ȯ��.
            //���� if������ ������ ��ȭ�� üũ ����.
            if(((isWeaponFull && checkWeapon.Contains(itemID))
                || (isWeaponFull && checkAccssory.Contains(itemID)))
                 && !isitemFullUP)
            {
                filteredItems.Add(itemID);

            }else if((!isAccessoryFull&& !isitemFullUP && !isWeapon)
                || (!isWeaponFull  && !isitemFullUP&& isWeapon))
            {
                //�Ǽ������� �� �����ְ� ���� �ϰ� �ִ� �Ǽ��������� Ȯ��

                //���Ⱑ �� �����ְ� ���� �ϰ� �ִ� �������� Ȯ��

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

    //���ʿ��� ������ ����
    void SettingList(ref List<int> ints)
    {
        for (int i = 0; i < ints.Count; i++)
        {
            //���� ����� �Ǽ����� �Ѵ� 6���� �ƴ�
            //�ٴ� ���� �ٷ����ϴ� �������� ���� ��ȭ �Ϸ���ִ� ���¶�� �����
            if (_itemEvent.IsItemFullUp(ints[i]) == true)
            {
                ints.Remove(ints[i]);
                continue;
            }

            //������ �� á��
            //�ش� �������� ���� �����ϰ� �ִ� �������� �ϳ����!!

            //�����߰� else if�� ����ؼ� ��ű��� ��á���� Ȯ���ϴٺ���
            //���� 6���� �� �����ۿ����� ������ �Ǵ°�
            if (_itemEvent.IsWeaponFull() == true) // 6�� �̻�
            {
                //���⼭ ��� ����������Ȯ���ʿ� �ҵ���.
                if (_itemEvent.istypeWeapon(ints[i]) == true)
                {
                    //������ ��� ����.
                    //�׷��� �������� ��ȭ �������¶��?//�׶��� ���� 
                    //Ȥ�� �������� �����ִµ� �װ��� �ִ� �������� �ƴ϶��? ����                    
                    List<int> list = _itemEvent.GetWeaponNumber();
                    if ((list.Contains(ints[i]) && _itemEvent.IsItemFullUp(ints[i]) == true)
                          || list.Contains(ints[i]) == false)
                    {
                        Debug.Log("�������� Ǯ������ Ȯ�� �ش� ��ȣ : " + ints[i]);

                        ints.Remove(ints[i]);
                        //�����ϰ� contine�� ���ϸ� �ٷ� ���� ���� �� 0 1 2 ���� 1�� ���� 2�� 1�� ��ġ�� ������
                        continue;
                    }
                }
                //���Ⱑ �ƴϹǷ� �Ѿ
                          
            }

            //��ű� �� á��
            //�ش� �������� ���� �����ϰ� �ִ� �������� �ϳ����!!
            if (_itemEvent.IsAccesoryFull() == true)
            {
                if (_itemEvent.istypeWeapon(ints[i]) == false)
                {
                    //������ ��� ����.
                    //�׷��� �������� ��ȭ �������¶��?//�׶��� ����
                    List<int> list = _itemEvent.GetAccesoryNumber();
                    if ((list.Contains(ints[i]) && _itemEvent.IsItemFullUp(ints[i]) == true)
                          || list.Contains(ints[i]) == false)
                    {
                        Debug.Log("�������� Ǯ������ Ȯ�� �ش� ��ȣ : " + ints[i]);

                        ints.Remove(ints[i]);
                        continue;
                    }
                }
            }   
        }
    }

    int ChooseInt(ref List<int> ints)
    {
        //����Ʈ���� �ش� ����� �� �ϳ� ���� ���� ������������ �ش� ����� �ȳ���������.        

        int number = Random.Range(0, ints.Count);        
        int element = ints[number];
        ints.RemoveAt(number);
        


        return element;
    }

    public UnityAction GetAction(int num)
    {
        //����� ���Ⱑ �������� ������ ��Ȳ �� ���� ���Ⱑ ���ü����ְ� ���⸦ �پ��׷��̵� �ص� ���ü� ����.               



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
