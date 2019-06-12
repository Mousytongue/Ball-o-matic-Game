using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    float LVL_ONE_TIMER = -10;          //Set to -10 to give delay before starting
    float LVL_TWO_TIMER = -10;
    float LVL_THREE_TIMER = -10;
    int LVL_ONE_COUNTER = 1;
    int LVL_TWO_COUNTER = 1;
    int LVL_THREE_COUNTER = 1;
    float LVL_ONE_SPAWN_RATE = 3f;
    float LVL_TWO_SPAWN_RATE = 3.5f;
    float LVL_THREE_SPAWN_RATE = 4;
    float BALL_SPAWN_TIMER = 0;
    float BALL_SPAWN_RATE = 7;


    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
        //Lock the cursor to the Game window
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        LevelOneSpawner();
        LevelTwoSpawner();
        LevelThreeSpawner();
        BallSpawner(); 
    }

    void LevelOneSpawner()
    {
        //Level 1 
        LVL_ONE_TIMER += Time.deltaTime;
        if (LVL_ONE_TIMER >= LVL_ONE_SPAWN_RATE)
        {
            //Spawn object
            if (LVL_ONE_COUNTER == 5)
            {
                GameObject redPrefab = Resources.Load("Prefabs/Prefab Objects/LvlOneRedTarget") as GameObject;
                GameObject redTarget = Instantiate(redPrefab) as GameObject;
                redTarget.transform.localPosition = new Vector3(58.75f, -1.7f, 60f);
                LVL_ONE_COUNTER = 0;
            }
            else
            {
                GameObject greenPrefab = Resources.Load("Prefabs/Prefab Objects/LvlOneGreenTarget") as GameObject;
                GameObject greenTarget = Instantiate(greenPrefab) as GameObject;
                greenTarget.transform.localPosition = new Vector3(58.75f, -1.7f, 60f);
            }

            LVL_ONE_COUNTER += 1;
            LVL_ONE_TIMER = 0;
        }
    }

    void LevelTwoSpawner()
    {
        //Level 2 
        LVL_TWO_TIMER += Time.deltaTime;
        if (LVL_TWO_TIMER >= LVL_TWO_SPAWN_RATE)
        {
            //Spawn object
            if (LVL_TWO_COUNTER == 3)
            {
                GameObject redPrefab = Resources.Load("Prefabs/Prefab Objects/LvlTwoRedTarget") as GameObject;
                GameObject redTarget = Instantiate(redPrefab) as GameObject;
                redTarget.transform.localPosition = new Vector3(-58.75f, 8f, 78f);
                LVL_TWO_COUNTER = 0;
            }
            else
            {
                GameObject greenPrefab = Resources.Load("Prefabs/Prefab Objects/LvlTwoGreenTarget") as GameObject;
                GameObject greenTarget = Instantiate(greenPrefab) as GameObject;
                greenTarget.transform.localPosition = new Vector3(-58.75f, 8f, 78f);
            }

            LVL_TWO_COUNTER += 1;
            LVL_TWO_TIMER = 0;
        }
    }

    void LevelThreeSpawner()
    {
        //Level 3
        LVL_THREE_TIMER += Time.deltaTime;
        if (LVL_THREE_TIMER >= LVL_THREE_SPAWN_RATE)
        {
            //Spawn object
            if (LVL_THREE_COUNTER == 2)
            {
                GameObject redPrefab = Resources.Load("Prefabs/Prefab Objects/LvlOneRedTarget") as GameObject;
                GameObject redTarget = Instantiate(redPrefab) as GameObject;
                redTarget.transform.localPosition = new Vector3(58.75f, 17f, 97f);
                LVL_THREE_COUNTER = 0;
            }
            else
            {
                GameObject greenPrefab = Resources.Load("Prefabs/Prefab Objects/LvlOneGreenTarget") as GameObject;
                GameObject greenTarget = Instantiate(greenPrefab) as GameObject;
                greenTarget.transform.localPosition = new Vector3(58.75f, 17f, 97f);
            }

            LVL_THREE_COUNTER += 1;
            LVL_THREE_TIMER = 0;
        }
    }

    void BallSpawner()
    {
        BALL_SPAWN_TIMER += Time.deltaTime;
        if (BALL_SPAWN_TIMER >= BALL_SPAWN_RATE)
        {
            //Spawn object
            foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                GameObject spherePrefab = Resources.Load("Prefabs/Prefab Objects/Sphere") as GameObject;
                GameObject sphere = Instantiate(spherePrefab) as GameObject;
                sphere.transform.localPosition = spawnPoint.transform.position + transform.up * 4f + new Vector3(0, 0, 4);
            }

            //Rail Upward Return spawner
            GameObject RailPrefab1 = Resources.Load("Prefabs/Prefab Objects/LeftUpwardBallReturn") as GameObject;
            GameObject Rail1 = Instantiate(RailPrefab1) as GameObject;
            Rail1.transform.localPosition = new Vector3(-48.5f, -13, 52.3f);

            GameObject RailPrefab2 = Resources.Load("Prefabs/Prefab Objects/RightUpwardBallReturn") as GameObject;
            GameObject Rail2 = Instantiate(RailPrefab2) as GameObject;
            Rail2.transform.localPosition = new Vector3(47, -13, 52.3f);

            BALL_SPAWN_TIMER = 0;
        }
    }

    public void IncreaseSpawnRate()
    {
        if (BALL_SPAWN_RATE >= 1.5)
        {
            BALL_SPAWN_RATE -= 0.5f;
        }
    }
}