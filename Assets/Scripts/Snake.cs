﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour {
    public float cycleDistance = 0.022f; //determines the 
    public bool debug = true;


    public Object prefab;
    private string gameOverScene = "gameovertest";

    private GameObject mHead;
    private float speed = 0.022f;
    private SnakeSegmentBehavior mNextSnakeSegmentBehavior;
    private Transform mStereoCamera;
    private float mSpawnTimer = 0.0f;
    private float mMoveTimer = 0.0f; // depends on cycleDistance
    private float mMoveTimeDelay = 1.0f;
    private int mNumSegmentsToAdd = 0;
    private bool mLost = false;
    private int mSize = 0;
    private int mLastSize = 0;
    private float mIncreaseSpeedModifier = 0.001f;
    private Text[] scoreTexts;
    private int score;

    // Use this for initialization
    void Start () {
        // Create the snake head.
        mHead = (GameObject) Instantiate(prefab, transform.position, transform.rotation);
        mNextSnakeSegmentBehavior = mHead.GetComponent<SnakeSegmentBehavior>();
        mNextSnakeSegmentBehavior.init(0, transform.forward);

        //force cycle distance dependency.
        mMoveTimeDelay = speed / cycleDistance;

        //Acquire Camera for directions
        mStereoCamera = transform.FindChild("Head").FindChild("Main Camera");

        //Initialize Scoring text
        InitScoreText();
    }
	
	// Update is called once per physics cycle
	void FixedUpdate ()
    {

        //force cycle distance dependency.
        if (debug)
        {
            mMoveTimeDelay = speed / cycleDistance;
        }
        if (mSize != mLastSize)
        {
            //TODO: FIX THIS
            mNextSnakeSegmentBehavior.grow(mSize * mIncreaseSpeedModifier * 10);
            mLastSize = mSize;
        }
            

        mSpawnTimer += Time.deltaTime;
        mMoveTimer += Time.deltaTime;
        Vector3 oldPosition = transform.position;
        transform.position += (new Vector3(mStereoCamera.forward.x, 0.0f, mStereoCamera.forward.z).normalized * (cycleDistance + (mSize * mIncreaseSpeedModifier)));

        if(mLost)
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else if (mMoveTimer >= mMoveTimeDelay)
        {
            if (mNumSegmentsToAdd > 0)
            {
                IncrementScore();
                mSpawnTimer = 0.0f;
                mNextSnakeSegmentBehavior.addNewSnakeSegment(0, transform.position, transform.rotation, mStereoCamera.forward);
                mNumSegmentsToAdd--;
                mSize++;
            }
            else
            {
                mNextSnakeSegmentBehavior.propogatePosition(transform.position);
            }
            mMoveTimer = 0.0f;
            Vector3 forwardXZ = new Vector3(mStereoCamera.forward.x, 0.0f, mStereoCamera.forward.z);
            mNextSnakeSegmentBehavior.lookForward(transform.position + forwardXZ);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("WallTag") || (col.name.Contains("SnakeSegment") && isNotHead(col)))
        {
            print(col.name);
            print("YOU LOSE!");
            mLost = true;
        }
        else if (col.CompareTag(AppleBehavior.tagName))
        {
            Destroy(col.gameObject);
            mNumSegmentsToAdd++;
        }
    }

    bool isNotHead(Collider col)
    {
        return (((SnakeSegmentBehavior)col.GetComponent<SnakeSegmentBehavior>()).stackPosition()) != mNextSnakeSegmentBehavior.stackPosition();
    }

    private void InitScoreText()
    {
        score = 0;
        scoreTexts = new Text[2];
        int scoreTextsIndex = 0;
        Text[] textObjects = GameObject.FindObjectsOfType<Text>();
        foreach (Text text in textObjects)
        {
            if (text.name.Contains("Score"))
            {
                scoreTexts[scoreTextsIndex] = text;
                scoreTextsIndex++;
            }
        }
    }

    private void IncrementScore()
    {
        score++;
        foreach (Text text in scoreTexts)
        {
            if(text != null)
            {
                text.text = "Score: " + score;
            }
        }
    }
}
