using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private float m_RotationSpeed;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        //float AxisHor = Input.GetAxis("Cam_Horizontal");
        //float axisVer = Input.GetAxis("Cam_Vertical");
        float AxisHor = Input.GetAxis("Mouse X");
        float axisVer = Input.GetAxis("Mouse Y") * -1;

        if (transform.localEulerAngles.x > 300f && transform.localEulerAngles.x < 325f && axisVer < 0f)
        {
            axisVer = 0;
        }
        else if (transform.localEulerAngles.x > 45f && transform.localEulerAngles.x < 60f && axisVer > 0)
        {
            axisVer = 0;
        }


        transform.Rotate(new Vector3(0f , AxisHor * Time.deltaTime * m_RotationSpeed, 0f), Space.World);
        transform.Rotate(new Vector3(axisVer * Time.deltaTime * m_RotationSpeed, 0f, 0f), Space.Self);

	}
}
