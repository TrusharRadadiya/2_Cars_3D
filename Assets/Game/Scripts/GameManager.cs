using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Movement _movementObj;

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
    }
}
