using UnityEngine;
using System.Collections;

public class Snake : MonoBehaviour {
    public Object prefab;
    private GameObject head;

	// Use this for initialization
	void Start () {
        // Create the snake head.
        print(head);
        head = (GameObject) Instantiate(prefab, transform.position, transform.rotation);
        SnakeSegmentBehavior snakeSegmentBehavior = head.GetComponent<SnakeSegmentBehavior>();
        snakeSegmentBehavior.init(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
