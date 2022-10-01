namespace EventsManager {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;
    using UnityEngine.UI;

    public class MenuManager : Manager<MenuManager> {
        #region Variables
        [Header("Panels")]
        [Tooltip("Panel displayed when in the main menu")]
        [SerializeField] GameObject _MainMenuPanel;

        [Tooltip("Panel displayed when in pause")]
        [SerializeField] GameObject _PausePanel;

        [Tooltip("Panel displayed when game over")]
        [SerializeField] GameObject _GameOverPanel;

        [Tooltip("Panel displayed when victory")]
        [SerializeField] GameObject _VictoryPanel;

        List<GameObject> _AllPanels;
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            yield break;
        }

        #endregion

        #region Monobehaviour lifecycle
        protected override void Awake() {
            base.Awake();
            RegisterPanels();
        }
        #endregion

        #region Panel Methods
        void RegisterPanels() {
            // Add every panels to the list
            _AllPanels = new List<GameObject> {
                _MainMenuPanel,
                _PausePanel,
                _GameOverPanel,
                _VictoryPanel
            };
        }

        void OpenPanel(GameObject panel) {
            // Open a specific panel
            foreach (var item in _AllPanels) if (item) item.SetActive(item == panel);
        }
        #endregion

        #region UI OnClick Events
        /* Raising correspondent event */
        public void EscapeButtonHasBeenClicked() {
            EventManager.Instance.Raise(new EscapeButtonClickedEvent());
        }

        public void ResumeButtonHasBeenClicked() {
            EventManager.Instance.Raise(new ResumeButtonClickedEvent());
        }

        public void QuitButtonHasBeenClicked() {
            EventManager.Instance.Raise(new QuitButtonClickedEvent());
        }

        public void ReturnToMainMenuFromMenu() {
            OpenPanel(_MainMenuPanel);
        }
        #endregion

        #region Callbacks to GameManager events
        /* Displaying correspondent panel */
        protected override void GameMenu(GameMenuEvent e) {
            OpenPanel(_MainMenuPanel);
        }

        protected override void GamePlay(GamePlayEvent e) {
            OpenPanel(null);
        }

        protected override void GamePause(GamePauseEvent e) {
            OpenPanel(_PausePanel);
        }

        protected override void GameResume(GameResumeEvent e) {
            OpenPanel(null);
        }

        protected override void GameOver(GameOverEvent e) {
            OpenPanel(_GameOverPanel);
        }

        protected override void GameVictory(GameVictoryEvent e) {
            OpenPanel(_VictoryPanel);
        }
        #endregion
    }
}
