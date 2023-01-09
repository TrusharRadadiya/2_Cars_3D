using UnityEngine;
using DG.Tweening;

public class CarMovement : MonoBehaviour
{
    public CarSide carSide;
    public bool isRight = false;
    [SerializeField] private float _posToUpdate = 1.1f;
    [SerializeField] private float _degreeToRotate = 35f;
    [SerializeField] private MeshRenderer _carBodyRenderer;
    [SerializeField] private Color _boostColor;
    [SerializeField] private CameraShake _cameraShake;
    private bool _canInput = true;
    private Transform _thisTransform;
    public bool IsBoosted { get; set; }

    public void Awake()
    {
        _thisTransform = transform;
    }

    public void Update()
    {
        if (_canInput)
        {
            var pos = _thisTransform.position;

            if (Input.GetMouseButtonUp(0))
            {
                if ((carSide == CarSide.Left && Input.mousePosition.x < Screen.width * .5f) ||
                    (carSide == CarSide.Right && Input.mousePosition.x > Screen.width * .5f))
                {
                    if (!isRight)
                    {
                        isRight = true;
                        _canInput = false;
                        DOTween.Sequence()
                            .Append(_thisTransform.DOLocalMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                            .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                            .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                            .AppendCallback(() => {
                                _canInput = true;
                                _cameraShake.Shake();
                            });
                    }
                    else if (isRight)
                    {
                        isRight = false;
                        _canInput = false;
                        DOTween.Sequence()
                            .Append(_thisTransform.DOLocalMoveX(pos.x - _posToUpdate, .15f).SetEase(Ease.InOutSine))
                            .Join(_thisTransform.DORotate(new Vector3(0, -_degreeToRotate, 0), .1f))
                            .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                            .AppendCallback(() => {
                                _canInput = true;
                                _cameraShake.Shake();
                            });
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isRight)
            {
                isRight = true;
                _canInput = false;
                DOTween.Sequence()
                    .Append(_thisTransform.DOLocalMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                    .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                    .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                    .AppendCallback(() => {
                        _canInput = true;
                        _cameraShake.Shake();
                    });
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && isRight)
            {
                isRight = false;
                _canInput = false;
                DOTween.Sequence()
                    .Append(_thisTransform.DOLocalMoveX(pos.x - _posToUpdate, .15f).SetEase(Ease.InOutSine))
                    .Join(_thisTransform.DORotate(new Vector3(0, -_degreeToRotate, 0), .1f))
                    .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                    .AppendCallback(() => {
                        _canInput = true;
                        _cameraShake.Shake();
                    });
            }
#endif
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsBoosted = true;
            
            DOTween.Sequence()
                .Append(_thisTransform.DOLocalMoveZ(2, .15f).SetEase(Ease.InOutFlash))
                .Append(_thisTransform.DOLocalMoveZ(0, .15f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoadObject"))
        {
            Destroy(other.gameObject);

            DOTween.Sequence()
                .Append(_thisTransform.DOScale(.5f, .15f))
                .Append(_thisTransform.DOScale(.4f, .15f).SetEase(Ease.InOutBounce));
        }
    }
}

public enum CarSide
{
    Left, Right
}