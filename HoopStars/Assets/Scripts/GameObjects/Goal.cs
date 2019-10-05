using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;

public class Goal : MonoBehaviour
{
    string[] triggerArray = { "TopTrigger", "BottomTrigger" };
    string activeTrigger = "";
    static bool IsActive = true;

    private void OnTriggerEnter(Collider other) {
        if (IsActive == false) {
            return;
        }
        string currentTrigger = other.gameObject.name;
        if (System.Array.IndexOf(triggerArray, currentTrigger) > -1
            && activeTrigger != ""
            && activeTrigger != currentTrigger) {
            BallFactory.Instance.IncPoints();
            TapticManager.Impact(ImpactFeedback.Medium);
            Invoke("ChangePosition", 0.3f);
            activeTrigger = "";
            return;
        }
        if (System.Array.IndexOf(triggerArray, currentTrigger) > -1) {
            activeTrigger = currentTrigger;
        }
    }

    void OnTriggerExit(Collider other)
    {
        string currentTrigger = other.gameObject.name;
        if (System.Array.IndexOf(triggerArray, currentTrigger) > -1 && activeTrigger != "") {
            activeTrigger = "";
        }
    }

    public static void StopTrigger() {
        IsActive = false;
    }

    public static void RunTrigger() {
        IsActive = true;
    }

    public void ChangePosition() {
        if (IsActive == false) {
            return;
        }
        float minX = -2.0f;
        float maxX = 2.0f;
        float minY = -1.0f;
        float maxY = 3.0f;

        float randomX = 0.0f;
        float randomY = 0.0f;
        Collider[] hitColliders;

        do {
            randomX = Random.Range(minX, maxX);
            randomY = Random.Range(minY, maxY);

            Vector3 newPosition = new Vector3(randomX, randomY, 0.0f);
            hitColliders = Physics.OverlapSphere(newPosition, 1.0f);
        } while (hitColliders.Length > 0);

        transform.position = new Vector3(randomX, randomY, 0f);
    }
}
