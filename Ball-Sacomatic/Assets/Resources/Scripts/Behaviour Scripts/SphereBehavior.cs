using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehavior : MonoBehaviour {

    float mSpeed;
    float SPHERE_DESPAWN_TIMER = 0;
    int SPHERE_DESPAWN_TIME_LIMIT = 20;
    Vector3 mDir;
    GameObject mIntakeObj;
    bool mGround = false;
	
	// Update is called once per frame
	void Update () {
        DetermineIntake();
        ProcessIntake();
       
        
    }

    void DetermineIntake()
    {
        if (transform.localPosition.z <= 19 && transform.localPosition.y <= 1.5)
            mGround = true;
        else
            mGround = false;
    }

    void ProcessIntake()
    {
        if (mGround == true)
        {
            float mDistance = 9999;
            //Determines which intake is closest
            foreach (GameObject mIntake in GameObject.FindGameObjectsWithTag("Intake"))
            {
                Vector3 mI = mIntake.transform.position - transform.localPosition;
                float mD = mI.magnitude;
                if (mD < mDistance)
                {
                    mDistance = mD;
                    mIntakeObj = mIntake;
                }
            }
            Vector3 mDir = mIntakeObj.transform.position - transform.localPosition;
            mDir = mDir.normalized;
            mSpeed = (100 - mDistance) / 5;
            transform.localPosition += mDir * mSpeed * Time.deltaTime;
            if (mDistance < 1.5)
            {
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("Player").GetComponent<BallMachineBehavior>().AddRemoveBalls(1); //handles scaling and score
            }
            SPHERE_DESPAWN_TIMER = 0;
        }
        else
        {
            SPHERE_DESPAWN_TIMER += Time.deltaTime;
            if (SPHERE_DESPAWN_TIMER >= SPHERE_DESPAWN_TIME_LIMIT)
            {
                Destroy(gameObject);
                GameObject spherePrefab = Resources.Load("Prefabs/Prefab Objects/Sphere") as GameObject;
                GameObject sphere = Instantiate(spherePrefab) as GameObject;
                sphere.transform.localPosition = new Vector3(25, 45, -10);
            }
        }

    }
}
