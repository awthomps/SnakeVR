using UnityEngine;
using System.Collections;

public class Snake : MonoBehaviour {
    public float cycleDistance = 0.022f; //determines the 
    public bool debug = true;


    public Object prefab;
    private GameObject head;
    private float speed = 0.022f;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;
    private Transform stereoCamera;
    private float spawnTimer = 0.0f;
    private float spawnTimeDelay = 2.0f;
    private float moveTimer = 0.0f; // depends on cycleDistance
    private float moveTimeDelay = 1.0f;

    // Use this for initialization
    void Start () {
        // Create the snake head.
        head = (GameObject) Instantiate(prefab, transform.position, transform.rotation);
        mNextSnakeSegmentBehavior = head.GetComponent<SnakeSegmentBehavior>();
        mNextSnakeSegmentBehavior.init(0, transform.forward);

        //force cycle distance dependency.
        moveTimeDelay = speed / cycleDistance;

        //Acquire Camera for directions
        stereoCamera = transform.FindChild("Head").FindChild("Main Camera");
    }
	
	// Update is called once per physics cycle
	void FixedUpdate () {

        //force cycle distance dependency.
        if(debug)
        {
            moveTimeDelay = speed / cycleDistance;
        }

        spawnTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;
        Vector3 oldPosition = transform.position;
        transform.position += ( new Vector3(stereoCamera.forward.x, 0.0f, stereoCamera.forward.z).normalized * cycleDistance);

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
