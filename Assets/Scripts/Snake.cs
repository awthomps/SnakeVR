using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour {
    public float cycleDistance = 0.022f; //determines the 
    public bool debug = true;


    public Object prefab;
    private string gameOverScene = "gameovertest";
    private GameObject head;
    private float speed = 0.022f;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;
    private Transform stereoCamera;
    private float spawnTimer = 0.0f;
    private float moveTimer = 0.0f; // depends on cycleDistance
    private float moveTimeDelay = 1.0f;
    private int mNumSegmentsToAdd = 0;
    private bool lost = false;

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
	void FixedUpdate ()
    {
        HandleCollisions();

        //force cycle distance dependency.
        if (debug)
        {
            moveTimeDelay = speed / cycleDistance;
        }

        spawnTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;
        Vector3 oldPosition = transform.position;
        transform.position += (new Vector3(stereoCamera.forward.x, 0.0f, stereoCamera.forward.z).normalized * cycleDistance);

        if(lost)
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else if (moveTimer >= moveTimeDelay)
        {
            if (mNumSegmentsToAdd > 0)
            {
                spawnTimer = 0.0f;
                mNextSnakeSegmentBehavior.addNewSnakeSegment(0, transform.position, transform.rotation, stereoCamera.forward);
                mNumSegmentsToAdd--;
            }
            else
            {
                mNextSnakeSegmentBehavior.propogatePosition(transform.position);
            }
            moveTimer = 0.0f;
        }
    }

    private void HandleCollisions()
    {
        HandleSnakeSegmentCollisions();
        HandleAppleCollisions();
    }

    private void HandleSnakeSegmentCollisions()
    {
        //Check for snake segments to see if game over (ignore head)
        GameObject[] snakeSegments = GameObject.FindGameObjectsWithTag(SnakeSegmentBehavior.tagName);
        foreach (GameObject snakeSegment in snakeSegments)
        {
            float distanceToSnakeSegment = Vector3.Distance(transform.position, snakeSegment.transform.position);
            if (distanceToSnakeSegment <= SnakeSegmentBehavior.radius)
            {
                if (snakeSegment.name.Contains("SnakeSegment") && snakeSegment != head)
                {
                    print(snakeSegment.name);
                    print("YOU LOSE!");
                    lost = true;
                }
            }
        }
    }

    private void HandleAppleCollisions()
    {
        //Find any apples that may have been consumed.
        GameObject[] apples = GameObject.FindGameObjectsWithTag(AppleBehavior.tagName);
        List<GameObject> applesToDestroy = new List<GameObject>();
        foreach (GameObject apple in apples)
        {
            float distancetoApple = Vector3.Distance(transform.position, apple.transform.position);
            if (distancetoApple <= AppleBehavior.radius)
            {
                applesToDestroy.Add(apple);
            }
        }

        //Destroy consumed apple and mark for adding segments:
        foreach (GameObject apple in applesToDestroy)
        {
            //TODO: carry out special case for apple if there is one:
            Destroy(apple);
            mNumSegmentsToAdd++;
        }
    }
}
