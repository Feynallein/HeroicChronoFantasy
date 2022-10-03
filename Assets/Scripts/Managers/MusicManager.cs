using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using EventsManager;
using FMODUnity;

public class MusicManager : SingletonGameStateObserver<MusicManager> {
    [SerializeField] private StudioEventEmitter _Emitter;
    [SerializeField] private float _FadeDuration;

    private Coroutine _CurrentCoroutine;

    protected override void Awake() {
        base.Awake();
        _Emitter.Play();
    }

    private void Menu() {
        if(_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        _CurrentCoroutine = StartCoroutine(SwapTrackCoroutine(0, _FadeDuration));
    }

    private void Game() {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        _CurrentCoroutine = StartCoroutine(SwapTrackCoroutine(1, _FadeDuration));
    }

    private IEnumerator SwapTrackCoroutine(int target, float duration) {
        _Emitter.EventInstance.getParameterByName("IsPlaying", out float currentValue);
        if (target == currentValue) yield break;
        float elapsedTime = 0;
        while(elapsedTime <= duration) {
            float value = elapsedTime / duration;
            _Emitter.SetParameter("IsPlaying", target == 1 ? value : 1 - value);
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
        }
        _Emitter.SetParameter("IsPlaying", target);
    }

    protected override void GamePlay(GamePlayEvent e) {
        Game();
    }

    protected override void GameMenu(GameMenuEvent e) {
        Menu();
    }

    protected override void GameOver(GameOverEvent e) {
        Menu();
    }
}
