using UnityEngine;
using UnityEngine.UI;

public class FPSMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float lookSensitivity = 2f;
	public float jumpForce = 5f;
	public float gravity = -9.81f;

	private CharacterController characterController;
	private float xRotation = 0f;
	private Vector3 velocity;
	private Vector3 horizontalVelocity;

	public Slider SensivitySlider;
	public void SetSensivity()
	{
		var sens = SensivitySlider.value;
		lookSensitivity = sens;
	}

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		if (Cursor.lockState != CursorLockMode.Locked) return;

		float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.Rotate(Vector3.up * mouseX);
		Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;
		move = Vector3.ClampMagnitude(move, 1f);
		move *= moveSpeed;

		if (characterController.isGrounded)
		{
			horizontalVelocity = Vector3.Lerp(horizontalVelocity, move, Time.deltaTime * 10);
			velocity.y = -2f;
			if (Input.GetButtonDown("Jump"))
			{
				velocity.y = Mathf.Sqrt(jumpForce * 2f * Mathf.Abs(gravity));
			}
		}
		else
		{
			horizontalVelocity = Vector3.Lerp(horizontalVelocity, move, Time.deltaTime);
			velocity.y += gravity * Time.deltaTime;
		}

		Vector3 finalMove = horizontalVelocity + Vector3.up * velocity.y;
		characterController.Move(finalMove * Time.deltaTime);
	}
}
