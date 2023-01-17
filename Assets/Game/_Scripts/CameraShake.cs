using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake()
    {
        float intensity = 2f;
        float time = .2f;

        var multPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        var halfTime = time * .5f;
        DOTween.Sequence()
            .Append(DOTween.To(() => multPerlin.m_AmplitudeGain, x => multPerlin.m_AmplitudeGain = x, intensity, halfTime))
            .Append(DOTween.To(() => multPerlin.m_AmplitudeGain, x => multPerlin.m_AmplitudeGain = x, 0, halfTime));
    }
}
