using UnityEngine;
using DG.Tweening;

public class CarMovement : MonoBehaviour
{
    public CarSide carSide;
    public bool isRight = false;
    [SerializeField] private float _posToUpdate = 1.1f;
    [SerializeField] private float _degreeToRotate = 35f;
    [SerializeField] private MeshRenderer _carBodyRenderer;
    [SerializeField] private Color _saturatedColor;
    [SerializeField] private float _angleForSwipeUp = 45;
    [SerializeField] private GameObject _boostTrails;    
    private bool _canInput = true;
    private Transform _thisTransform;
    public bool IsBoosted { get; set; }
    private Vector2 _startMousePos;
    private Vector2 _endMousePos;

    public void Awake()
    {
        _thisTransform = transform;
    }

    public void Update()
    {
        if (_canInput)
        {
            var pos = _thisTransform.position;

            if (Input.GetMouseButtonDown(0))
            {
                _startMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _endMousePos = Input.mousePosition;

                if ((carSide == CarSide.Left && Input.mousePosition.x < Screen.width * .5f) ||
                    (carSide == CarSide.Right && Input.mousePosition.x > Screen.width * .5f))
                {
                    var distance = _endMousePos - _startMousePos;
                    var angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;                    
                    if (_endMousePos.y > _startMousePos.y && (angle >= 90 - _angleForSwipeUp && angle <= 90 + _angleForSwipeUp))
                    {
                        IsBoosted = true;
                        _boostTrails.SetActive(true);
                        var mat = _carBodyRenderer.materials[2];
                        var defaultColor = mat.color;

                        GameManager.Instance.SpeedBoost();
                        DOTween.Sequence()
                            .Append(_thisTransform.DOLocalMoveZ(2, .15f).SetEase(Ease.InOutFlash))
                            .Join(mat.DOColor(_saturatedColor, .15f))
                            .Append(_thisTransform.DOLocalMoveZ(0, .15f))
                            .Join(mat.DOColor(defaultColor, .15f))
                            .AppendCallback(() =>
                            {
                                IsBoosted = false;
                                _boostTrails.SetActive(false);
                            });
                    }
                    else if (!isRight)
                    {
                        isRight = true;
                        _canInput = false;
                        DOTween.Sequence()
                            .Append(_thisTransform.DOLocalMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                            .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                            .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                            .AppendCallback(() => {
                                _canInput = true;
                                GameManager.Instance.ShakeCamera();
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
                                GameManager.Instance.ShakeCamera();
                            });
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsBoosted = true;
                _boostTrails.SetActive(true);
                var mat = _carBodyRenderer.materials[2];
                var defaultColor = mat.color;

                GameManager.Instance.SpeedBoost();
                DOTween.Sequence()
                    .Append(_thisTransform.DOLocalMoveZ(2, .15f).SetEase(Ease.InOutFlash))
                    .Join(mat.DOColor(_saturatedColor, .15f))
                    .Append(_thisTransform.DOLocalMoveZ(0, .15f))
                    .Join(mat.DOColor(defaultColor, .15f))
                    .AppendCallback(() =>
                    {
                        IsBoosted = false;
                        _boostTrails.SetActive(false);
                    });
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !isRight)
            {
                isRight = true;
                _canInput = false;
                DOTween.Sequence()
                    .Append(_thisTransform.DOLocalMoveX(pos.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                    .Join(_thisTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                    .Append(_thisTransform.DORotate(new Vector3(0, 0, 0), .05f))
                    .AppendCallback(() => {
                        _canInput = true;
                        GameManager.Instance.ShakeCamera();
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
                        GameManager.Instance.ShakeCamera();
                    });
            }
#endif
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoadObject"))
        {
            Destroy(other.gameObject);
            var mat = _carBodyRenderer.materials[2];
            var defaultColor = mat.color;

            DOTween.Sequence()
                .Append(_thisTransform.DOScale(.5f, .15f))
                .Join(mat.DOColor(_saturatedColor, .15f))
                .Append(_thisTransform.DOScale(.4f, .15f).SetEase(Ease.InOutBounce))
                .Join(mat.DOColor(defaultColor, .15f));
        }
    }
}

public enum CarSide
{
    Left, Right
}