﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ViveControllersGrab : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	// 1
	private GameObject collidingObject;
	// 2
	private GameObject objectInHand;
	public GameObject ball;
	BallTriggeringScript bts;
	bool triggered;
	private Rigidbody rb;
    public ushort pulse;
    ArduinoBridge ab;
    
	// Use this for initialization
	void Start () {
		bts = ball.GetComponent<BallTriggeringScript>();
		rb = ball.GetComponent<Rigidbody>();
        ab = ball.GetComponent<ArduinoBridge>();  
	}
	
	// Update is called once per frame
	void Update () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		//Debug.Log(bts.entered);
		// 1
		if (Controller.GetAxis() != Vector2.zero)
		{
			//Debug.Log(gameObject.name + Controller.GetAxis());
		}

		// 2
		if (Controller.GetHairTriggerDown())
		{
			//Debug.Log(gameObject.name + " Trigger Press");
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			ball.transform.position = new Vector3(-.7f, 0.954f, 1.336f);

            //ab.msg = true;
		}

		// 3
		if (Controller.GetHairTriggerUp())
		{
			//Debug.Log(gameObject.name + " Trigger Release");

		}

		// 4
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
			//Debug.Log(gameObject.name + " Grip Press");

			if (collidingObject)
			{
				GrabObject();
				StartCoroutine(Wait());
			}

		}

		if (bts.entered == true)
		{
			ReleaseObject();
		}

		// 5
		/*if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Release");
		}*/

		//Controll vibration
		
	}

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col)
	{
		// 1
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		// 2
		collidingObject = col.gameObject;
	}
	
	// 1
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
	}

	// 2
	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
	}

	// 3
	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}

		collidingObject = null;
	}

	private void GrabObject()
	{
		// 1
		objectInHand = collidingObject;
		collidingObject = null;
		// 2
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// 3
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject()
	{
		// 1
		if (GetComponent<FixedJoint>())
		{
			// 2
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// 3
			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}
		// 4
		objectInHand = null;
	}

	IEnumerator Wait()
	{
        pulse = 0;
        if ((double)rb.mass == 1.0) pulse = 100;
        if ((double)rb.mass == 1.25) pulse = 500;
        if ((double)rb.mass == 1.5) pulse = 1000;

        while (true)
		{ 
			SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(pulse);
			yield return new WaitForSeconds(0.05f);
			if (bts.entered == true) break;
		}
		//print(Time.time);
	}
}
