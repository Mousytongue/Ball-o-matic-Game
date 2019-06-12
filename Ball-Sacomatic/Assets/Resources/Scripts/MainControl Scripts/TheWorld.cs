using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour {

    public SceneNode mBallMachine;
	
	void Update ()
    {
        Matrix4x4 i = Matrix4x4.identity;
        mBallMachine.CompositeXform(ref i);
    }
}
