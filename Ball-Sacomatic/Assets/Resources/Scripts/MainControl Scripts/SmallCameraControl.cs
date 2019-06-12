using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCameraControl : MonoBehaviour {

    public Transform CameraHead = null;
    public Transform LookAtPosition = null;

	void Update () {
        if (CameraHead != null || LookAtPosition != null)
        {
            transform.localPosition = CameraHead.position;
            LookAtPosition.localPosition = CameraHead.position + CameraHead.up * 100;
            transform.LookAt(LookAtPosition);
        }
    }
}
