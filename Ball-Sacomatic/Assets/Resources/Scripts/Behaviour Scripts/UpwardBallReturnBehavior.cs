using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardBallReturnBehavior : MonoBehaviour {

    bool flipped = false;
    float MOVEMENT_SPEED = 3;

	void Update () {
        ProcessMovement();       
	}

    void ProcessMovement()
    {
        transform.localPosition += new Vector3(0, MOVEMENT_SPEED * Time.deltaTime, 0);

        if (transform.localPosition.y >= 13.5)
        {
            if (flipped == false)
            {
                transform.eulerAngles = new Vector3(-45, 0, 0);
                Vector3 newPos = transform.localPosition;
                newPos.z = 51;
                newPos.y += 1;
                transform.localPosition = newPos;
                flipped = true;
            }
        }
        if (transform.localPosition.y >= 55)
            Destroy(gameObject);
    }
}
