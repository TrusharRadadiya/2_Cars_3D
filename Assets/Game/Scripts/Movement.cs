using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;    
    protected Transform _thisTransform;

    public virtual void Awake()
    {
        _thisTransform = transform;
    }

    public virtual void Update()
    {
        _thisTransform.position += Vector3.forward * _speed * Time.deltaTime;
    }
}
