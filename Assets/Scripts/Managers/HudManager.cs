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
        [SerializeField] GameObject _JobPopup;
        [SerializeField] GameObject _LeftPanel;

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
            EventManager.Instance.AddListener<JobPopupEvent>(JobPopup);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<PointGainedEvent>(PointGained);
            EventManager.Instance.RemoveListener<PointLostEvent>(PointLost);
            EventManager.Instance.RemoveListener<JobPopupEvent>(JobPopup);
        }

        private void JobPopup(JobPopupEvent e) {
            _JobPopup.GetComponentInChildren<Text>().text += e.eJob;
            _JobPopup.SetActive(true);
            _LeftPanel.SetActive(false);
        }

        public void JobPopupButtonClicked() {
            _JobPopup.SetActive(false);
            Time.timeScale = 1;
        }

        private void ShowButtons(bool value) {
            _HUDSkillsButtons.ForEach(button => button.SetActive(value));
        }

        private void UpdateCurrentPointsHUD() {
            _CurrentPointsHUD.text = "Current Points: " + GameManager.Instance.CurrentPoints;
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
            UpdateTexts(new List<int>() { e.eShape, e.eKnowledge, e.eScience, e.eSocial});
            UpdateCurrentPointsHUD();
        }

        private void PointGained(PointGainedEvent e) {
            UpdateCurrentPointsHUD();
        }

        private void PointLost(PointLostEvent e) {
            UpdateCurrentPointsHUD();
        }
        #endregion

        public void ShapeButtonPressed() {
            GameManager.Instance.IncreaseSkill("Shape");
        }

        public void KnowledgeButtonPressed() {
            GameManager.Instance.IncreaseSkill("Knowledge");
        }

        public void ScienceButtonPressed() {
            GameManager.Instance.IncreaseSkill("Science");
        }

        public void SocialButtonPressed() {
            GameManager.Instance.IncreaseSkill("Social");
        }
    }
}