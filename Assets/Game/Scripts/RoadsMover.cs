using UnityEngine;

public class RoadsMover : MonoBehaviour
{
    [SerializeField] private Transform _lastRoad;
    private float _currentZPos;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _currentZPos = _lastRoad.position.z;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Roads"))
        {
            var roadsPos = other.transform.position;
            _currentZPos += 2;
            roadsPos.z = _currentZPos;
            other.transform.position = roadsPos;

            other.GetComponent<Roads>().SpawnRoadObject();
        }
    }
}
