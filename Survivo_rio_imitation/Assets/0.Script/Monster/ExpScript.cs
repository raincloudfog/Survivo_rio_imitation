using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpScript : MonoBehaviour
{
    
    [SerializeField]private int _originExp,_exp;

    private EndObj _endObj;
    private BoxCollider2D _boxCollider;

    public int GetExp
    {
        get
        {
            return _exp;
        }
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void LevelUpExp(int level)
    {
        _exp = _originExp + (int)(_originExp * (level * 0.1));
    }

    public void Init(int level,EndObj newevent)
    {
        _boxCollider.size =  Vector2.one * PlayManager.Instance.Magnet;
        _exp = _originExp + (int)(_originExp * (level * 0.1));
        _endObj = null;
        _endObj += newevent;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().AddExp(_exp);
            _endObj?.Invoke();
        }
    }    

}
