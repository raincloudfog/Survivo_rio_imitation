using GameItem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



[Serializable]
public class ItemList
{
    public List<Item> items;
}

public class LevelUpItem : MonoBehaviour
{    
    public Button _thisbutton;

    public Sprite[] _itemimgs;

    public Image _icon;
    public TMP_Text _itemnametxt;
    public TMP_Text _itemexplanationtxt;    
    
    public Item _item;

    //레벨업시 버튼에 랜덤한 아이템 등장 시키기 (텍스트 , 함수 추가)
    public void Getitem(Item item, UnityAction newevent)
    {
        _item = item;
        _itemnametxt.text = _item.name;
        _itemexplanationtxt.text = _item.description[0];
        Debug.Log("_item.id - 1 " + (_item.id - 1));
        _icon.sprite = _itemimgs[_item.id - 1];

        _thisbutton.onClick.RemoveAllListeners();
        _thisbutton.onClick.AddListener(newevent);
        _thisbutton.onClick.AddListener(()=> PlayManager.Instance.LevelUpEnd());
    }

    public void CheckLevelUP()
    {
        _thisbutton.onClick?.Invoke();
    }

}
