using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour {
    [SerializeField] private float _Duration;
    [SerializeField] private Image _Image;

    private float _ElapsedTime;

    protected virtual void OnEnable() {
        _ElapsedTime = 0;
    }

    protected virtual void Update() {
        print(_ElapsedTime);
        if (_ElapsedTime >= _Duration) {
            MiniGameManager.Instance.MiniGameCallback(false);
        } else _ElapsedTime += Time.deltaTime;
    }
}
