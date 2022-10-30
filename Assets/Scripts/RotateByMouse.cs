using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByMouse : MonoBehaviour
{
    public float speedX = 40;
    public float speedY = 20;

    private float x = 0;
    private float y = 0;

    public float turnSpeed = 10;

    public GameManager gameManager;
    public InputManager inputManager;

	private void Start()
	{
		if (inputManager.inputCondition == InputCondition.Driving)
		{
			enabled = false;
		}
	}

	private void FixedUpdate()
    {
        if (!gameManager.gameStart) return;

        x += speedX * Input.GetAxis("Mouse X") * speedX;
        y -= speedY * Input.GetAxis("Mouse Y") * speedY;

        transform.localEulerAngles = new Vector3(y, x, 0) * Time.fixedDeltaTime;
    }
}
