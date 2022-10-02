using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;
using SDD.Events;
using FMODUnity;

public class LevelManager : Manager<LevelManager> {
    public static int _DurationBetweenEvents = 10;

    [SerializeField] List<EnemySpawner> _Spawners = new();
    [SerializeField] private List<GameObject> _Minigames;
    [SerializeField] private GameObject _Background;
    [SerializeField] private int _MaxEnemies;
    [SerializeField] private EventReferenceDictionary _Events;

    private float _Clock;
    private bool _Start = false;
    private int _Enemies;

    protected override IEnumerator InitCoroutine() {
        _Background.SetActive(false);
        _Minigames.ForEach(miniGame => miniGame.SetActive(false));
        yield break;
    }

    void Update() {
        if (!_Start) return;
        if (_Clock >= _DurationBetweenEvents) {
            Spawns();
            _Clock = 0;
        } else _Clock += Time.deltaTime;
    }

    private void Spawns() {
        if (_Enemies >= _MaxEnemies) return;
        int nbToSpawn = ComputeSpawnsFromWave(GameManager.Instance.Wave);
        for(int i = 0; i < nbToSpawn; i++) {
            _Spawners[Random.Range(0, _Spawners.Count)].Spawn();
            _Enemies++;
        }
        GameManager.Instance.AddWave();
    }

    private int ComputeSpawnsFromWave(int wave) {
        return wave switch {
            < 2 => 1,
            < 5 => 2,
            < 9 => 3,
            < 14 => 4,
            < 20 => 5,
            _ => 6,
        };
    }

    public void MiniGameTime() {
        int rand = Random.Range(0, _Minigames.Count);
        _Minigames[rand].SetActive(true);
        _Background.SetActive(true);
        GameManager.Instance.SetTimeScale(0);
    }

    public void MiniGameCallback(bool win) {
        _Background.SetActive(false);
        _Minigames.ForEach(miniGame => miniGame.SetActive(false));
        GameManager.Instance.SetTimeScale(1);
        if (win) {
            SfxManager.PlayOneShot(_Events["win"], transform);
            SDD.Events.EventManager.Instance.Raise(new PointGainedEvent());
            _Enemies--;
        } else {
            SfxManager.PlayOneShot(_Events["lose"], transform);
            GameManager.Instance.DecrementHealth(1);
        }
    }

    protected override void GameOver(GameOverEvent e) {
        _Start = false;
    }

    protected override void GamePlay(GamePlayEvent e) {
        _Clock = 0;
        _Start = true;
        Spawns();
    }

    protected override void GameMenu(GameMenuEvent e) {
        _Start = false;
    }
}
