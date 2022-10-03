namespace EventsManager {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;
    using UnityEngine.UI;
    using TMPro;

    public class MenuManager : Manager<MenuManager> {
        #region Variables
        [Header("Panels")]
        [Tooltip("Panel displayed when in the main menu")]
        [SerializeField] GameObject _MainMenuPanel;

        [Tooltip("Panel displayed when in pause")]
        [SerializeField] GameObject _PausePanel;

        [Tooltip("Panel displayed when game over")]
        [SerializeField] GameObject _GameOverPanel;

        [Tooltip("Panel displayed for credits")]
        [SerializeField] GameObject _CreditsPanel;

        [SerializeField] private TextMeshProUGUI _GameOverStats;

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
                _CreditsPanel,
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

        public void PlayButtonHasBeenClicked() {
            EventManager.Instance.Raise(new PlayButtonClickedEvent());
        }

        public void MainMenuButtonHasBeenClicked() {
            EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
        }
        
        public void CreditButtonHasBeenClicked() {
            OpenPanel(_CreditsPanel);
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
            _GameOverStats.text = "Enemies killed: " + GameManager.Instance.GetTotalPoint();
        }
        #endregion
    }
}
