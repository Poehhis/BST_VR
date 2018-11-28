using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour {

    private Rigidbody rb;
    private Renderer rend;
    private BallTriggeringScript btss;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        btss = GetComponent<BallTriggeringScript>();
    }
	
	// Update is called once per frame
	void Update () {
        SetColor();
	}

    //Change color of the object based on its mass
    void SetColor()
    {
        //Green for soft ball
        if (rb.mass >= 0.9 && rb.mass <= 1.1)
        {
            //Set the main Color of the Material to green
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }
        //Yellow for medium ball
        if (rb.mass >= 1.2 && rb.mass <= 1.3)
        {
            //Set the main Color of the Material to green
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.yellow);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }
        //Red for hard ball
        if (rb.mass >= 1.4 && rb.mass <= 1.6)
        {
            //Set the main Color of the Material to green
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.red);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }
    }
}
