using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMainMenuWithDelayBehavior: MonoBehaviour {
    public float timeDelay = 5.0f;

    private string mainMenu = "mainmenu";
    private float timeLeft = 0.0f;
    // Use this for initialization
    void Start()
    {
        timeLeft = timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0.0f)
        {
            SceneManager.LoadScene(mainMenu);
        }
    }
}
