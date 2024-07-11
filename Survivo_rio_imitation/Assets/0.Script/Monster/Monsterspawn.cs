using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsterspawn : MonoBehaviour
{
    /*
        ���ʹ� ȭ�� �� �������� �����ϰ� ��ȯ��
        �Ǵٸ� ��ȯ��
        �������εѷ��μ� ��ȯ��
        �׸��� �Ѹ����� ��ȯ�Ǵ°Ծƴ϶� ���ڸ� ���ؼ� ��ȯ��

        �׸��� �̷����̺�Ʈ�� �����ʰ� �׳� ���������� �������� ��ȯ�Ǵ°� ������ ������.
     */
    
    public GameObject _monsterPrefab;
    public int _spawnCount = 10;

    public MonsterSpawnEvent.SpawnType _type;

    private void SpawnMonster()
    {
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < _spawnCount; i++)
        {
            switch (_type)
            {
                case MonsterSpawnEvent.SpawnType.RandomOutsideScreen:
                    spawnPosition = GetRandomOutsideScreenPosition();
                    break;
                case MonsterSpawnEvent.SpawnType.Circular:
                    spawnPosition = GetRandomCircularPosition();
                    break;
                default:
                    break;
            }

            Instantiate(_monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomOutsideScreenPosition()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        float randomX = Random.Range(-cameraWidth, cameraWidth) / 2;
        float randomY = Random.Range(-cameraHeight, cameraHeight) / 2;

        if (Random.Range(0, 2) == 0)
        {
            randomX = randomX < 0 ? randomX - mainCamera.orthographicSize :
                randomX + mainCamera.orthographicSize;
        }
        else
        {
            randomY = randomY < 0 ? randomX - mainCamera.orthographicSize :
                randomY + mainCamera.orthographicSize;
        }

        return new Vector3(randomX, randomY, 0);
    }

    private Vector3 GetRandomCircularPosition()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        float radius = cameraWidth - 0.5f; // ���� ������
        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        return new Vector3(x, y, 0);
    }
}
