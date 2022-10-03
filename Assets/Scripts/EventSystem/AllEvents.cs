namespace EventsManager {
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

    public class GameStatisticsChangedEvent : SDD.Events.Event {
        public int eStr { get; set; }
        public int eInt { get; set; }
        public int eDex { get; set; }
        public int eHealth { get; set; }
    }

    public class PointGainedEvent : SDD.Events.Event {

    }

    public class PointLostEvent : SDD.Events.Event {

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
