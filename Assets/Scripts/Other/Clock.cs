using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Clock : MonoBehaviour {

    private void Awake() {
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
