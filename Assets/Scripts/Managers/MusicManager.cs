using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using EventsManager;
using FMODUnity;

public class MusicManager : SingletonGameStateObserver<MusicManager> {
    [SerializeField] private StudioEventEmitter _Emitter;

    protected override void Awake() {
        base.Awake();
        _Emitter.Play();
    }

    protected override void GamePlay(GamePlayEvent e) {
        _Emitter.SetParameter("IsPlaying", 1);
    }

    protected override void GameMenu(GameMenuEvent e) {
        _Emitter.SetParameter("IsPlaying", 0);
    }

    protected override void GameOver(GameOverEvent e) {
        _Emitter.SetParameter("IsPlaying", 0);
    }

    protected override void GamePause(GamePauseEvent e) {
        //todo
    }

    protected override void GameResume(GameResumeEvent e) {
        //todo
    }
}
