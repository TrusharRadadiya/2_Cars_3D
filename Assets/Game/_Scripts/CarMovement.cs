using UnityEngine;
using DG.Tweening;

public class CarMovement : MonoBehaviour
{
    [HideInInspector] public bool isRight = false;
    [SerializeField] private MeshRenderer _carBodyRenderer;
    [SerializeField] private Color _saturatedColor;
    [SerializeField] private GameObject _boostTrails;    
    [HideInInspector] public bool canInput = true;    
    public bool IsBoosted { get; set; }
    
    private AudioSource _audioSource;    


    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound()
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    public void Boost()
    {
        IsBoosted = true;
        _boostTrails.SetActive(true);
        var mat = _carBodyRenderer.materials[2];
        var defaultColor = mat.color;

        GameManager.Instance.SpeedBoost();
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
            AudioSystem.Instance.PlaySound(GameManager.Instance._collectOrbClip);
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

public enum CarSide
{
    Left, Right
}