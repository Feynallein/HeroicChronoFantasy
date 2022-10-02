using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpam : MiniGame {
    [SerializeField] private int _NumberOfClickNeeded;

    private int _NumberOfClicks;

    protected override void OnEnable() {
        base.OnEnable();
        _NumberOfClicks = 0;
    }

    protected override void Update() {
        base.Update();

        if (_NumberOfClicks >= _NumberOfClickNeeded) LevelManager.Instance.MiniGameCallback(true);
    }

    public void ButtonCallback() {
        _NumberOfClicks++;
    }
}
