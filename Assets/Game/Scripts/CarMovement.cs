using UnityEngine;
using DG.Tweening;

public class CarMovement : Movement
{
    [SerializeField] private CarSide _carSide;
    [SerializeField] private bool _isRight = false;
    [SerializeField] private float _posToUpdate = 1.1f;
    [SerializeField] private float _degreeToRotate = 35f;
    private bool _canInput = true;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        if (_canInput)
        {
            var pos = _thisTransform.position;

            if (Input.GetMouseButtonUp(0))
            {
                if ((_carSide == CarSide.Left && Input.mousePosition.x < Screen.width * .5f) ||
                    (_carSide == CarSide.Right && Input.mousePosition.x > Screen.width * .5f))
                {
                    if (!_isRight)
                    {
                        _isRight = true;
                        _canInput = false;
                        DOTween.Sequence()
                            .Append(_thisTransform.DOMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                            .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                            .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                            .AppendCallback(() => _canInput = true);
                    }
                    else if (_isRight)
                    {
                        _isRight = false;
                        _canInput = false;
                        DOTween.Sequence()
                            .Append(_thisTransform.DOMoveX(pos.x - _posToUpdate, .15f).SetEase(Ease.InOutSine))
                            .Join(_thisTransform.DORotate(new Vector3(0, -_degreeToRotate, 0), .1f))
                            .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                            .AppendCallback(() => _canInput = true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && !_isRight)
            {
                _isRight = true;
                _canInput = false;
                DOTween.Sequence()
                    .Append(_thisTransform.DOMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                    .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                    .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                    .AppendCallback(() => _canInput = true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && _isRight)
            {
                _isRight = false;
                _canInput = false;
                DOTween.Sequence()
                    .Append(_thisTransform.DOMoveX(pos.x - _posToUpdate, .15f).SetEase(Ease.InOutSine))
                    .Join(_thisTransform.DORotate(new Vector3(0, -_degreeToRotate, 0), .1f))
                    .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                    .AppendCallback(() => _canInput = true);
            }
        }

        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoadObject"))
        {
            Destroy(other.gameObject);
        }
    }
}

public enum CarSide
{
    Left, Right
}