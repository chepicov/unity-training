using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float slowdownFactor = 0.05f;

    public void DoSlowMotion() {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ReturnDefault() {
        Time.timeScale = 1.0f;
    }
}
