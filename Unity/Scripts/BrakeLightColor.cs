using UnityEngine;
using System.Collections;

public class BrakeLightColor : MonoBehaviour {

    // put the first material here.
    public Material Brakelights;
 // put the second material here.
    public Material ReverseLights;
    bool FirstMaterial = true;
    bool SecondndMaterial = false;
    GameObject Car;
    void Start () 
    {
     GetComponent<Renderer>().material = Brakelights;
    }
 
 void Update()
 {
     Car = GameObject.FindWithTag("Cars");
     CarEngine Car_Engine = Car.GetComponent<CarEngine>();
     //Material[] materials = Car.GetComponent<Renderer>().materials;

        if (Car_Engine.breaking == true)
        {
         //GetComponent<Renderer>().materials[0] = ReverseLights;
         GetComponent<Renderer>().materials[4].color = Color.red;
         //materials[4] = ReverseLights;
         SecondndMaterial = true;
         FirstMaterial = false;
        }
        else if (Car_Engine.breaking == false)
        {
         //materials[4] = Brakelights;
         //GetComponent<Renderer>().materials[0] = Brakelights;
         GetComponent<Renderer>().materials[4].color = Color.black;
         FirstMaterial = false;
         SecondndMaterial = true;
        }
    }

}

