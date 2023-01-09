using DG.Tweening;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject _brokenPiece;
    [SerializeField] private float _explosionForce = 100;
    private bool _isBroken = false;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Car"))
    //    {
    //        var carMovement = collision.gameObject.GetComponent<CarMovement>();
    //        if (_isBroken && !carMovement.IsBoosted) return;

    //        if (collision.relativeVelocity.magnitude >= _breakForce)
    //        {
    //            _isBroken = true;
    //            var brokenPiece = Instantiate(_brokenPiece, transform.position, transform.rotation);

    //            var thisRend = GetComponent<Renderer>();
    //            var rends = brokenPiece.GetComponentsInChildren<Renderer>();
    //            foreach (var rend in rends) rend.material = thisRend.material;

    //            var rbs = brokenPiece.GetComponentsInChildren<Rigidbody>();
    //            foreach (var rb in rbs)
    //                rb.AddExplosionForce(collision.relativeVelocity.magnitude * _explosionForce, collision.contacts[0].point, 2);

    //            Destroy(this.gameObject);
    //            carMovement.IsBoosted = false;
    //        }
    //    }
    //}

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
                rb.AddExplosionForce(100, this.transform.position, 1f, .1f);
                rb.constraints = RigidbodyConstraints.None;
                
                return;
            }

            _isBroken = true;
            var brokenPiece = Instantiate(_brokenPiece, this.transform.position, this.transform.rotation);

            var thisRend = GetComponent<Renderer>();
            var rends = brokenPiece.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends) rend.material = thisRend.material;

            var rbs = brokenPiece.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(Random.Range(_explosionForce * .5f, _explosionForce), this.transform.position, 1, .05f);                
            }

            Destroy(this.gameObject);
            carMovement.IsBoosted = false;
        }
    }
}
