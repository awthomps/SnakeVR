using UnityEngine;
using System.Collections;


public class SnakeSegmentBehavior : MonoBehaviour {
    public static string tagName = "SnakeSegmentTag";
    public static float radius = 1.0f;

    // Members
    private int mStackPosition = 0;
    private Transform mNextSegment = null;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;
    private Rigidbody rb;

    // Begin Unity Engine Code
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = tagName;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        // DESTROY ALL PHYSICS EFFECTS TO VELOCITY
        //rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    //End Unity Engine Code

    public void init(int stackPosition, Vector3 forward) {
        mStackPosition = stackPosition;
        transform.forward = forward;
    }

    // Add new end to the snake.
    public void addNewSnakeSegment(int stackPosition, Vector3 position, Quaternion rotation, Vector3 newForward)
    {
        //Increment the position
        stackPosition++;
        Vector3 oldForward = transform.forward;
        transform.forward = newForward;
        Vector3 lastPosition = transform.position;
        transform.position = position;
        if(isNotTail()) {
            //Not tail, so propogate call.
            mNextSnakeSegmentBehavior = mNextSegment.GetComponent<SnakeSegmentBehavior>();
            mNextSnakeSegmentBehavior.addNewSnakeSegment(stackPosition, lastPosition, rotation, oldForward);
        }
        else {
            //Is tail so create a new tail.
            mNextSegment = (Transform) Instantiate(transform, lastPosition, rotation);
            mNextSnakeSegmentBehavior = mNextSegment.GetComponent<SnakeSegmentBehavior>();
            mNextSnakeSegmentBehavior.init(stackPosition, oldForward);
        }
    }

    //Propogate the position down the snake to the tail.
    public void propogatePosition(Vector3 newPosition)
    {
        Vector3 lastPosition = transform.position;
        transform.position = newPosition;
        if(isNotTail()) {
            mNextSnakeSegmentBehavior.propogatePosition(lastPosition);
        }
        
    }

    public void move(Vector3 direction, float speed)
    {
        transform.forward = direction;
        transform.position += direction * speed;
    }

    public bool isNotTail() {
        return mNextSegment != null;
    }

    /*
    void OnCollisionEnter(Collision col)
    {
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (col.gameObject.name.Contains("SnakeSegment"))
        {
            print(col.gameObject.name);
            print("YOU LOSE!");
        }
        else if (col.gameObject.name.Contains("Apple"))
        {
            Destroy(col.gameObject);
            print("Apple consumed!");
        }
    }
    */
}
