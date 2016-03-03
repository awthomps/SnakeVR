﻿using UnityEngine;
using System.Collections;


public class SnakeSegmentBehavior : MonoBehaviour {
    // Members
    private int mStackPosition = 0;
    private Transform mNextSegment = null;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;

    // Begin Unity Engine Code
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
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

}