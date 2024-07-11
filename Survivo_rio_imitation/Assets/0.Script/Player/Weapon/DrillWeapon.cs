using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDrill
{
    public void EnterDrill(Vector3 pos);
}

public class DrillWeapon : BasicWeapon
{

    /* ����
     ���� Ư������ �帱 �����̾� �����̹��� ��Ʈ���̼��� ���� �� ����
    ���� �������� ƨ��� ������ ������ ����

    ���⼭ ���� �����س� ���
    1. �÷��̾� �ڽĿ�����Ʈ Ȥ�� ����ٴϰ� �ϴ� ��ũ��Ʈ�� �ڵ����� �����̴� ���� ������Ʈ���� ����
    �ػ� ũ�⸸ŭ ���������� ���� ����

    2. �ػ� ũ�⸦ �׻� üũ�Ͽ� �� ���� �̻����δ� �������� �ϱ�
    �׻� �ػ� ũ�⸦ üũ������ ���� ���� ������ ũ�� �������ٸ�?
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

        // ���� ��ġ ���
        Vector3 currentPosition = transform.position;

        // ���ο� ��ġ ���
        Vector3 newPosition = currentPosition + _moveDirection * Time.deltaTime * _moveSpeed;

        //Debug.Log("�׽�Ʈ ���� : " + newPosition);
        // ��ġ ����
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        

        // ���� ������ �ݻ�
        if (newPosition.x <= minX || newPosition.x >= maxX)
        {
            ReflectDirection(Vector3.up);
        }
        if (newPosition.y <= minY || newPosition.y >= maxY)
        {
            ReflectDirection(Vector3.right);
        }

        //���� ȸ�� // 2024 07 06
        float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));

        // ��ġ ����
        transform.position = newPosition;
    }

    void SetRandomDirection()
    {

        float randomAngle = Random.Range(0f, 360f);
        _moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
        _moveDirection = _moveDirection.normalized; // ���� ���͸� ����ȭ�Ͽ� ���⸸ ����

    }

    //���� ��ȯ
    void ReflectDirection(Vector3 normal)
    {
        /* //�����Ͽ� �ݻ���� ���
         Vector3 reflectDir = Vector3.zero;

         if (transform.position.x <= minX || transform.position.x >= maxX)
         {
             reflectDir.x = -inputDirection.x;
         }

         if(transform.position.y <= minY || transform.position.y >= maxY)
         {
             reflectDir.y = -inputDirection.y;
         }

         // ���� ���͸� ����ȭ�Ͽ� ���⸸ ����
         _moveDirection = reflectDir.normalized;*/

        //2��° ����
        //_moveDirection = Vector3.Reflect(_moveDirection, normal).normalized;

        //3��° ���� ���� ���� �̿� �̰� �˻��ؼ� ���� ���
        _moveDirection = _moveDirection - 2 * Vector3.Dot(_moveDirection, normal) * normal;
        _moveDirection = _moveDirection.normalized;

    }


    //�ػ� ũ�� ���� ����
    void UpdateResolutionBounds()
    {
        //���� �ػ��� viewport ũ�� ���ϱ�
        Camera mainCamera = Camera.main;

        if(mainCamera != null )
        {
            //������
            /*//Viewport�� ��ǥ�� World ��ǥ�� ��ȯ
            Vector3 lowerLeft = mainCamera.ViewportToScreenPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 upperRight = mainCamera.ViewportToScreenPoint(new Vector3(1, 1, mainCamera.nearClipPlane));*/

            // ī�޶� ���ߴ� ���� ȭ�� ���� ���
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;

            //�ػ� ���� ������Ʈ
            //������
            /*minX = lowerLeft.x;
            maxX = upperRight.x;
            minY = lowerLeft.y;
            maxY = upperRight.y;
*/

            // �ػ� ���� ������Ʈ
            minX = mainCamera.transform.position.x - cameraWidth / 2;
            maxX = mainCamera.transform.position.x + cameraWidth / 2;
            minY = mainCamera.transform.position.y - mainCamera.orthographicSize;
            maxY = mainCamera.transform.position.y + mainCamera.orthographicSize;

            // ���� ��ġ�� �ػ󵵸� ����� ���� �ݻ�
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
