using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTriggeringScript : MonoBehaviour {

	public bool entered = false;
	public GameObject table;
	private Collider tabCol;
	private int handleTime = 0;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		tabCol = table.GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Trigger")
		{
			entered = true;
			gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
			/*if (handleTime%2 == 0)
			{ 
			entered = true;
			gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
			handleTime += 1;
			}
			if (handleTime % 2 == 1)
			{
				entered = true;
				gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
				handleTime += 1;
			}*/
			Debug.Log("entered");
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (entered == true && other.gameObject.tag == "Trigger" )
		{
			Debug.Log("out of collider");
			gameObject.transform.position = new Vector3(other.transform.position.x, 0.3f , other.transform.position.z);
			rb.velocity = Vector3.zero;
			rb.AddForce(new Vector3(0f, 3.5f,6f),ForceMode.Impulse);
			tabCol.enabled = false;
			StartCoroutine(Wait(1));
		}
	}
	IEnumerator Wait(int time)
	{
		//print(Time.time);
		yield return new WaitForSeconds(time);
		tabCol.enabled = true;
		//print(Time.time);
	}
}
