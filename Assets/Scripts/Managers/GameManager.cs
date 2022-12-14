    namespace EventsManager {
    using System.Collections;
    using UnityEngine;
    using System.Collections.Generic;
    using SDD.Events;
    using UnityEngine.InputSystem;
    using System.Linq;
    using FMODUnity;
    using EventManager = SDD.Events.EventManager;

    public enum GameState { gameMenu, gamePlay, initializingLevel, gamePause, gameOver}

    public class GameManager : Manager<GameManager> {
        #region Game State
        private GameState _GameState;
        public bool IsPlaying { get { return _GameState == GameState.gamePlay; } }

        public bool IsInMainMenu { get { return _GameState == GameState.gameMenu; } }

        public bool IsPausing { get { return _GameState == GameState.gamePause; } }

        public bool IsGameOver { get { return _GameState == GameState.gameOver; } }
        #endregion

        [SerializeField] private int _MaxHealth;
        [SerializeField] private StudioEventEmitter _Emitter;

        private List<int> _Skills = new() { 0, 0, 0 };

        private int _CurrentPoints = 0;

        private int _Health;

        private int _Wave = 0;

        public int Wave { get { return _Wave; } }

        public int CurrentPoints { get { return _CurrentPoints; } }

        public void AddWave() {
            _Wave++;
        }

        public int GetSkill(int idx) {
            return _Skills[idx];
        }

        public void DecrementHealth(int decrement) {
            _Health -= decrement;
            if (_Health <= 0) Over();
            else EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eHealth = _Health, eStr = _Skills[0], eInt = _Skills[1], eDex = _Skills[2] });
        }

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
            Menu();
            yield break;
        }

        private void Update() {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) {
                if (IsPausing) Resume();
                else Pause();
            }
        }

        public string GetClass() {
            int total = GetTotalPoint();

            if (total == 0) return Jobs.JobToString(Jobs.ConvertRangeSkillStringToJob("lowlowlow"));

            float strPercentage = (_Skills[0] * 100) / total;
            float intPercentage = (_Skills[1] * 100) / total;
            float dexPercentage = (_Skills[2] * 100) / total;

            string query = ComputeQuery(strPercentage, intPercentage, dexPercentage, total);

            return Jobs.JobToString(Jobs.ConvertRangeSkillStringToJob(query));
        }

        public int GetTotalPoint() {
            return _Skills.Sum();
        }

        private string ComputeQuery(float strPercentage, float intPercentage, float dexPercentage, float total) {

            if (strPercentage == intPercentage && intPercentage == dexPercentage) return "medmedmed";

            if (strPercentage == intPercentage && strPercentage != 0 && strPercentage > dexPercentage) return "medmedlow";
            if (strPercentage == dexPercentage && strPercentage != 0 && strPercentage > intPercentage) return "medlowmed";
            if (intPercentage == dexPercentage && intPercentage != 0 && intPercentage > strPercentage) return "lowmedmed";

            string query = "";

            if (Mathf.Max(strPercentage, Mathf.Max(intPercentage, dexPercentage)) == strPercentage) query += "high";
            else if (strPercentage > intPercentage || strPercentage > dexPercentage) query += "med";
            else query += "low";
            
            if (Mathf.Max(strPercentage, Mathf.Max(intPercentage, dexPercentage)) == intPercentage) query += "high";
            else if(intPercentage > strPercentage || intPercentage > dexPercentage) query += "med";
            else query += "low";

            if (Mathf.Max(strPercentage, Mathf.Max(intPercentage, dexPercentage)) == dexPercentage) query += "high";
            else if (dexPercentage > strPercentage || dexPercentage > intPercentage) query += "med";
            else query += "low";

            return query;
        }

        public void SetTimeScale(float newTimeScale) {
            Time.timeScale = newTimeScale;
        }

        public void IncreaseSkill(string skill) {
            switch(skill) {
                case "str": 
                    _Skills[0]++; 
                    break;
                case "int": 
                    _Skills[1]++;
                    break;
                case "dex": 
                    _Skills[2]++;
                    break;
                default:
                    break;
            }
            EventManager.Instance.Raise(new PointLostEvent());
            EventManager.Instance.Raise(new GameStatisticsChangedEvent { eStr = _Skills[0], eInt = _Skills[1], eDex = _Skills[2], eHealth = _Health });
            PlayerController.Instance.UpdateSkin(GetClass());
        }
        #endregion

        #region Callbacks to Events issued by MenuManager
        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e) {
            if (IsPausing || IsGameOver) Menu();
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
            _Health = _MaxHealth;
            _Skills = new() { 0, 0, 0};
            _Wave = 0;
            _CurrentPoints = 0;
            EventManager.Instance.Raise(new GameStatisticsChangedEvent { eStr = _Skills[0], eInt = _Skills[1], eDex = _Skills[2], eHealth = _Health });
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
            _Emitter.Play();
            SetTimeScale(0);
            _GameState = GameState.gameOver;
            EventManager.Instance.Raise(new GameOverEvent());
        }
        #endregion
    }
}

