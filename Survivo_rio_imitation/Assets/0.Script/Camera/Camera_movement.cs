using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public Transform _player;
    public Camera _camera;

    [SerializeField] float _zoomspeed = 7f;
    [SerializeField] float _size;

    Vector3 offset = new Vector3(0, 0, -10);

    public void Update()
    {
        if(Input.GetKeyDown
            (KeyCode.Keypad1))

        {
            SetCameraSize(_size);
        }
    }


    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    public void SetCameraSize(float size)
    {
        StartCoroutine(CameraSizeChange(size));
    }
    IEnumerator CameraSizeChange(float size)
    {
        float _elapsedTime = 0;


        while (_elapsedTime < 1)
        {
            yield return null;
            _elapsedTime += Time.deltaTime * _zoomspeed;
            float t = Mathf.Clamp01(_elapsedTime / 1);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, size, t * t);

        }
        _camera.orthographicSize = size;
    }

    private void LateUpdate()
    {
        if(_player != null)
        {
            transform.position = _player.transform.position + offset;
        }
    }
}
