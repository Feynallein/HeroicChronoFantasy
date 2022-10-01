using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;
using SDD.Events;

public class MiniGameManager : SingletonGameStateObserver<MiniGameManager> {
    [SerializeField] private int _DurationBetweenEvents = 10;
    [SerializeField] private List<GameObject> _Minigames;
    [SerializeField] private GameObject _Background;

    private float _Clock;
    private bool _Start = false;

    private void Start() {
        _Background.SetActive(false);
        _Minigames.ForEach(miniGame => miniGame.SetActive(false));
    }

    void Update() {
        if (!_Start) return;
        if (_Clock >= _DurationBetweenEvents) {
            MiniGameTime();
            _Clock = 0;
        } else _Clock += Time.deltaTime;
    }

    protected override void GamePlay(GamePlayEvent e) {
        _Clock = 0;
        _Start = true;
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
}
