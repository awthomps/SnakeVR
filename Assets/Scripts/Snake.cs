using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour {
    public float cycleDistance = 0.022f; //determines the 
    public bool debug = true;
    public Vector3 appleSpawnUpperRight = new Vector3(7.0f, 0.5f, 7.0f);
    public Vector3 appleSpawnLowerLeft = new Vector3(-7.0f, 0.5f, -7.0f);
    public Object applePrefab;

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
            //mNextSnakeSegmentBehavior.grow(mSize * mIncreaseSpeedModifier * 10);
            mLastSize = mSize;
        }
            

        mSpawnTimer += Time.deltaTime;
        mMoveTimer += Time.deltaTime;
        Vector3 oldPosition = transform.position;
        //transform.position += (new Vector3(mStereoCamera.forward.x, 0.0f, mStereoCamera.forward.z).normalized * (cycleDistance + (mSize * mIncreaseSpeedModifier)));
        transform.position += (new Vector3(mStereoCamera.forward.x, 0.0f, mStereoCamera.forward.z).normalized * cycleDistance);

        if (mLost)
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
            spawnApple();
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

    private void spawnApple()
    {
        Vector3 newApplePosition = randomAppleLocationInRange();
        while(Physics.OverlapSphere(newApplePosition, 0.2f).Length > 0)
        {
            print("Stuck in loop");
            newApplePosition = randomAppleLocationInRange();
        }
        print(newApplePosition);
        //mNextSegment = (Transform)Instantiate(transform, lastPosition, rotation);
        GameObject newApple = (GameObject) Instantiate(applePrefab, newApplePosition, new Quaternion());
    }

    private Vector3 randomAppleLocationInRange()
    {
        float x, y, z;
        if(appleSpawnUpperRight.x == appleSpawnLowerLeft.x)
        {
            x = appleSpawnUpperRight.x;
        }
        else
        {
            x = (Random.value * (appleSpawnUpperRight.x - appleSpawnLowerLeft.x)) - ((appleSpawnUpperRight.x - appleSpawnLowerLeft.x) / 2);
        }

        if (appleSpawnUpperRight.y == appleSpawnLowerLeft.y)
        {
            y = appleSpawnUpperRight.y;
        }
        else
        {
            y = (Random.value * (appleSpawnUpperRight.y - appleSpawnLowerLeft.y)) - ((appleSpawnUpperRight.y - appleSpawnLowerLeft.y) / 2);
        }

        if (appleSpawnUpperRight.z == appleSpawnLowerLeft.z)
        {
            z = appleSpawnUpperRight.z;
        }
        else
        {
            z = (Random.value * (appleSpawnUpperRight.z - appleSpawnLowerLeft.z)) - ((appleSpawnUpperRight.z - appleSpawnLowerLeft.z) / 2);
        }
        return new Vector3(x, y, z); ;
    }
}
