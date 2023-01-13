using DG.Tweening;
using UnityEngine;

public class Dance : MonoBehaviour
{
    private void Start()
    {
        transform.DOScaleY(Random.Range(.5f, 2f), Random.Range(.2f, .5f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
