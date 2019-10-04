using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject PlayPanel;
    [SerializeField] GameObject StartPanel;
    private static UIController instance;

    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "UIController";
                    instance = obj.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EnablePanel(StartPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnablePanel(GameObject panel) {
        GameObject[] menuPanels = { WinPanel, PlayPanel, StartPanel };
        foreach (var menuPanel in menuPanels)
        {
            menuPanel.SetActive(false);
        }
        panel.SetActive(true);
    }

    public static void SetScore(string score) {
        Instance.ScoreText.text = score;
    }

    public static void ShowPlay() {
        Instance.EnablePanel(Instance.PlayPanel);
    }

    public static void ShowWin() {
        Instance.ScoreText.text = "Win";
        Instance.EnablePanel(Instance.WinPanel);
    }
}
