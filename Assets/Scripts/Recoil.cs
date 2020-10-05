using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
   // Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	public int recoil_type;
	public bool isShooting=false;
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
	void addRecoil() {
		switch (recoil_type)
		{
			//rifle
			case 1:
				if (shakeAmount < 0.6f)
				{
					shakeAmount += 0.01f;
					if (shakeAmount > 0.55) {
						returnToCam = false;
					}
				}
				break;
			//shotgun
			case 2:
				if (shakeAmount < 0.6f)
				{
					shakeAmount += 0.05f;
				}
				break;
		}
	}
	void SetVars(bool setAll)
    {
		switch (recoil_type)
		{
			//pistol
			case 0:
				if (setAll) { shakeAmount = 0.05f; }
				shakeDuration = 0.1f;
				decreaseFactor = 0.1f;
				returnToCam = true;
				break;
			//rifle
			case 1:
				if (setAll) { shakeAmount = 0.5f; }
				shakeDuration = 0.1f;
				decreaseFactor = 0.5f;
				returnToCam = true;
				break;
			//shotgun
			case 2:
				if (setAll) { shakeAmount = 0.1f; }
				shakeDuration = 0.2f;
				decreaseFactor = 0.1f;
				returnToCam = true;
				break;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
		 SetVars(true);

	}
	void StartRecoil() {
		isShooting = true;
	}
	void StopRecoil() {
		isShooting = false;
	}

	void Update()
	{
		if (isShooting) { 
			if (shakeDuration > 0f)
			{
				camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
				addRecoil();
				SetVars(false);
				shakeDuration -= Time.deltaTime* decreaseFactor;
				
			}
			else
			{
				addRecoil();
				SetVars(true);
				isShooting = false;
				if (returnToCam ==	 true) {
					camTransform.localPosition = originalPos;
				}
			}
		}
		
	}
}

