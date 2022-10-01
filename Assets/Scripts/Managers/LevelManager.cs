using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;
using SDD.Events;

public class LevelManager : Manager<LevelManager> {
    [SerializeField] List<EnemySpawner> _Spawners = new();
    [SerializeField] private int _DurationBetweenEvents = 10;
    [SerializeField] private List<GameObject> _Minigames;
    [SerializeField] private GameObject _Background;

    private float _Clock;
    private bool _Start = false;

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
        for(int i = 0; i < GameManager.Instance.Wave; i++) {
            _Spawners[Random.Range(0, _Spawners.Count)].Spawn();
        }
        GameManager.Instance.AddWave();
    }

    private void MiniGameTime() {
        int rand = Random.Range(0, _Minigames.Count);
        GameManager.Instance.SetTimeScale(0);
        _Minigames[rand].SetActive(true);
        _Background.SetActive(true);
    }

    public void MiniGameCallback(bool win) {
        _Background.SetActive(false);
        _Minigames.ForEach(miniGame => miniGame.SetActive(false));
        GameManager.Instance.SetTimeScale(1);
        if (win) EventManager.Instance.Raise(new PointGainedEvent());
        else GameManager.Instance.DecrementHealth(1);
    }

    protected override void GameOver(GameOverEvent e) {

    }

    protected override void GameVictory(GameVictoryEvent e) {

    }

    protected override void GamePlay(GamePlayEvent e) {
        _Clock = 0;
        _Start = true;
    }

    protected override void GameMenu(GameMenuEvent e) {

    }
}
