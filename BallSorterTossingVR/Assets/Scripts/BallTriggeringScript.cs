using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallTriggeringScript : MonoBehaviour {

	public bool entered = false;
	public GameObject table;
	private Collider tabCol;
	private int handleTime = 0;
	private Rigidbody rb;
	public float xPow;
	public float yPow;
	public float zPow;
	private float[] varMass;
	public Text scoreText;
	private string scores;
	private int points;
	private int ballNbr;
	// Use this for initialization
	void Start () {
		tabCol = table.GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		points = 0;
		ballNbr = 1;
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Ball#: " + ballNbr +" Points "+ points +"/20";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Trigger")
		{
			entered = true;
			gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);

			if (other.gameObject.name == "SoftTrigger")
			{
				xPow = -1.25f;
				yPow = 3.7f;
				zPow = 8f;
			}
			if (other.gameObject.name == "MediumTrigger")
			{
				xPow = 0f;
				yPow = 5f;
				zPow = 9f;
			}
			if (other.gameObject.name == "HardTrigger")
			{
				xPow = 1.5f;
				yPow = 6f;
				zPow = 10f;
			}
		}
		if (other.gameObject.tag == "Goal")
		{
			varMass = new[] { 1.0f, 1.25f, 1.5f };
			gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
			
			Debug.Log("GOAAAAALLLL!!!!!!");
			points++;

			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			
			rb.mass = varMass[Random.Range(0,varMass.Length)];
			
			gameObject.transform.position = new Vector3(-.7f, 0.954f, 1.336f);
			ballNbr++;
			entered = false;
			tabCol.enabled = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (entered == true && other.gameObject.tag == "Trigger" )
		{
			Debug.Log("out of collider");
			gameObject.transform.position = new Vector3(other.transform.position.x, 0.3f , other.transform.position.z);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.AddForce(new Vector3(xPow, yPow, zPow),ForceMode.Impulse);
			tabCol.enabled = false;
			StartCoroutine(Wait(1));
			entered = false;
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
