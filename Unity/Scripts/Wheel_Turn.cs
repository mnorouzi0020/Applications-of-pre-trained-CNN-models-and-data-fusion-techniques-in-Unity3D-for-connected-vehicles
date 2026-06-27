using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_Turn : MonoBehaviour {



	public WheelCollider wheelTurn;
	// Use this for initialization
	private Vector3 wheelposition = new Vector3();
	private Quaternion wheelrotation = new Quaternion();



	
	// Update is called once per frame
	private void Update () {

		wheelTurn.GetWorldPose(out wheelposition, out wheelrotation);
		transform.position = wheelposition;
		transform.rotation = wheelrotation * Quaternion.Euler(0,90,0);

	}
}
