using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;
using System.Xml;

public class LevelManager : Manager<LevelManager> {
    protected override IEnumerator InitCoroutine() {
        yield break;
    }

    protected override void GameOver(GameOverEvent e) {

    }

    protected override void GameVictory(GameVictoryEvent e) {

    }

    protected override void GamePlay(GamePlayEvent e) {

    }

    protected override void GameMenu(GameMenuEvent e) {

    }
}
