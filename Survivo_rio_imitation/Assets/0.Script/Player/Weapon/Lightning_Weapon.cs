using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

interface ILightning
{
    public void AcvtiveWeapon();
}

public class Lightning_Weapon : BasicWeapon, ILightning
{
    //번개 이펙트
    public GameObject _effect;
    [SerializeField] float _transparenctyspeed = 5;
    [SerializeField] float elapsedTime = 0;


    public void AcvtiveWeapon()
    {
        StartCoroutine(Active());
    }   

    IEnumerator Active() 
    {


        SpriteRenderer _effecttransparency = _effect.GetComponent<SpriteRenderer>();

        Color effectcollor = _effecttransparency.color;
        Color targetColor = new Color(effectcollor.r, effectcollor.g, effectcollor.b, 1);
        _effect.SetActive(true);
        
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * _transparenctyspeed;

            float t = Mathf.Clamp01(elapsedTime / 1);
            _effecttransparency.color =  new Color(1,1,1, Mathf.Lerp(0,1,t * t));

            /*Debug.Log(elapsedTime / 255);
            effectcollor.a = elapsedTime /255;
            _effecttransparency.color = effectcollor;*/

            yield return null;
        }

        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D collider in enemys)
        {
            Monster enemy = collider.GetComponent<Monster>();
            if (enemy != null)
            {
                //Debug.Log("데미지 들어가는 숫자 확인");
                enemy.OnHit(_damage);
            }
        }
        yield return new WaitForSeconds(0.35f);

        elapsedTime = 0;
        _effect.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(Active());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, 0.2f);

    }
}
