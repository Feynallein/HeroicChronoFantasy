namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Manager<T> : SingletonGameStateObserver<T> where T : Component {
        protected bool _IsReady = false;
        public bool IsReady { get { return _IsReady; } }

        protected abstract IEnumerator InitCoroutine();

        // Use this for initialization
        protected virtual IEnumerator Start() {
            _IsReady = false;
            yield return StartCoroutine(InitCoroutine());
            _IsReady = true;
        }
    }
}
