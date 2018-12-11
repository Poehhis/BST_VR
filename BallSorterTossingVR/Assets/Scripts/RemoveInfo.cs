using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    BallTriggeringScript btsTrigger;
    private bool delete;
    GameObject ball;
	// Use this for initialization
	void Start () {
        btsTrigger = ball.GetComponent<BallTriggeringScript>();
        delete = btsTrigger.entered;
	}
	
	// Update is called once per frame
	void Update () {
        if (delete) Destroy(this);
	}
}
