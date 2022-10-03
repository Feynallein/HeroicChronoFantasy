using FMODUnity;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class EventReferenceDictionary : SerializableDictionaryBase<string, EventReference> { }


public static class SfxManager {
    static public void PlayOneShot(EventReference eventReference, Transform position, Dictionary<string, int> parameters = null) {
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        GameObject emptyGo = RuntimeManager.Instantiate(new GameObject());
        RuntimeManager.AttachInstanceToGameObject(eventInstance, emptyGo.transform);


        if (parameters != null) {
            foreach (KeyValuePair<string, int> entry in parameters)
                eventInstance.setParameterByName(entry.Key, entry.Value);
        }

        eventInstance.start();
        eventInstance.release();
    }
}