using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallTriggeringScript : MonoBehaviour {

    Scene scene;
    ArduinoBridge ab;
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
    public Text infoText;
    private string scores;
	private int points;
	private int ballNbr;
    public float ballMass;
    public bool sceneChange;
    // Use this for initialization
    void Start () {
		tabCol = table.GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
        ab = GetComponent<ArduinoBridge>();
		points = 0;
		ballNbr = 1;
        SwapBall();
        sceneChange = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        scene = SceneManager.GetActiveScene();
        //Scene change and layouts for vibrotactile testing
        if (scene.name == "TrialScene")
        {
            scoreText.text = "Ball#: " + ballNbr + " Points " + points + "/9";
            //Debug.Log(points);
            if (points == 9) SceneManager.LoadScene("MainScene");
        }
        else
        {
            scoreText.text = "Ball#: " + ballNbr + " Points " + points + "/20";   
        }

        //Scene change and layouts for pneumotactile testing
        if (scene.name == "TrialScenePneumo")
        {
            scoreText.text = "Ball#: " + ballNbr + " Points " + points + "/9";
            //Debug.Log(points);
            if (points == 9)
            {
                sceneChange = true;
                SceneManager.LoadScene("MainScenePneumo");
            }
        }
        else
        {
            scoreText.text = "Ball#: " + ballNbr + " Points " + points + "/20";

        }

        //return ball with no points 
        if (gameObject.transform.position.z >= 7f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ballNbr++;
            //Check if it's a last ball
            if (ballNbr >= 21) gameObject.transform.position = new Vector3(-.7f, 0.954f, -1.336f);
            else
            {
                gameObject.transform.position = new Vector3(-.7f, 0.954f, 1.336f);
            }
            
            SwapBall();
            
            entered = false;
            tabCol.enabled = true;
        }
     }


private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Trigger")
		{
			entered = true;
            Destroy(infoText);
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
			
			//Debug.Log("GOAAAAALLLL!!!!!!");
			points++;

			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

            ballNbr++;

            //When one have thrown 20 balls
            if (ballNbr >= 21) gameObject.transform.position = new Vector3(-.7f, 0.954f, -1.336f);
            else
            {
                gameObject.transform.position = new Vector3(-.7f, 0.954f, 1.336f);
            }
            
            
            SwapBall();
            
            entered = false;
			tabCol.enabled = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (entered == true && other.gameObject.tag == "Trigger" )
		{
			//Debug.Log("out of collider");
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
    //Change mass of the object randomly
    void SwapBall()
    {

        varMass = new[] { 1.0f, 1.25f, 1.5f };
       
        
        if (scene.name == "TrialScene" || scene.name == "TrialScenePneumo")
        {
            //Debug.Log(ballNbr);
            if (ballNbr >= 1 && ballNbr <= 3) rb.mass = varMass[0];
            if (ballNbr >= 4 && ballNbr <= 6) rb.mass = varMass[1];
            if (ballNbr >= 7 && ballNbr <= 9) rb.mass = varMass[2];
            ballMass = rb.mass;
            //Debug.Log("bts ball mass: " + ballMass);
            ab.msg = true;
        }
        else
        {
            rb.mass = varMass[Random.Range(0, varMass.Length)];
            ballMass = rb.mass;
            ab.msg = true;
        }
    }
}
