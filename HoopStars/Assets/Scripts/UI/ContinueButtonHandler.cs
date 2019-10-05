using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
    [SerializeField] TimeManager timeManager;

    public void OnClick() {
        timeManager.ReturnDefault();
        GameManager.Instance.LoadLevel();
        Debug.Log(2);
        UIController.Instance.ShowPlay();
    }
}
