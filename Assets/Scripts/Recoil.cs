using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
   // Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	public bool doit = true;
	public int recoil_type;
	// How long the object should shake for.
	private float shakeAmount;
	private Boolean returnToCam=false;
	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeDuration;
	private float decreaseFactor;
	
	Vector3 originalPos;
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
		switch (recoil_type)
		{
			//pistol
			case 0:
				shakeAmount = 0.05f;
				shakeDuration = 0.1f;
				decreaseFactor = 0.1f;
				returnToCam = true;
				break;
			//rifle
			case 1:
				shakeAmount = 0.6f;
				shakeDuration = 0.1f;
				decreaseFactor = 0.001f;
				break;
			//shotgun
			case 2:
				shakeAmount =1f;
				shakeDuration =0.1f;
				decreaseFactor = 0.05f;
				returnToCam = true;
				break;
		}
	}

	void Update()
	{
		
		if (shakeDuration > 0f)
		{
			camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime;
		}
		else
		{
			shakeDuration = 0f;
			if(returnToCam = true){
				camTransform.localPosition = originalPos;
			}
		}
		
	}
}
