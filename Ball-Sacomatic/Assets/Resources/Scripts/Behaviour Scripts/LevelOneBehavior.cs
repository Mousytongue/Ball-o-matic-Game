﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneBehavior : MonoBehaviour {

    float TARGET_MOVE_SPEED = 5;
    Vector3 curPos;
	
	// Update is called once per frame
	void Update () {
        ProcessMovement();
	}

    void ProcessMovement()
    {
        transform.localPosition += new Vector3(-TARGET_MOVE_SPEED * Time.deltaTime, 0, 0);
        curPos = transform.localPosition;
        if (curPos.x <= -55)
        {
            Destroy(gameObject);
        }
    }
}