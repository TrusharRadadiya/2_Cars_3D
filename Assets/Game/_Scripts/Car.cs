using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    public bool isRight = false;
    [SerializeField] private MeshRenderer _carBodyRenderer;
    [SerializeField] private Color _saturatedColor;
    [SerializeField] private GameObject _boostTrails;

    [Header("AudioSources")]
    [SerializeField] private AudioSource _whooshSource;
    [SerializeField] private AudioSource _collectOrbSource;
    [SerializeField] private AudioSource _boostSource;
    [SerializeField] private AudioSource _explosionSource;

    [HideInInspector] public bool canInput = true;    
    public bool IsBoosted { get; set; }
    

    public void PlayWhooshSound() => _whooshSource.PlayOneShot(_whooshSource.clip);

    public void PlayExplosionSound() => _explosionSource.PlayOneShot(_explosionSource.clip);

    public void Boost()
    {
        if (IsBoosted) return;

        IsBoosted = true;
        _boostTrails.SetActive(true);
        var mat = _carBodyRenderer.materials[2];
        var defaultColor = mat.color;

        _boostSource.PlayOneShot(_boostSource.clip);
        GameManager.Instance.SpeedBoostEffect();
        DOTween.Sequence()
            .Append(transform.DOLocalMoveZ(2, .15f).SetEase(Ease.InOutFlash))
            .Join(mat.DOColor(_saturatedColor, .15f))
            .Append(transform.DOLocalMoveZ(0, .15f))
            .Join(mat.DOColor(defaultColor, .15f))
            .AppendCallback(() =>
            {
                IsBoosted = false;
                _boostTrails.SetActive(false);
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoadObject"))
        {
            GameManager.Instance.ScoreUp();
            _collectOrbSource.PlayOneShot(_collectOrbSource.clip);
            Destroy(other.gameObject);
            var mat = _carBodyRenderer.materials[2];
            var defaultColor = mat.color;

            DOTween.Sequence()
                .Append(transform.DOScale(.5f, .15f))
                .Join(mat.DOColor(_saturatedColor, .15f))
                .Append(transform.DOScale(.4f, .15f).SetEase(Ease.InOutBounce))
                .Join(mat.DOColor(defaultColor, .15f));
        }
    }
}