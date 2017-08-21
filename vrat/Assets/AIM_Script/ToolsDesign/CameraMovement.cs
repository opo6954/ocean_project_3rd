using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Transform myCamera;

    

	// Use this for initialization
	void Start () {
        myCamera = gameObject.GetComponent<Camera>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion cameraRotation;
        cameraRotation = myCamera.rotation;

        Vector3 p = cameraRotation.eulerAngles;

        p.y = p.y + 0.5f;

        myCamera.rotation = Quaternion.Euler(p);
	}

    public void OnClickAuthoringMode()
    {
        Application.LoadLevel(1);
    }


}
 