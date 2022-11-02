using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByMouse : MonoBehaviour
{
    public float speedX = 10;
    public float speedY = 5;

    private float x = 0;
    private float y = 0;

    public GameManager gameManager;
	
	private void FixedUpdate()
    {
        if (!gameManager.gameStart) return;

        x += speedX * Input.GetAxis("Mouse X") * speedX;
        y -= speedY * Input.GetAxis("Mouse Y") * speedY;

        transform.localEulerAngles = new Vector3(y, x, 0) * Time.fixedDeltaTime;
    }
}
