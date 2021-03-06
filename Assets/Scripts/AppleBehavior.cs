﻿using UnityEngine;
using System.Collections;

public class AppleBehavior : MonoBehaviour {
    public static string tagName = "AppleTag";
    public static float radius = 1.0f;
    public float speed = 2.0f;
    public float oscillatation = 0.0675f;
    private float time = 0.0f;
    private float startY;

	// Use this for initialization
	void Start ()
    {
        gameObject.tag = tagName;
        startY = transform.position.y;
        time = 1 - (2 * Random.value);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}

    void FixedUpdate()
    {
        time += Time.deltaTime;
        float height = (oscillatation * Mathf.Sin(speed * time)) + startY;
        Vector3 pos = new Vector3(transform.position.x, height, transform.position.z);
        transform.position = pos;
    }
}
