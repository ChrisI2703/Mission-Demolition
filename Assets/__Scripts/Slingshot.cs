using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	[Header("Set in Inspector")]
	public GameObject prefabProjectile;
	public float velocityMult = 8f;
	
	[Header("Set Dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	
	private Rigidbody projectileRigidBody;
	
	void Awake(){
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}
	
	void OnMouseEnter(){
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
	}
	
	void OnMouseExit(){
		//print("Slingshot:OnMouseExit()");
		launchPoint.SetActive(false);
	}
	
	void OnMouseDown(){
		aimingMode = true;
		// create a projectile
		projectile = Instantiate(prefabProjectile) as GameObject;
		// set it to launchPoint
		projectile.transform.position = launchPos;
		// set it to be kinematic
		projectile.GetComponent<Rigidbody>().isKinematic = true;
		
		projectileRigidBody = projectile.GetComponent<Rigidbody>();
		projectileRigidBody.isKinematic = true;
	}
		
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// if slingshot is not in aiming mode, don't do this
		if (!aimingMode) return;
		
		// Get the current position of the mouse
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		
		// calculating distance between mouse and launch position
		Vector3 mouseDelta = mousePos3D - launchPos;
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude){
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}
		
		// Move the projectile to this new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		
		if (Input.GetMouseButtonUp(0)){
		    aimingMode = false;
			projectileRigidBody.isKinematic = false;
			projectileRigidBody.velocity = -mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;			
		}
        
    }
}
