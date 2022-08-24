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
	private InputAction movement;

    [SerializeField] private float movementDir = 1f;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 direction = Vector3.zero;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		playerControls = new PlayerControls();
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
		direction += movement.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementDir;
		direction += movement.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementDir;

		rb.AddForce(direction, ForceMode.Impulse);
		direction = Vector3.zero;

		Vector3 hVelocity = rb.velocity;
		hVelocity.y = 0;
		if (hVelocity.sqrMagnitude > moveSpeed * moveSpeed)
			rb.velocity = hVelocity.normalized * moveSpeed + Vector3.up * rb.velocity.y;

		LookAt();
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
		playerControls.Player.Enable();
	}

	private void OnDisable()
	{
        playerControls.Player.Disable();
	}

	
	private Vector3 GetCameraForward(Camera playerCamera)
	{
		Vector3 forward = playerCamera.transform.forward;
		forward.y = 0;
		return forward.normalized;
	}

	private Vector3 GetCameraRight(Camera playerCamera)
	{
		Vector3 right = playerCamera.transform.right;
		right.y = 0;
		return right.normalized;
	}
	#endregion


}
