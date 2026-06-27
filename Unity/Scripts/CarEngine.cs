using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading;
using System.Net; 
using System.Net.Sockets; 
using System.Linq; 
using System; 
using System.IO; 
using System.Text; 


// Rainy -> 3
//Sunny -> 1.2
//Snowy -> 5

public class CarEngine : MonoBehaviour {
	public Transform Path;
	private List<Transform> nodes;
	private int currentnode = 0;
	public float maxSteeringAngle = 60f;
	public float TurnSpeed = 10f;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
	public float currentSpeed;
	public float maxSpeed = 200f;
	public float MaxMotorTorque = 150f;
	public bool breaking;
	private float Nearby_Car30;
	private float Nearby_Car50;
	private bool Nearby_Cars_bool_30;
	private bool Nearby_Cars_bool_50;

	public Material Brakelights;
	// put the second material here.





	[Header("Sensors")]
	public float SensorLength1 = 10f;
	public float SensorLength2 = 10f;
	public Vector3 FrontSensorPos = new Vector3(0f, 1.5f, -2.25f);
	public float FrontsideSensorPos = 0.2f;
	public float FrontSensorAngle0 = 5f;
	public float FrontSensorAngle1 = 10f;
	public float FrontSensorAngle2 = 15f;
	public float FrontSensorAngle3 = 20f;
	public float FrontSensorAngle4 = 30f;
	public float FrontSensorAngle5 = 60f;
	public float FrontSensorAngle6 = 90f;
	private bool Avoid;
	public float TargetSteerAngle = 0;
	public float Vicinity = 10f;




	// Use this for initialization
	private void Start () {

		Transform[] Pathtransforms = Path.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();

		//loop through array
		for(int i = 0; i < Pathtransforms.Length; i++){
			if(Pathtransforms[i] != Path.transform){
				nodes.Add(Pathtransforms[i]);

			}
		}
	}




	// Update is called once per frame
	private void FixedUpdate () {
		//Sensing();
		ApplySteer();
		Drive();
		NextpointDistance();
		Break();
		SmoothSteer();
		Wheel_Friction();
	
		
	}

////////////////////////
	void Update()
    {
		RoadSign30 RoadSign30 = GameObject.Find("SpeedLimit30").GetComponent<RoadSign30>();
        Nearby_Cars_bool_30 = RoadSign30.Nearby_Cars_bool_30;
		RoadSign50 RoadSign50 = GameObject.Find("SpeedLimit50").GetComponent<RoadSign50>();
        Nearby_Cars_bool_50 = RoadSign50.Nearby_Cars_bool_50;
		//if ((Nearby_Cars_bool_30 == true) || (Nearby_Cars_bool_50 == true))
		//{
		//TakingPicture.TakeSC_Static(500, 500);
		//Debug.Break();
		//}

	}

	private void Sensing(){
		RaycastHit hit;
		Vector3 SensorStartPos = transform.position;
		SensorStartPos += transform.forward * FrontSensorPos.z;
		SensorStartPos += transform.up * FrontSensorPos.y;
		float AvoidMultiplier = 0;
		Avoid = false;




		//front right sensor
		SensorStartPos += transform.right * FrontsideSensorPos;
		if(Physics.Raycast(SensorStartPos, transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){
				Debug.DrawLine(SensorStartPos, hit.point);	
				Avoid = true;
				AvoidMultiplier -= 1f;
			}else if((hit.collider.gameObject.tag == "SpeedLimit30") || (hit.collider.gameObject.tag == "SpeedLimit50")){    // ignore if speedlimit
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);
				Avoid = false;
				AvoidMultiplier = 0;
			}
			
		}
		
	
/*
		//front right Angle sensor 5 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle0, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle0, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;

			}
		}




		//front right Angle sensor 10 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle1, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle1, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;
		}
		}

		//front right Angle sensor 15 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle2, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle2, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;
		}
		}

		//front right Angle sensor 20 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle3, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle3, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier -= 0.5f;
		}
		}
*/

