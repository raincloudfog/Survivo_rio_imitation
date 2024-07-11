using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    public int _killMonsterCount;

    public TMP_Text _killMonsterCountText;

    public int _GetCoinCount;
    public TMP_Text _GetCoinCountText;


    //����ġ ǥ�� ������Ʈ
    public GameObject[] _expObject;

    public TMP_Text _levelTxt;


    public void KillMonster()
    {
        _killMonsterCount++;
        _killMonsterCountText.text = _killMonsterCount.ToString();
    }

    public void GetCoin(int coin)
    {
        _GetCoinCount += coin;
        _killMonsterCountText.text = _GetCoinCount.ToString();
    }

    public void ExpObjectOn(int length)
    {       

        for (int i = 0; i < length; i++)
        {
                _expObject[i].gameObject.SetActive(true);            
        }


        /*Debug.Log("����ġ �� ���� �Լ�");
        for (int i = 0; i < _expObject.Length; i++)
        {
            if(_expObject[i].gameObject.activeSelf == false)
            {
                Debug.Log("����ġ �� ��ĭ ����");
                _expObject[i].gameObject.SetActive(true);
                return;
            }
            
        }

        LevelObjOff();*/
    }

    public void LevelUp()
    {
        for (int i = 0; i < _expObject.Length; i++)
        {
            _expObject[i].gameObject.SetActive(true);

        }
    }

    public void LevelObjOff()
    {
        for (int i = 0; i < _expObject.Length; i++)
        {
            _expObject[i].gameObject.SetActive(false);

        }
    }
}
