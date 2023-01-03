using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private Rigidbody _rb;
    protected Transform _thisTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _thisTransform = transform;
    }

    private void Update()
    {
        var posToMove = _thisTransform.position + (Vector3.forward * _speed * Time.deltaTime);
        _rb.MovePosition(posToMove);
    }
}