		//front right Angle sensor 30 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle4, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){
				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier -= 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30") | (hit.collider.gameObject.tag == "SpeedLimit50")){	
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}

		//front right Angle sensor 60 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle5, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){
				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier -= 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30") | (hit.collider.gameObject.tag == "SpeedLimit50")){	
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}
		//front right Angle sensor 90 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(FrontSensorAngle6, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){
				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier -= 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30") || (hit.collider.gameObject.tag == "SpeedLimit50")){
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);	
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}



		//front left sensor
		SensorStartPos -= transform.right * 2 * FrontsideSensorPos;
		if(Physics.Raycast(SensorStartPos, transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){
				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier += 1f;

			}else if((!hit.collider.CompareTag("SpeedLimit30")) || (hit.collider.gameObject.tag == "SpeedLimit50")){	
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}
/*
		//front left Angle sensor 5 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle0, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle0, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){
		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;
		}
		}

		//front left Angle sensor 10 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle1, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle1, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}

		//front left Angle sensor 15 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle2, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle2, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}

		//front left Angle sensor 20 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle3, transform.up)*transform.forward, out hit, SensorLength1)){
			if(!hit.collider.CompareTag("Terrain")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;

			}
		}else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle3, transform.up)*transform.forward, out hit, SensorLength2)){
			if(!hit.collider.CompareTag("Cars")){

		Debug.DrawLine(SensorStartPos, hit.point);
		Avoid = true;
		AvoidMultiplier += 0.5f;
			}
		}
*/
		//front left Angle sensor 30 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle4, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){

				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier += 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30" || hit.collider.gameObject.tag == "SpeedLimit50")){
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);	
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}

		//front left Angle sensor 60 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle5, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){

				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier += 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30" || hit.collider.gameObject.tag == "SpeedLimit50")){
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);	
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}
		
		//front left Angle sensor 90 deg
		else if(Physics.Raycast(SensorStartPos, Quaternion.AngleAxis(-FrontSensorAngle6, transform.up)*transform.forward, out hit, SensorLength1)){
			if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){

				Debug.DrawLine(SensorStartPos, hit.point);
				Avoid = true;
				AvoidMultiplier += 0.5f;

			}else if((hit.collider.gameObject.tag == "SpeedLimit30" || hit.collider.gameObject.tag == "SpeedLimit50")){
				Debug.DrawLine(SensorStartPos, hit.point, Color.green);	
				Avoid = false;
				AvoidMultiplier = 0;
			}
		}

		//front center sensor
		if(AvoidMultiplier == 0){
			if(Physics.Raycast(SensorStartPos, transform.forward, out hit, SensorLength1)){
				if((hit.collider.gameObject.tag == "Terrain") || (hit.collider.gameObject.tag == "Cars")){

					Debug.DrawLine(SensorStartPos, hit.point);
					Avoid = true;
					if(hit.normal.x < 7f){
						AvoidMultiplier = -1;
					}else{
						AvoidMultiplier = 1;
					}
		
				}else if((hit.collider.gameObject.tag == "SpeedLimit30") || (hit.collider.gameObject.tag == "SpeedLimit50")){
					Debug.DrawLine(SensorStartPos, hit.point, Color.green);	
					Avoid = false;
					AvoidMultiplier = 0;
			}
			}

		}

		if (Avoid){
			TargetSteerAngle = maxSteeringAngle * AvoidMultiplier;
			//wheelFL.steerAngle = maxSteeringAngle * AvoidMultiplier;
			//wheelFR.steerAngle = maxSteeringAngle * AvoidMultiplier;
			if (((TargetSteerAngle == 22.5f) || (TargetSteerAngle == -22.5f) || (TargetSteerAngle == 45f) || (TargetSteerAngle == -45f))  && (!hit.collider.CompareTag("Cars"))){
				Vector3 distanceVec = transform.InverseTransformPoint(nodes[currentnode].position);
				float newSteer = (distanceVec.x / distanceVec.magnitude) * maxSteeringAngle;
				TargetSteerAngle = newSteer;
				breaking = false;

			}else {
				breaking = false;

			}


		}

			

	
	}


	private void ApplySteer(){
		if (Avoid) return;
		Vector3 distanceVec = transform.InverseTransformPoint(nodes[currentnode].position);
		float newSteer = (distanceVec.x / distanceVec.magnitude) * maxSteeringAngle;
		TargetSteerAngle = newSteer;
		//wheelFL.steerAngle = newSteer;
		//wheelFR.steerAngle = newSteer;


	}

	private void Drive(){
		//currentSpeed = 2 * Mathf.PI * wheelRL.radius * wheelRL.rpm * 60 / 100;
		currentSpeed = MainMenuScript.SliderVal;

		if (currentSpeed < maxSpeed && !breaking){
					wheelRL.motorTorque = MaxMotorTorque;
					wheelRR.motorTorque = MaxMotorTorque;
		}else{
					wheelRL.motorTorque = 0;
					wheelRR.motorTorque = 0;
		}


	}
	private void NextpointDistance(){
		if(Vector3.Distance(transform.position, nodes[currentnode].position) < 5f){
			if(currentnode == nodes.Count - 1){
				currentnode = 0;
			}
			else {
				currentnode++;
			}
		}

	}
	private void Break(){
	if(breaking) {
		wheelRL.brakeTorque = 10000f;
		wheelRR.brakeTorque = 10000f;


		//GameObject.GetComponentsInChildren<MeshRenderer>().materials[4].color = Color.red;
		
	}
	else if (!breaking){
		wheelRL.brakeTorque = 0;
		wheelRR.brakeTorque = 0;
		//GameObject.GetComponentsInChildren<MeshRenderer>().materials[4].color = Color.black;
		}


	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2)
			breaking = true;
	}

	private void SmoothSteer(){
		wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, TargetSteerAngle, TurnSpeed * Time.deltaTime);
		wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, TargetSteerAngle, TurnSpeed * Time.deltaTime);
	}

//https://docs.unity3d.com/ScriptReference/WheelFrictionCurve.html
	private void Wheel_Friction(){
	 WheelFrictionCurve Forward_frictionFL = wheelFL.sidewaysFriction;
	 WheelFrictionCurve Forward_frictionFR = wheelFR.sidewaysFriction;
	 WheelFrictionCurve Forward_frictionRL = wheelRL.sidewaysFriction;
	 WheelFrictionCurve Forward_frictionRR = wheelRR.sidewaysFriction;

 	 Forward_frictionFL.extremumSlip = 1.2f;
	 Forward_frictionFR.extremumSlip = 1.2f;
	 Forward_frictionRL.extremumSlip = 1.2f;
	 Forward_frictionRR.extremumSlip = 1.2f;

 	 wheelFL.sidewaysFriction = Forward_frictionFL;
	 wheelFR.sidewaysFriction = Forward_frictionFR;
	 wheelRL.sidewaysFriction = Forward_frictionRL;
	 wheelRR.sidewaysFriction = Forward_frictionRR;

	}

	


}

