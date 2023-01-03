using UnityEngine;

public class CameraFollowMovement : Movement
{
    [SerializeField] private Transform _cam;
    private Vector3 _offset;

    private void Start()
    {
        _offset = _cam.position - _thisTransform.position;
    }

    private void LateUpdate()
    {
        var camPos = _cam.position;
        camPos.z = _thisTransform.position.z + _offset.z;
        _cam.position = camPos;
    }
}
