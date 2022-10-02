using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Clock : MonoBehaviour {
    void Update() {
        transform.RotateAround(transform.position, Vector3.forward, -34f * Time.deltaTime);
    }
}
