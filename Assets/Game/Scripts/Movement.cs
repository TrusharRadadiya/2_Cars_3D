using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;
    protected Transform _thisTransform;

    public virtual void Awake()
    {
        _thisTransform = transform;
    }

    public void Update()
    {
        _thisTransform.position += Vector3.forward * _speed * Time.deltaTime;
    }
}
