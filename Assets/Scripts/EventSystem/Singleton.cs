using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component {
    static T _Instance;

    public static T Instance {
        get { return _Instance; }
    }

    [Header("Singleton")]
    [SerializeField] private bool _DoNotDestroyGameObjectOnLoad;

    protected virtual void Awake() {
        if (_Instance != null)
            Destroy(gameObject);
        else _Instance = this as T;

        if (_DoNotDestroyGameObjectOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
