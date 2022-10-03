using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseSpam : MiniGame {
    [SerializeField] private int _MaxNumberOfClickNeeded;
    [SerializeField] private int _MinNumberOfClickNeeded;
    [SerializeField] private TextMeshProUGUI _ClickCounter;
    [SerializeField] private TextMeshProUGUI _Timer;

    private int _NumberOfClicks;
    private int _NumberOfClickNeeded;

    protected override void AdaptToDifficultyChild(float difficulty) {
        _NumberOfClicks = 0;
        _NumberOfClickNeeded = Mathf.FloorToInt(_MinNumberOfClickNeeded + difficulty * _MaxNumberOfClickNeeded);
    }

    protected override void Update() {
        base.Update();
        _ClickCounter.text = (_NumberOfClickNeeded - _NumberOfClicks).ToString();
        _Timer.text = (_Duration - _ElapsedTime).ToString("n2");
        if (_NumberOfClicks >= _NumberOfClickNeeded) LevelManager.Instance.MiniGameCallback(true);
    }

    public void ButtonCallback() {
        _NumberOfClicks++;
        _Emitter.Play();
    }
}
