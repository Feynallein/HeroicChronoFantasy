using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clic : MiniGame {
    [SerializeField] private GameObject _ButtonToSpawn;
    [SerializeField] private Transform _TopLeftBound;
    [SerializeField] private Transform _BottomRightBound;
    [SerializeField] private Image _Image;
    [SerializeField] private int _NumberOfButtonToSpawn;
    [SerializeField] private int _TimeBetweenSpawns;

    private List<GameObject> _SpawnedButton = new();
    private float _TimeBeforeSpawn = 0;
    private int _NumberOfButtonSpawned = 0;

    protected override void OnEnable() {
        base.OnEnable();
        _TimeBeforeSpawn = 0;
        _NumberOfButtonSpawned = 0;
        _SpawnedButton.ForEach(x => Destroy(x));
        _SpawnedButton.Clear();
    }

    protected override void Update() {
        base.Update();
        if (_NumberOfButtonSpawned == _NumberOfButtonToSpawn && _SpawnedButton.Count == 0) LevelManager.Instance.MiniGameCallback(true);
        if(_TimeBeforeSpawn >= _TimeBetweenSpawns && _NumberOfButtonSpawned < _NumberOfButtonToSpawn) {
            Spawn();
            _TimeBeforeSpawn = 0;
        }
        else _TimeBeforeSpawn += Time.unscaledDeltaTime;
    }

    private void Spawn() {
        Vector3 pos = new Vector3(Random.Range(_TopLeftBound.position.x, _BottomRightBound.position.x), Random.Range(_TopLeftBound.position.y, _BottomRightBound.position.y), 0);
        GameObject go = Instantiate(_ButtonToSpawn, pos, transform.rotation, transform);
        go.GetComponent<Button>().onClick.AddListener(delegate { ButtonCallback(go); });
        _NumberOfButtonSpawned++;
        //go.GetComponent<Button>().image = _Image;
        _SpawnedButton.Add(go);
    }

    public void ButtonCallback(GameObject button) {
        Destroy(button);
        _SpawnedButton.Remove(button);
    }
}
