namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    #region GameManager Events
    public class GameMenuEvent : SDD.Events.Event {
    }

    public class GamePlayEvent : SDD.Events.Event {
    }

    public class GamePauseEvent : SDD.Events.Event {
    }

    public class GameResumeEvent : SDD.Events.Event {
    }

    public class GameOverEvent : SDD.Events.Event {
    }

    public class GameVictoryEvent : SDD.Events.Event {
    }

    public class GameStatisticsChangedEvent : SDD.Events.Event {
        public int eShape { get; set; }
        public int eKnowledge { get; set; }
        public int eScience { get; set; }
        public int eSocial { get; set; }
    }

    public class PointGainedEvent : SDD.Events.Event {

    }

    public class PointLostEvent : SDD.Events.Event {

    }

    public class JobPopupEvent : SDD.Events.Event {
        public string eJob { get; set; }
    }
    #endregion

    #region MenuManager Events
    public class EscapeButtonClickedEvent : SDD.Events.Event {
    }

    public class PlayButtonClickedEvent : SDD.Events.Event {
    }

    public class ResumeButtonClickedEvent : SDD.Events.Event {
    }

    public class MainMenuButtonClickedEvent : SDD.Events.Event {
    }

    public class QuitButtonClickedEvent : SDD.Events.Event { }
    #endregion
}
