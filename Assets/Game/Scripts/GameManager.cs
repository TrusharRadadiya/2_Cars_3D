using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Movement _movementObj;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Volume _speedVolume;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void GameOver()
    {
        _movementObj.SetMovement(false);
        _cameraShake.Shake();
    }

    public void ShakeCamera() => _cameraShake.Shake();

    public void SpeedBoost()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => _speedVolume.weight, x => _speedVolume.weight = x, 1, .15f).SetEase(Ease.InSine))
            .Append(DOTween.To(() => _speedVolume.weight, x => _speedVolume.weight = x, 0, .15f).SetEase(Ease.OutSine));
    }
}
