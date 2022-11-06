using UnityEngine;

public class RotateByMouse : MonoBehaviour
{
    public float speedX = 10;
    public float speedY = 5;

    private float x = 0;
    private float y = 0;

    private CountDown countDown;

    private void Awake()
    {
        countDown = FindObjectOfType<CountDown>();
    }

    private void FixedUpdate()
    {
        if (!countDown.CountDownEnd) return;

        x += speedX * Input.GetAxis("Mouse X") * speedX;
        y -= speedY * Input.GetAxis("Mouse Y") * speedY;

        transform.localEulerAngles = new Vector3(y, x, 0) * Time.fixedDeltaTime;
    }
}