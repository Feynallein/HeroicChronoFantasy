namespace EventsManager {
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using SDD.Events;
    using System.Linq;

    public enum GameState { gameMenu, gamePlay, initializingLevel, gamePause, gameOver, gameVictory}

    public class GameManager : Manager<GameManager> {
        //Todo: add music & sfx callbacks

        private List<Skill> _Skills = new() {
            new Skill("Shape"),
            new Skill("Knowledge"),
            new Skill("Science"),
            new Skill("Social"),
        };

        private int _CurrentPoints = 0;

        public int CurrentPoints { get { return _CurrentPoints; } }

        #region Game State
        private GameState _GameState;
        public bool IsPlaying { get { return _GameState == GameState.gamePlay; } }

        public bool IsInMainMenu { get { return _GameState == GameState.gameMenu; } }

        public bool IsPausing { get { return _GameState == GameState.gamePause; } }

        public bool IsVictory { get { return _GameState == GameState.gameVictory; } }

        public bool IsGameOver { get { return _GameState == GameState.gameOver; } }
        #endregion

        #region Events' subscription
        public override void SubscribeEvents() {
            base.SubscribeEvents();

            //MainMenuManager
            EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

            EventManager.Instance.AddListener<PointGainedEvent>(PointGained);
            EventManager.Instance.AddListener<PointLostEvent>(PointLost);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();

            //MainMenuManager
            EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

            EventManager.Instance.RemoveListener<PointGainedEvent>(PointGained);
            EventManager.Instance.RemoveListener<PointLostEvent>(PointLost);
        }
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            //Menu();
            Play();
            yield break;
        }

        void SetTimeScale(float newTimeScale) {
            Time.timeScale = newTimeScale;
        }

        public void IncreaseSkill(string skill) {
            switch(skill) {
                case "Shape": 
                    _Skills[0].IncrementValue(1); 
                    break;
                case "Knowledge": 
                    _Skills[1].IncrementValue(1);
                    break;
                case "Science": 
                    _Skills[2].IncrementValue(1);
                    break;
                case "Social":
                    _Skills[3].IncrementValue(1);
                    break;
                default: break;
            }
            EventManager.Instance.Raise(new PointLostEvent());
            EventManager.Instance.Raise(new GameStatisticsChangedEvent { eShape = _Skills[0].Value, eKnowledge = _Skills[1].Value, eScience = _Skills[2].Value, eSocial = _Skills[3].Value });
        }
        #endregion

        #region Callbacks to Events issued by MenuManager
        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e) {
            if (IsPausing || IsVictory || IsGameOver) Menu();
        }

        private void PlayButtonClicked(PlayButtonClickedEvent e) {
            Play();
        }

        private void ResumeButtonClicked(ResumeButtonClickedEvent e) {
            Resume();
        }

        private void EscapeButtonClicked(EscapeButtonClickedEvent e) {
            if (IsPlaying) Pause();
            else if (IsPausing) EventManager.Instance.Raise(new ResumeButtonClickedEvent());
            else if (IsInMainMenu) EventManager.Instance.Raise(new QuitButtonClickedEvent());
        }

        private void QuitButtonClicked(QuitButtonClickedEvent e) {
            Application.Quit();
        }

        private void PointGained(PointGainedEvent e) {
            _CurrentPoints++;
        }

        private void PointLost(PointLostEvent e) {
            _CurrentPoints--;
        }
        #endregion

        #region GameState methode
        private void Menu() {
            SetTimeScale(0);
            _GameState = GameState.gameMenu;
            EventManager.Instance.Raise(new GameMenuEvent());
        }


        private void Play() {
            SetTimeScale(1);
            _GameState = GameState.gamePlay;
            EventManager.Instance.Raise(new GameStatisticsChangedEvent { eShape = _Skills[0].Value, eKnowledge = _Skills[1].Value, eScience = _Skills[2].Value, eSocial = _Skills[3].Value });
            EventManager.Instance.Raise(new GamePlayEvent());
        }

        private void Pause() {
            if (!IsPlaying) return;
            SetTimeScale(0);
            _GameState = GameState.gamePause;
            EventManager.Instance.Raise(new GamePauseEvent());
        }

        private void Resume() {
            if (IsPlaying) return;
            SetTimeScale(1);
            _GameState = GameState.gamePlay;
            EventManager.Instance.Raise(new GameResumeEvent());
        }

        public void Over() {
            if (_GameState == GameState.gameOver) return;
            SetTimeScale(0);
            _GameState = GameState.gameOver;
            EventManager.Instance.Raise(new GameOverEvent());
        }

        public void Victory() {
            SetTimeScale(0);
            _GameState = GameState.gameVictory;
            EventManager.Instance.Raise(new GameVictoryEvent());
        }
        #endregion
    }
}

