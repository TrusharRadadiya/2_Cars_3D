using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [SerializeField] private UnityEvent _onClick;
    private float _scaleX;

    private void Start()
    {
        _scaleX = transform.localScale.x;
    }

    private void OnMouseDown()
    {
        var scale = new Vector3(_scaleX + .02f, _scaleX + .02f, _scaleX + .02f);
        transform.DOScale(scale, .25f);
    }

    private void OnMouseUp()
    {
        var scale = new Vector3(_scaleX, _scaleX, _scaleX);
        transform.DOScale(scale, .25f).OnComplete(() => _onClick.Invoke());
    }
}
