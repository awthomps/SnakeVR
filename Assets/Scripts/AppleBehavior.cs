using UnityEngine;
using System.Collections;

public class AppleBehavior : MonoBehaviour {
    public static string tagName = "AppleTag";
    public static float radius = 1.0f;
	// Use this for initialization
	void Start () {
        gameObject.tag = tagName;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    //
    void FixedUpdate()
    {

    }
}
