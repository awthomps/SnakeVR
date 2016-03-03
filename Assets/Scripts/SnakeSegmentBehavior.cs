using UnityEngine;
using System.Collections;


public class SnakeSegmentBehavior : MonoBehaviour {
    // Members
    private int mStackPosition = 0;
    private GameObject mNextSegment = null;
	
    // Begin Unity Engine Code
    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    //End Unity Engine Code

    public void init(int stackPosition) {
        mStackPosition = stackPosition;
    }

    // Add new end to the snake.
    public void addNewSnakeSegment(int stackPosition, Vector3 position, Quaternion rotation)
    {
        //Increment the position
        stackPosition++;
        if(isNotTail()) {
            //Not tail, so propogate call.
            SnakeSegmentBehavior snakeSegmentBehavior = mNextSegment.GetComponent<SnakeSegmentBehavior>();
            snakeSegmentBehavior.addNewSnakeSegment(stackPosition, position, rotation);
        }
        else {
            //Is tail so create a new tail.
            mNextSegment = (GameObject) Instantiate((Object) transform.parent.gameObject, position, rotation);
            SnakeSegmentBehavior snakeSegmentBehavior = mNextSegment.GetComponent<SnakeSegmentBehavior>();
            snakeSegmentBehavior.init(stackPosition);
        }
    }

    // Propogate the position down the snake to the tail.
    public void propogatePosition(Vector3 newPosition)
    {
        Vector3 nextPosition = transform.position;
        transform.position = newPosition;
        
    }

    public bool isNotTail() {
        return mNextSegment != null;
    }

}
