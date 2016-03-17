using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadTestLevelBehavior : MonoBehaviour {
    public float timeDelay = 10.0f;

    private float timeLeft = 0.0f;
	// Use this for initialization
	void Start () {
        timeLeft = timeDelay;
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;

        if(timeLeft <= 0.0f)
        {
            SceneManager.LoadScene("test");
        }
    }
}
