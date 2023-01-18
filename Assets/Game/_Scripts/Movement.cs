using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Car _leftCar;
    [SerializeField] private Car _rightCar;
    [SerializeField] private float _posToUpdate = 1.1f;
    [SerializeField] private float _degreeToRotate = 35f;
    [SerializeField] private float _angleForSwipeUp = 45;
    [SerializeField] private float _speed = 10f;
    private Transform _thisTransform;
    private bool _canMove = false;
    private Vector2 _startTouchPos, _endTouchPos;


    private void Awake()
    {
        _thisTransform = transform;
        Input.multiTouchEnabled = true;
    }

    private void Update()
    {
        if (!_canMove) return;
        _thisTransform.position += Vector3.forward * _speed * Time.deltaTime;

#if PLATFORM_ANDROID
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (i >= 2) return;
            var touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began) _startTouchPos = touch.position;
            else if (touch.phase == TouchPhase.Moved)
            {
                var touchPos = touch.position;
                _endTouchPos = touch.position;
                var distance = _endTouchPos - _startTouchPos;
                var angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
                if (_endTouchPos.y > _startTouchPos.y && (angle >= 90 - _angleForSwipeUp && angle <= 90 + _angleForSwipeUp))
                {
                    if (touchPos.x < Screen.width * .5f)
                    {
                        _leftCar.Boost();
                    }
                    else if (touchPos.x > Screen.width * .5f)
                    {
                        _rightCar.Boost();
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                var touchPos = touch.position;
                if (touchPos.x < Screen.width * .5f)
                {
                    MoveCar(_leftCar);
                }
                else if (touchPos.x > Screen.width * .5f)
                {
                    MoveCar(_rightCar);
                }
            }
        }
//#else
        if (Input.GetKeyUp(KeyCode.A) && _leftCar.isRight) MoveCar(_leftCar);
        else if (Input.GetKeyUp(KeyCode.D) && !_leftCar.isRight) MoveCar(_leftCar);        
        else if (Input.GetKeyUp(KeyCode.W)) _leftCar.Boost();

        if (Input.GetKeyUp(KeyCode.LeftArrow) && _rightCar.isRight) MoveCar(_rightCar);        
        else if (Input.GetKeyUp(KeyCode.RightArrow) && !_rightCar.isRight) MoveCar(_rightCar);        
        else if (Input.GetKeyUp(KeyCode.UpArrow)) _rightCar.Boost();
#endif
    }

    private void MoveCar(Car car)
    {
        if (!car.canInput || car.IsBoosted) return;

        var carTransform = car.transform;
        if (!car.isRight)
        {
            car.PlayWhooshSound();
            car.isRight = true;
            car.canInput = false;
            DOTween.Sequence()
                .Append(carTransform.DOLocalMoveX(carTransform.position.x + _posToUpdate, .15f).SetEase(Ease.InOutSine))
                .Join(carTransform.DORotate(new Vector3(0, _degreeToRotate, 0), .1f))
                .Append(carTransform.DORotate(new Vector3(0, 0, 0), .05f))
                .AppendCallback(() =>
                {
                    car.canInput = true;
                    GameManager.Instance.ShakeCamera();
                });
        }
        else if (car.isRight)
        {
            car.PlayWhooshSound();
            car.isRight = false;
            car.canInput = false;
            DOTween.Sequence()
                .Append(carTransform.DOLocalMoveX(carTransform.position.x - _posToUpdate, .15f).SetEase(Ease.InOutSine))
                .Join(carTransform.DORotate(new Vector3(0, -_degreeToRotate, 0), .1f))
                .Append(carTransform.DORotate(new Vector3(0, 0, 0), .05f))
                .AppendCallback(() =>
                {
                    car.canInput = true;
                    GameManager.Instance.ShakeCamera();
                });
        }
    }

    public void SetMovement(bool shouldMove)
    {
        _canMove = shouldMove;
    }
}
