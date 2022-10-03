using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour {
    [SerializeField] protected float _MaxDuration;
    [SerializeField] protected float _MinDuration;
    [SerializeField] protected StudioEventEmitter _Emitter;

    protected float _Duration;

    protected float _ElapsedTime;

    protected virtual void Update() {
        if (_ElapsedTime >= _Duration) {
            LevelManager.Instance.MiniGameCallback(false);
        } else _ElapsedTime += Time.unscaledDeltaTime;
    }

    public void SetDifficulty(float difficulty) {
        AdaptToDifficulty(difficulty);
    }

    private void AdaptToDifficulty(float difficutly) {
        _ElapsedTime = 0;
        _Duration = _MinDuration + difficutly * _MaxDuration;
        AdaptToDifficultyChild(difficutly);
    }

    protected abstract void AdaptToDifficultyChild(float difficulty);
}
