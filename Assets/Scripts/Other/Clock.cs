using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using SDD.Events;
using EventsManager;

public class Clock : SimpleGameStateObserver {
    protected override void GamePlay(GamePlayEvent e) {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate() {
        int i = 0;
        while(i < 10) {
            yield return new WaitForSeconds(1);
            transform.RotateAround(transform.position, Vector3.forward, -36f);
        }
        StartCoroutine(Rotate());
    }
}
