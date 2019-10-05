using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] TimeManager timeManager;
    Rigidbody playerRigidbody;
    private static GameManager instance;
    private static int ActiveLevel = -1;
    static int[] WinPoints = { 10, 15 };

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameManager";
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        playerRigidbody = Player.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PausePlayer(playerRigidbody);
    }

    public void LoadLevel()
    {
        if (ActiveLevel > -1)
        {
            SceneManager.UnloadSceneAsync(SceneConstants.Levels[ActiveLevel].Id);
        }
        ActiveLevel = Mathf.Min(ActiveLevel + 1, SceneConstants.Levels.Length - 1);
        SceneManager.LoadScene(SceneConstants.Levels[ActiveLevel].Id, LoadSceneMode.Additive);

        float delayTime = 0.2f;
        playerRigidbody.velocity = Vector3.zero;
        Player.transform.position = new Vector3(0, 0, 0);
        Player.transform.rotation = Quaternion.identity;
        StartCoroutine(StartPhysics(playerRigidbody, delayTime));
    }

    IEnumerator StartPhysics(Rigidbody rb, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneConstants.Levels[ActiveLevel].Id));
        playerRigidbody.isKinematic = false;
        UIController.Instance.ShowPlay();
    }

    public void ChangeScore(int points)
    {
        UIController.Instance.SetScore(points.ToString());
        if (points >= WinPoints[ActiveLevel]) {
            BallFactory.Instance.Stop();
            UIController.Instance.ShowWin();
            // PausePlayer(Instance.playerRigidbody);
            timeManager.DoSlowMotion();
        }
    }

    static void PausePlayer(Rigidbody playerRigidbody)
    {
        playerRigidbody.isKinematic = true;
    }
}
