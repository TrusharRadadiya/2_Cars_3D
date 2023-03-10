using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Movement _movementObj;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Volume _speedVolume;

    [Header("Score"), SerializeField] private TextMeshProUGUI _scoreText;
    private int _score
    {
        get => PlayerPrefs.GetInt(nameof(_score), 0);
        set => PlayerPrefs.SetInt(nameof(_score), value);
    }

    [Header("Game Over"), SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private Transform _gameOverTextTransform;
    [SerializeField] private Transform _restartTextTransform;

    [Header("Audio Clips")]
    public AudioClip _collectOrbClip;
    public AudioClip _whooshClip;

    [Space, SerializeField] private GameObject _settingsAndInfoCanvas;

    private bool _gameOver = true;
    public bool gameOver => _gameOver;
    private Camera _cam;
    
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this.gameObject);

        _cam = Camera.main;
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
        _scoreText.text = _score.ToString();
    }


    public void StartGame()
    {
        _movementObj.SetMovement(true);
        _gameOver = false;
        _settingsAndInfoCanvas.SetActive(false);
    }

    public void GameOver()
    {
        _gameOver = true;
        _movementObj.SetMovement(false);
        _cameraShake.Shake();

        _gameOverCanvas.SetActive(true);
        _restartTextTransform.DOScale(.9f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _gameOverTextTransform.DOScale(1f, .5f).From(0).SetEase(Ease.OutBack);
    }

    public void ShakeCamera() => _cameraShake.Shake();

    public void SpeedBoostEffect()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => _speedVolume.weight, x => _speedVolume.weight = x, 1, .15f).SetEase(Ease.InSine))
            .Append(DOTween.To(() => _speedVolume.weight, x => _speedVolume.weight = x, 0, .15f).SetEase(Ease.OutSine));
    }

    public void ScoreUp()
    {
        _score++;

        var scoreTransform = _scoreText.transform;
        DOTween.Sequence()
            .Append(scoreTransform.DOScale(1.1f, .1f))
            .AppendCallback(() => _scoreText.text = _score.ToString())
            .Join(scoreTransform.DOScale(1f, .1f));
    }

    public void OnClick_RestartText() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void OnToggle_PostProcessing(bool isOn)
    {
        _cam.GetUniversalAdditionalCameraData().renderPostProcessing = isOn;
    }
}
