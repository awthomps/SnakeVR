using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIEventSystemReticleInteractions : MonoBehaviour {
    public float timeUntilTrigger = 3.0f;

    private string arcadeModeScene = "";
    private string testScene = "test";
    private string largeTestMapScene = "largetestmap";
    private float elapsedTime = 0.0f;
    private bool isHighlighted;
    // Use this for initialization
    void Start () {
        isHighlighted = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isHighlighted)
        {
            print("Is Highlighted");
            elapsedTime += Time.deltaTime;
            selecting();
            if (elapsedTime >= timeUntilTrigger)
            {
                engageTrigger();
            }
        }
        else if(elapsedTime > 0.0f)
        {
            elapsedTime -= Time.deltaTime;
            unselect();
        }
        

    }

    public void Test()
    {

    }
    private void selecting()
    {
        Component[] gameObjects = gameObject.GetComponentsInParent(typeof(Text));
        Color color;
        foreach (Text text in gameObjects)
        {
            color = text.color;
            color.a = 1.0f - (elapsedTime / timeUntilTrigger);
            if (color.a < 0)
                color.a = 0.0f;
            text.color = color;
        }
    }

    private void unselect()
    {
        Component[] gameObjects = gameObject.GetComponentsInParent(typeof(Text));
        Color color;
        foreach (Text text in gameObjects)
        {
            color = text.color;
            color.a = 1.0f - (elapsedTime / timeUntilTrigger);
            if (color.a > 1.0f)
                    color.a = 1.0f;
            text.color = color;
        }
    }

    void engageTrigger()
    {
        print("Trigger engaged");
        if (gameObject.name.Contains("TestMode"))
            SceneManager.LoadScene(testScene);
        else if (gameObject.name.Contains("LargeTestMap"))
            SceneManager.LoadScene(largeTestMapScene);
    }
    public void highlight(bool highlight)
    {
        isHighlighted = highlight;
    }
}
