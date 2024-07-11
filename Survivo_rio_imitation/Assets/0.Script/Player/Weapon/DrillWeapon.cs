using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDrill
{
    public void EnterDrill(Vector3 pos);
}

public class DrillWeapon : BasicWeapon
{

    /* 설명
     탕탕 특공대의 드릴 뱀파이어 서바이버의 룬트레이서를 모작 한 무기
    벽에 닿았을경우 튕기는 성질을 가지고 있음

    여기서 이제 생각해낸 방식
    1. 플레이어 자식오브젝트 혹은 따라다니게 하는 스크립트로 자동으로 움직이는 투명 오브젝트들을 만들어서
    해상도 크기만큼 만들어놨으니 편할 듯함

    2. 해상도 크기를 항상 체크하여 그 범위 이상으로는 못나가게 하기
    항상 해상도 크기를 체크하지만 만약 내가 범위를 크게 만들어놨다면?
     */

    public Vector2 minDir , maxDir;
    public Vector3 _moveDirection;

    private float minX, maxX, minY, maxY;

    public float _moveSpeed = 5;

    public void Start()
    {
        UpdateResolutionBounds();
        SetRandomDirection();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            Monster enemy = collision.GetComponent<Monster>();
            if (enemy != null)
            {
                enemy.OnHit(_damage);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        UpdateResolutionBounds();

        // 현재 위치 계산
        Vector3 currentPosition = transform.position;

        // 새로운 위치 계산
        Vector3 newPosition = currentPosition + _moveDirection * Time.deltaTime * _moveSpeed;

        //Debug.Log("테스트 벡터 : " + newPosition);
        // 위치 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        

        // 벽에 닿으면 반사
        if (newPosition.x <= minX || newPosition.x >= maxX)
        {
            ReflectDirection(Vector3.up);
        }
        if (newPosition.y <= minY || newPosition.y >= maxY)
        {
            ReflectDirection(Vector3.right);
        }

        //방향 회전 // 2024 07 06
        float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));

        // 위치 설정
        transform.position = newPosition;
    }

    void SetRandomDirection()
    {

        float randomAngle = Random.Range(0f, 360f);
        _moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
        _moveDirection = _moveDirection.normalized; // 방향 벡터를 정규화하여 방향만 유지

    }

    //방향 전환
    void ReflectDirection(Vector3 normal)
    {
        /* //예측하여 반사방향 계산
         Vector3 reflectDir = Vector3.zero;

         if (transform.position.x <= minX || transform.position.x >= maxX)
         {
             reflectDir.x = -inputDirection.x;
         }

         if(transform.position.y <= minY || transform.position.y >= maxY)
         {
             reflectDir.y = -inputDirection.y;
         }

         // 방향 벡터를 정규화하여 방향만 유지
         _moveDirection = reflectDir.normalized;*/

        //2번째 버전
        //_moveDirection = Vector3.Reflect(_moveDirection, normal).normalized;

        //3번째 버전 수학 공식 이용 이건 검색해서 얻은 결과
        _moveDirection = _moveDirection - 2 * Vector3.Dot(_moveDirection, normal) * normal;
        _moveDirection = _moveDirection.normalized;

    }


    //해상도 크기 제한 변경
    void UpdateResolutionBounds()
    {
        //현재 해상도의 viewport 크기 구하기
        Camera mainCamera = Camera.main;

        if(mainCamera != null )
        {
            //구버전
            /*//Viewport의 좌표를 World 좌표로 변환
            Vector3 lowerLeft = mainCamera.ViewportToScreenPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 upperRight = mainCamera.ViewportToScreenPoint(new Vector3(1, 1, mainCamera.nearClipPlane));*/

            // 카메라가 비추는 실제 화면 영역 계산
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;

            //해상도 범위 업데이트
            //구버전
            /*minX = lowerLeft.x;
            maxX = upperRight.x;
            minY = lowerLeft.y;
            maxY = upperRight.y;
*/

            // 해상도 범위 업데이트
            minX = mainCamera.transform.position.x - cameraWidth / 2;
            maxX = mainCamera.transform.position.x + cameraWidth / 2;
            minY = mainCamera.transform.position.y - mainCamera.orthographicSize;
            maxY = mainCamera.transform.position.y + mainCamera.orthographicSize;

            // 현재 위치가 해상도를 벗어나면 방향 반사
            if (transform.position.x <= minX || transform.position.x >= maxX
                || transform.position.y <= minY || transform.position.y >= maxY)
            {
                ReflectDirection(_moveDirection);
            }

            minDir = new Vector2(minX, minY);
            maxDir = new Vector2(maxX, maxY);

        }
    }
}
