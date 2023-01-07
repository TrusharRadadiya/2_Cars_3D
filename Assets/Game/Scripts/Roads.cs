using UnityEngine;

public class Roads : MonoBehaviour
{
    [SerializeField] private Transform[] _leftSpawnPos;
    [SerializeField] private Transform[] _rightSpawnPos;
    [Space, SerializeField] private Transform _roadObject;
    [SerializeField, Range(0f, 1f)] private float _spawnProbability = .5f;
    [Space, SerializeField] private Transform _wrongObject;
    [SerializeField, Range(0f, 1f)] private float _wrongObjProbability = .1f;

    public void SpawnRoadObject()
    {
        Vector3 spawnPos = Vector3.zero;
        Transform objToSpawn = null;
        for (int i = 0; i < 2; i++) // if 0 = leftSpawnPos, 1 = rightSpawnPos
        {
            var spawnVal = Random.Range(0, 2); // Spawn if got 1
            if (spawnVal == 1)
            {
                var objProb = Random.Range(0, 2); // if 1 = roadObject, otherwise = wrongObject
                if (objProb == 1)
                {
                    if (Random.Range(0f, 1f) > _spawnProbability) return;
                    objToSpawn = _roadObject;
                }
                else
                {
                    if (Random.Range(0f, 1f) > _wrongObjProbability) return;
                    objToSpawn = _wrongObject;
                }

                if (i == 0)
                {
                    spawnVal = Random.Range(0, _leftSpawnPos.Length);
                    spawnPos = _leftSpawnPos[spawnVal].position;
                }
                else
                {
                    spawnVal = Random.Range(0, _rightSpawnPos.Length);
                    spawnPos = _rightSpawnPos[spawnVal].position;
                }

                var obj = Instantiate(objToSpawn, spawnPos, Quaternion.identity);
                Destroy(obj.gameObject, 5);
            }
        }
    }
}
