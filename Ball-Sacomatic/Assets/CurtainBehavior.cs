using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainBehavior : MonoBehaviour {

    public Transform mLeftCurtain = null;
    public Transform mRightCurtain = null;
    float REVEAL_SPEED = 5;
    float REVEAL_DELAY = 5;
    float DESTRUCT_TIMER = 30;
    float mTimer;
	
	// Update is called once per frame
	void Update () {
        mTimer += Time.deltaTime;

        if(mTimer >= REVEAL_DELAY)
        {
            mLeftCurtain.position += new Vector3(-REVEAL_SPEED * Time.deltaTime, 0, 0);
            mRightCurtain.position += new Vector3(REVEAL_SPEED * Time.deltaTime, 0, 0);
        }

        if (mTimer >= DESTRUCT_TIMER)
        {
            Destroy(gameObject);
        }
	}
}
