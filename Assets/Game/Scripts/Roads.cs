using UnityEngine;

public class Roads : MonoBehaviour
{
    [SerializeField] private Transform[] _leftSpawnPos;
    [SerializeField] private Transform[] _rightSpawnPos;
    [SerializeField] private Transform _roadObject;
    [SerializeField, Range(0f, 1f)] private float _spawnProbability = .5f;

    public void SpawnRoadObject()
    {
        Vector3 spawnPos = Vector3.zero;
        for (int i = 0; i < 2; i++)
        {
            var randVal = Random.Range(0, 2);
            if (randVal == 1)
            {
                if (Random.Range(0f, 1f) < _spawnProbability) return;

                if (i == 0)
                {
                    randVal = Random.Range(0, _leftSpawnPos.Length);
                    spawnPos = _leftSpawnPos[randVal].position;
                }
                else
                {
                    randVal = Random.Range(0, _rightSpawnPos.Length);
                    spawnPos = _rightSpawnPos[randVal].position;
                }
                var obj = Instantiate(_roadObject, spawnPos, Quaternion.identity);
                Destroy(obj.gameObject, 5);
            }
        }
    }
}
