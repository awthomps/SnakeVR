using UnityEngine;
using System.Collections;

public class Snake : MonoBehaviour {
    public Object prefab;
    private GameObject head;
    private float speed = 0.01f;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;
    private Transform stereoCamera;
    private float spawnTimer = 0.0f;
    private float spawnTimeDelay = 5.0f;
    private float moveTimer = 0.0f;
    private float moveTimeDelay = 1.0f;

    // Use this for initialization
    void Start () {
        // Create the snake head.
        head = (GameObject) Instantiate(prefab, transform.position, transform.rotation);
        mNextSnakeSegmentBehavior = head.GetComponent<SnakeSegmentBehavior>();
        mNextSnakeSegmentBehavior.init(0, transform.forward);

        //Acquire Camera for directions
        stereoCamera = transform.FindChild("Head").FindChild("Main Camera");
    }
	
	// Update is called once per physics cycle
	void FixedUpdate () {
        spawnTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;
        Vector3 oldPosition = transform.position;
        transform.position += (stereoCamera.forward.normalized * speed);

        if (moveTimer >= moveTimeDelay)
        {
            if (spawnTimer >= spawnTimeDelay)
            {
                spawnTimer = 0.0f;
                mNextSnakeSegmentBehavior.addNewSnakeSegment(0, transform.position, transform.rotation, stereoCamera.forward);
            }
            else
            {
                mNextSnakeSegmentBehavior.propogatePosition(transform.position);
            }
            moveTimer = 0.0f;
        }
    }
}
