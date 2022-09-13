using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
	[SerializeField] private Animator animator;
	[SerializeField] private Camera playerCamera;

    private PlayerControls playerControls;
	private InputAction jump;
	private InputAction movement;

	[SerializeField] private float jumpPower = 2f;
	[SerializeField] private float movementDir = 1f;
    [SerializeField] private float moveSpeed = 5f;
	private Vector3 direction = Vector3.zero;

	private bool isGrounded;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		playerControls = new PlayerControls();
		isGrounded = true;
	}

	private void Update()
	{
		if (rb.velocity.x > 0.01f || rb.velocity.z > 0.01f)
		{
			animator.SetBool("isBored", false);
			animator.SetFloat("Speed", rb.velocity.magnitude / moveSpeed);
		}
		else
		{
			animator.SetBool("isBored", true);
		}

	}

	private void FixedUpdate()
	{
		direction += movement.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementDir; // X - Axis
		direction += movement.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementDir; // Z - Axis

		if (isGrounded)
		{ 
			rb.AddForce(direction, ForceMode.Impulse);
			direction = Vector3.zero;

			Vector3 horizontalVelocity = rb.velocity;
			horizontalVelocity.y = 0;
			if (horizontalVelocity.sqrMagnitude > moveSpeed * moveSpeed)
				rb.velocity = horizontalVelocity.normalized * moveSpeed + Vector3.up * rb.velocity.y;

			LookAt();
		}
	}

	private void LookAt()
	{
		Vector3 dir = rb.velocity;
		dir.y = 0;

		if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && dir.sqrMagnitude > 0.1f)
		{
			rb.rotation = Quaternion.LookRotation(dir, Vector3.up);
		} else
		{
			rb.angularVelocity = Vector3.zero;
		}
	}

	#region
	private void OnEnable()
	{
		movement = playerControls.Player.Move;
		jump = playerControls.Player.Jump;
		playerControls.Player.Enable();
	}

	private void OnDisable()
	{
        playerControls.Player.Disable();
	}

	// Z - AXIS
	private Vector3 GetCameraForward(Camera playerCamera)
	{
		Vector3 forward = playerCamera.transform.forward;
		forward.y = 0;
		return forward.normalized;
	}

	// X - AXIS
	private Vector3 GetCameraRight(Camera playerCamera)
	{
		Vector3 right = playerCamera.transform.right;
		right.y = 0;
		return right.normalized;
	}
	#endregion


}
