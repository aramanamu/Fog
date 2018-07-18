using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneViewCamera : MonoBehaviour {
	float mouseSensitivity = 5f;
	float zoomSpeed = 3.0f;
	float flySpeed = 10.0f;
	float grabSpeed = 0.5f;
	bool targetSet = false;

	Vector3 altLookTarget = Vector3.zero;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float Xaxis = 0.0f;
		float Yaxis = 0.0f;
		
		
		if(Input.GetAxisRaw("Mouse ScrollWheel") != 0.0f)		//	Will choose the scrollwheel as first priority
		{
			transform.position += transform.forward * Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
		}
		else if(Input.GetButton("Fire2"))						//	Fire1 is set up as mouse right click. enables mouse look and wasd movement
		{
			Xaxis = Input.GetAxis("Mouse X") * mouseSensitivity;
			Yaxis = Input.GetAxis("Mouse Y") * mouseSensitivity;

			transform.eulerAngles += new Vector3(-Yaxis, Xaxis, 0.0f);	

			if(Input.GetKey(KeyCode.W))
			{
				transform.position += transform.forward * Time.deltaTime * flySpeed;
			}
			if(Input.GetKey(KeyCode.S))
			{
				transform.position -= transform.forward * Time.deltaTime * flySpeed;
			}
			if(Input.GetKey(KeyCode.A))
			{
				transform.position -= transform.right * Time.deltaTime * flySpeed;
			}
			if(Input.GetKey(KeyCode.D))
			{
				transform.position += transform.right * Time.deltaTime * flySpeed;
			}

		}
		else if(Input.GetButton("Fire3"))
		{
			Xaxis = Input.GetAxis("Mouse X");
			Yaxis = Input.GetAxis("Mouse Y");

			transform.position -= (transform.up * Yaxis + transform.right * Xaxis) * grabSpeed;
		}
		else if(Input.GetKey(KeyCode.Z) && Input.GetButton("Fire1"))
		{
			
			RaycastHit hitInfo;
			 
			if(!targetSet && Physics.Raycast(transform.position, transform.forward, out hitInfo, 50.0f))
			{
				altLookTarget = hitInfo.point;
				targetSet = true;
			}
			else if(!targetSet)
			{
				//altLookTarget = transform.position + transform.forward * 50.0f;		//	if not hit, look at the end point of the raycast.
				altLookTarget = Vector3.zero;
				targetSet = true;
			}

			float distance = Vector3.Distance(transform.position, altLookTarget);
			distance = distance / 50.0f;

			float forwardAngle = Vector3.Angle(transform.forward, Vector3.up);

			Debug.Log(forwardAngle);

			if(forwardAngle > 10f && forwardAngle < 170f)
			{
				Yaxis = Input.GetAxis("Mouse Y") * mouseSensitivity;				
			}
			else if(forwardAngle < 10.0f && Input.GetAxis("Mouse Y") < 0.0f)	//	if we've hit the low bounds, only allow input in the opposite direction
			{
				Yaxis = Input.GetAxis("Mouse Y") * mouseSensitivity;
			}
			else if(forwardAngle > 170f && Input.GetAxis("Mouse X") > 0.0f)		//	same if we've hit the upper bounds
			{
				Yaxis = Input.GetAxis("Mouse Y") * mouseSensitivity;
			}

			Xaxis = Input.GetAxis("Mouse X") * mouseSensitivity;
			//Yaxis = Input.GetAxis("Mouse Y") * mouseSensitivity;
			
			transform.position -= (transform.up * Yaxis + transform.right * Xaxis) * grabSpeed * distance;		//	this is not a rotation, so gradually the target gets further away.
			transform.LookAt(altLookTarget);

		}
		else
		{
			targetSet = false;
		}
			
		
		
	}
}
