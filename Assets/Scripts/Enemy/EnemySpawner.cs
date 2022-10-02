using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] Transform _TopLeftBound;
    [SerializeField] Transform _BottomRightBound;
    [SerializeField] private GameObject _EnemyToSpawn;

    public void Spawn() {
        GameObject go = Instantiate(_EnemyToSpawn, transform.position, transform.rotation);
        go.GetComponent<Enemy>().SetBounds(_TopLeftBound.position, _BottomRightBound.position);
    }
}
