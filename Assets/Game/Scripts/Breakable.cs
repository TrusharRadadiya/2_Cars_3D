using DG.Tweening;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject _brokenPiece;
    [SerializeField] private float _explosionForce = 100;
    private bool _isBroken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isBroken) return;

        if (other.CompareTag("Car"))
        {
            var carMovement = other.GetComponent<CarMovement>();
            if (!carMovement.IsBoosted)
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
            var brokenPiece = Instantiate(_brokenPiece, this.transform.position, this.transform.rotation);

            var thisRend = GetComponent<Renderer>();
            var rends = brokenPiece.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends) rend.material = thisRend.material;

            var rbs = brokenPiece.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(_explosionForce, this.transform.position, 1, .05f, ForceMode.Impulse);                
            }

            Destroy(this.gameObject);
            Destroy(brokenPiece.gameObject, 10);
            carMovement.IsBoosted = false;
        }
    }
}
