using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;
    protected Transform _thisTransform;
    private bool _canMove = false;

    private void Awake()
    {
        _thisTransform = transform;
    }

    private void Update()
    {
        if (!_canMove) return;
        _thisTransform.position += Vector3.forward * _speed * Time.deltaTime;
    }

    public void SetMovement(bool shouldMove)
    {
        _canMove = shouldMove;
    }
}
