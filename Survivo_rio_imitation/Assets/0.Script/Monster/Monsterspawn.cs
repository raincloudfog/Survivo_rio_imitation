using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsterspawn : MonoBehaviour
{
    /*
        몬스터는 화면 밖 범위에서 랜덤하게 소환됨
        또다른 소환은
        원형으로둘러싸서 소환됨
        그리고 한마리만 소환되는게아니라 숫자를 정해서 소환됨

        그리고 이렇게이벤트를 넣지않고도 그냥 정기적으로 랜덤으로 소환되는게 있으면 좋겠음.
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

        float radius = cameraWidth - 0.5f; // 원형 반지름
        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        return new Vector3(x, y, 0);
    }
}
