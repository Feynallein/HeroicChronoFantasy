using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour {
    [SerializeField] protected float _Duration;

    protected float _ElapsedTime;

    protected virtual void OnEnable() {
        _ElapsedTime = 0;
    }

    protected virtual void Update() {
        if (_ElapsedTime >= _Duration) {
            LevelManager.Instance.MiniGameCallback(false);
        } else _ElapsedTime += Time.unscaledDeltaTime;
    }
}
