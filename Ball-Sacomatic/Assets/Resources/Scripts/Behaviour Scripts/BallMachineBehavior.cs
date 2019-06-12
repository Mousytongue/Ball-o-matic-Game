using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallMachineBehavior : MonoBehaviour
{
    private float MACHINE_MOVE_SPEED = 20f;
    public Transform mCannonSceneNode;
    public Transform mCannonGeom;
    public Transform mSackGeom;
    public Transform mSackSceneNode;
    public Transform mCannonCam;
    public GameObject mExplosion;
    Transform mSmallCam;
    Transform mMainCam;
    Transform mCannonCamPos;
    Transform mCannonCamLook;
    SmallCameraControl mSmallScript;
    SmallCameraControl mMainScript;

    float MAX_MOVE_X = 45; 
    float MAX_MOVE_Z = 12;
    float SACK_SIZE_LIMIT = 30; //30 balls
    float CANNON_FIRE_SPEED = .5f;
    float fireCounter = 0; 
    bool isDead = false;
    bool isMainView = true;

    //For cannon-camera movement
    float RotateDeltaX = 0.0f;
    float RotateDeltaY = 0.0f;
    float mRotSpeed = 2.0f;
    float MIN_VIEW_RANGE = -25;
    float MAX_VIEW_RANGE = 45;

    //Player's Score, balls and sack
    private int score = 0;
    public TextMeshProUGUI scoreBoard = null;
    int ballcount = 0;
    Vector3 initSackSize;

    public Camera mSmallCamera = null;

    //Streak and points
    int streak = 0;

    //Audio of vacuum
    AudioSource vacuumSound, cannonSound;

    // Use this for initialization
    void Start()
    {
        //mFloorRadius = mFloor.localScale.x / 2;
        RotateDeltaX = mCannonSceneNode.eulerAngles.x;
        RotateDeltaY = -90;
        //RotateDeltaY = mCannonSceneNode.eulerAngles.y;
        initSackSize = GameObject.FindGameObjectWithTag("Sack").transform.localScale;

        mSmallCam = GameObject.FindGameObjectWithTag("SmallCamera").transform;
        mMainCam = GameObject.FindGameObjectWithTag("LargeCamera").transform;
        mCannonCamPos = GameObject.FindGameObjectWithTag("SmallCameraPos").transform;
        mCannonCamLook = GameObject.FindGameObjectWithTag("SmallCameraLookAt").transform;
        mSmallScript = mSmallCam.GetComponent<SmallCameraControl>();
        mMainScript = mMainCam.GetComponent<SmallCameraControl>();
        SwapCameras();

        //Audios
        AudioSource[] audios = GetComponents<AudioSource>();
        vacuumSound = audios[0]; //Not really used at the moment, it's in loop mode
        cannonSound = audios[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            ProcessMachineMovement();
            ProcessCannonRotation();
            FireCannon();
            Death();

            if (Input.GetKeyDown(KeyCode.Space))
                SwapCameraMode();    
        }
    }
    void Death()
    {
        //Death We could put a cool script here, perhaps call for the main camera to overtake
        //The overhead view, show explosion, and have a "Game Over" screen with reset button
        if (ballcount > SACK_SIZE_LIMIT)
        {
            //could also add audio file here for explosion.

            Debug.Log("Death Called");
            Instantiate(mExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            isDead = true;
        }
    }

    void FireCannon()
    {
        //Fires the cannonball
        fireCounter += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (fireCounter >= CANNON_FIRE_SPEED)
            {
                GameObject mSack = GameObject.FindGameObjectWithTag("Sack");
                if (ballcount > 0)
                {
                    cannonSound.Play(0); //turned back on because of click shoot
                    GameObject spherePrefab = Resources.Load("Prefabs/Prefab Objects/Sphere") as GameObject;
                    GameObject sphere = Instantiate(spherePrefab) as GameObject;
                    sphere.transform.localPosition = mSmallCamera.transform.localPosition;
                    Rigidbody rg = sphere.GetComponent<Rigidbody>();
                    rg.AddForce(mSmallCamera.transform.forward * 75, ForceMode.Impulse);
                    AddRemoveBalls(-1); //Scales and decrements ball count
                }
                fireCounter = 0;
            }
        }
    }

    void ProcessCannonRotation()
    {
        //Cannon movement
        RotateDeltaY += mRotSpeed * Input.GetAxis("Mouse X");
        RotateDeltaX += mRotSpeed * Input.GetAxis("Mouse Y");
        RotateDeltaX = Mathf.Clamp(RotateDeltaX, MIN_VIEW_RANGE, MAX_VIEW_RANGE);
        mSackSceneNode.rotation = Quaternion.Euler(0, RotateDeltaY, RotateDeltaX);

        //Handles the cannons relative growth with the sack
        SceneNode SN = mCannonSceneNode.GetComponent<SceneNode>();
        SN.NodeOrigin.y = mSackGeom.localScale.y / 2;
        mCannonGeom.localScale = new Vector3(1, mSackGeom.localScale.y / 2, 1);
        mCannonCam.localPosition = new Vector3(-(mSackGeom.localScale.y / 2), 2.5f, 0);
    }

    void SwapCameraMode()
    {

        if (isMainView == true)
        {
            isMainView = false;
            SwapCameras();
        }
        else
        {
            isMainView = true;
            SwapCameras();
        }
    }

    void SwapCameras()
    {
        
        mSmallScript.CameraHead = null;
        mSmallScript.LookAtPosition = null;
        mMainScript.CameraHead = null;
        mMainScript.LookAtPosition = null;
        
        if (isMainView == true)
        {
            mSmallCam.position = new Vector3(0, 30f, -50f);
            mSmallCam.eulerAngles = new Vector3(13.2f, 0, 0);

            mMainScript.CameraHead = mCannonCamPos;
            mMainScript.LookAtPosition = mCannonCamLook;
            mSmallCamera = mMainCam.transform.GetComponent<Camera>();
        }
        else
        {
            mMainCam.position = new Vector3(0, 30f, -50f);
            mMainCam.eulerAngles = new Vector3(13.2f, 0, 0);

            mSmallScript.CameraHead = mCannonCamPos;
            mSmallScript.LookAtPosition = mCannonCamLook;
            mSmallCamera = mSmallCam.transform.GetComponent<Camera>();
        }
    }

    void ProcessMachineMovement()
    {
        if (isMainView == false)
        {
            //Movement for overhead WASD controls
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                Vector3 inputVector = new Vector3(h, 0, v);

                inputVector = Vector3.ClampMagnitude(inputVector, 1);                                //this clamps the movement speed, so diag movements arent double speed
                Vector3 newPos = transform.localPosition += inputVector * MACHINE_MOVE_SPEED/50;
                newPos.x = Mathf.Clamp(newPos.x, -MAX_MOVE_X, MAX_MOVE_X);                                       //this bounds the object within the playing area
                newPos.z = Mathf.Clamp(newPos.z, -MAX_MOVE_Z, MAX_MOVE_Z);                                       //this bounds the object within the playing area
                transform.localPosition = newPos;
            }
        }
        else
        {

            //Movement for first person WASD controls
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Vector3 inputVector = new Vector3(0, 0, 0);
                if (Input.GetKey(KeyCode.W))
                    inputVector += mSackGeom.transform.right * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    inputVector += -mSackGeom.transform.right * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    inputVector += mSackGeom.transform.forward * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    inputVector += -mSackGeom.transform.forward * Time.deltaTime;

                inputVector = Vector3.ClampMagnitude(inputVector, 1);
                Vector3 newPos = transform.localPosition += inputVector * MACHINE_MOVE_SPEED;
                newPos.x = Mathf.Clamp(newPos.x, -MAX_MOVE_X, MAX_MOVE_X);
                newPos.z = Mathf.Clamp(newPos.z, -MAX_MOVE_Z, MAX_MOVE_Z);
                newPos.y = Mathf.Clamp(newPos.y, 0, 0);
                transform.localPosition = newPos;
            }
        }
    }

    //This is called by the targets, and their collision script to update the score
    public void UpdateScore(int amount)
    {
        if (score + amount < 0)
            score = 0;
        else
            score += amount;
        ShowScore();
    }

    //Called by sphere behavior when balls are sucked in and this script when balls are fired
    //Changes ballcount by ammount, and manages the scaling of the sphere
    public void AddRemoveBalls(int amount)
    {
        if (ballcount + amount < 0)
        {
            ballcount = 0;
            GameObject.FindGameObjectWithTag("Sack").transform.localScale = initSackSize;
        }
        else
        {
            ballcount += amount;
            float scale = ballcount * .1f;
            GameObject.FindGameObjectWithTag("Sack").transform.localScale = initSackSize + new Vector3(scale, scale, scale);
        }

        //if(scoreBoard.color.r + amount * 10 >= 0 || scoreBoard.faceColor.r <= 255) //10 balls will make it super red
        //    scoreBoard.color += new Color(amount, 0f, 0f); //The color will get more red as the sack fills

        ShowScore();
    }

    public int GetStreak()
    {
        return streak;
    }

    public void IncrementStreak()
    {
        streak++;
    }

    public void ResetStreak()
    {
        streak = 0;
    }

    public void ShowScore()
    {
        scoreBoard.text = "Score: " + score.ToString("000") + "\nBalls: " + ballcount + "\nStreak: " + streak;
    }
}