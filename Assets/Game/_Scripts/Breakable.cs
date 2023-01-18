using DG.Tweening;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private Transform _brokenPiece;
    [SerializeField] private float _explosionForce = 100;    
    private bool _isBroken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isBroken) return;

        if (other.CompareTag("Car"))
        {
            var car = other.GetComponent<Car>();
            if (!car.IsBoosted)
            {
                GameManager.Instance.GameOver();
                other.isTrigger = false;
                var rb = other.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddExplosionForce(2, this.transform.position, 1f, .1f, ForceMode.Impulse);
                rb.constraints = RigidbodyConstraints.None;
                
                return;
            }

            _isBroken = true;
            GameManager.Instance.ShakeCamera();
            var brokenPiece = Instantiate(_brokenPiece, transform.position, transform.rotation);            

            var rbs = brokenPiece.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
                rb.AddExplosionForce(_explosionForce, this.transform.position, 1, .05f, ForceMode.Impulse);

            car.PlayExplosionSound();
            Destroy(gameObject);
            Destroy(brokenPiece.gameObject, 10);
            car.IsBoosted = false;
        }
    }
}
