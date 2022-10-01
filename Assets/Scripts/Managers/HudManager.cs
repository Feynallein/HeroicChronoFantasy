namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using SDD.Events;

    public class HudManager : Manager<HudManager> {
        #region Variables
        [Tooltip("HUD GameObject (global parent)")]
        [SerializeField] GameObject _HUD;
        #endregion

        #region Manager implementation
        protected override void Awake() {
            base.Awake();
            SetHudActive(false);
        }

        protected override IEnumerator InitCoroutine() {
            yield break;
        }
        #endregion

        #region HudManager Methods
        void SetHudActive(bool active) {
            _HUD.SetActive(active);
        }
        #endregion

        #region Event's Callbacks
        protected override void GamePlay(GamePlayEvent e) {
            SetHudActive(true);
        }

        protected override void GameVictory(GameVictoryEvent e) {
            SetHudActive(false);
        }

        protected override void GameOver(GameOverEvent e) {
            SetHudActive(false);
        }

        protected override void GameMenu(GameMenuEvent e) {
            SetHudActive(false);
        }


        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e) {

        }
        #endregion
    }
}