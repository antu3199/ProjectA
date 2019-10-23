using UnityEngine;
using System.Collections.Generic;

public class PlayerExample : MonoBehaviour {


    public float moveSpeed;
    public Joystick joystick;

	void Update () 
	{
        Joystick.DPadDirection curDPadDirection = joystick.GetDPadDirection();
        Debug.Log("Direction: " + curDPadDirection.ToString());
        
        Vector3 moveVector = (transform.right * joystick.Horizontal + transform.forward * joystick.Vertical).normalized;
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);
	}

}