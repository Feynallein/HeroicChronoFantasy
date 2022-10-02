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

        [SerializeField] List<Text> _HUDSkills = new();
        [SerializeField] List<GameObject> _HUDSkillsButtons = new();
        [SerializeField] Text _CurrentPointsHUD;
        [SerializeField] GameObject _LeftPanel;
        [SerializeField] Text _CurrentClass;

        List<string> _HUDSkillsBaseText = new();

        #region Manager implementation
        protected override void Awake() {
            base.Awake();
            SetHudActive(false);
            _HUDSkills.ForEach(text => _HUDSkillsBaseText.Add(text.text));
        }

        protected override IEnumerator InitCoroutine() {
            yield break;
        }

        public override void SubscribeEvents() {
            base.SubscribeEvents();
            EventManager.Instance.AddListener<PointGainedEvent>(PointGained);
            EventManager.Instance.AddListener<PointLostEvent>(PointLost);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<PointGainedEvent>(PointGained);
            EventManager.Instance.RemoveListener<PointLostEvent>(PointLost);
        }

        private void ShowButtons(bool value) {
            _HUDSkillsButtons.ForEach(button => button.GetComponent<Button>().interactable = value);
        }

        private void UpdateCurrentPointsHUD() {
            _CurrentPointsHUD.text = "Points Available: " + GameManager.Instance.CurrentPoints;
            if (GameManager.Instance.CurrentPoints == 0) ShowButtons(false);
            else ShowButtons(true);
        }
        #endregion

        #region HudManager Methods
        void SetHudActive(bool active) {
            _HUD.SetActive(active);
        }

        private void UpdateTexts(List<int> skills) {
            for (int i = 0; i < _HUDSkills.Count; i++) {
                _HUDSkills[i].text = _HUDSkillsBaseText[i] + skills[i];
            }
        }

        private void UpdateClass() {
            _CurrentClass.text = "You are a " + GameManager.Instance.GetClass().ToLower();
        }
        #endregion

        #region Event's Callbacks
        protected override void GamePlay(GamePlayEvent e) {
            SetHudActive(true);
        }

        protected override void GameOver(GameOverEvent e) {
            SetHudActive(false);
        }

        protected override void GameMenu(GameMenuEvent e) {
            SetHudActive(false);
        }

        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e) {
            UpdateTexts(new List<int>() { e.eStr, e.eInt, e.eDex });
            UpdateCurrentPointsHUD();
            UpdateClass();
        }

        private void PointGained(PointGainedEvent e) {
            UpdateCurrentPointsHUD();
        }

        private void PointLost(PointLostEvent e) {
            UpdateCurrentPointsHUD();
        }
        #endregion

        public void StrButtonPressed() {
            GameManager.Instance.IncreaseSkill("str");
        }

        public void IntButtonPressed() {
            GameManager.Instance.IncreaseSkill("int");
        }

        public void DexButtonPressed() {
            GameManager.Instance.IncreaseSkill("dex");
        }
    }
}