using UnityEngine;
using System;

namespace FG
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
	public class CharacterMovement : MonoBehaviour
	{
		[NonSerialized] public float sidewaysInput;
		[NonSerialized] public float forwardInput;
		[NonSerialized] public float turnInput;
		[NonSerialized] public bool crouchInput;
		[NonSerialized] public bool jumpInput;
		[NonSerialized] public bool runInput;

		[SerializeField] private CharacterData _characterData;
		[SerializeField] Transform playerInputSpace;
		[SerializeField] private Transform playerModel;
		[SerializeField] private float maxDistance;
		
		public bool IsCrouching { get; private set; }

		private Vector2 originalCapsuleSize;
		private Vector3 moveDirection;
		private float currentSpeed;
		private float adjustVerticalVelocity;
		private float inputAmount;

		private Transform _transform;
		private Rigidbody body;
		private CapsuleCollider capsuleCollider;
		private bool isSliding;
		private float lastSlideTime;
		private bool isGrounded;

		private bool isWallRunning;
		private bool wallOnLeft, wallOnRight;

		private float slideSpeedModifier = 1;


		private void Awake()
		{
			_transform = transform;
			body = GetComponent<Rigidbody>();
			capsuleCollider = GetComponent<CapsuleCollider>();
			originalCapsuleSize.Set(capsuleCollider.radius, capsuleCollider.height);
		}

		private void LateUpdate()
		{
			isGrounded = IsGrounded();
			CheckWallRunning();
			WallRun();
			
			if (!isWallRunning)
			{
				Move();
				Crouch();
				Slide();
			}
			
			Jump();
			
			if(!isSliding)
			{
				SetVelocity();
			}
			RotatePlayer();
		}

		private void RotatePlayer()
		{
			Vector3 dir = playerInputSpace.TransformDirection(transform.forward);
			playerModel.LookAt(playerModel.position + dir);
			playerModel.rotation = Quaternion.Euler(0, playerModel.transform.eulerAngles.y, 0);
		}
		
		private void Move()
		{
			body.MoveRotation(body.rotation * Quaternion.Euler(Vector3.up * turnInput));
			moveDirection = (sidewaysInput * _transform.right + forwardInput * _transform.forward).normalized;
			inputAmount = Mathf.Clamp01(Mathf.Abs(forwardInput) + Mathf.Abs(sidewaysInput));

			adjustVerticalVelocity = body.velocity.y;

			if (isGrounded)
			{
					if (IsCrouching)
					{
						currentSpeed = _characterData.crouchSpeed;
					}
					else
					{
						currentSpeed = runInput ? _characterData.runSpeed : _characterData.walkingSpeed;
					}
			}
			else
			{
				currentSpeed *= _characterData.inAirMovementMultiplier;
			}
		}

		private void Jump()
		{
			if (isWallRunning)
			{
				
			}
			else if (isGrounded)
			{
				if (jumpInput)
				{
					adjustVerticalVelocity = _characterData.jumpForce;
					SetVelocity();
				}
			}
		}

		private void Crouch()
		{
			
			if (crouchInput && !IsCrouching)
			{
				EnterCrouch();
			}
			if(!crouchInput && IsCrouching)
			{
				ExitCrouch();
			}
		}

		private void Slide()
		{
			if (isSliding && isGrounded)
			{
				body.drag = _characterData.slideDrag;
			}
			else
			{
				body.drag = 0f;
			}

			if (body.velocity.magnitude < _characterData.slideVelocityThreshold && isSliding)
			{
				isSliding = false;
				lastSlideTime = Time.time;
			}
			if (isSliding)
			{
				AddCharacterForce();
			}
		}

		private void CheckWallRunning()
		{
			Vector3 dir = playerInputSpace.TransformDirection(transform.right);
			Debug.DrawRay(_transform.position, dir, Color.red, 0.1f);
			Debug.DrawRay(_transform.position, -dir, Color.blue, 0.1f);
			wallOnLeft = Physics.Raycast(_transform.position, -dir, _characterData.maxDistanceFromWall);
			wallOnRight = Physics.Raycast(_transform.position, dir, _characterData.maxDistanceFromWall);

			bool wallRunning = false;
			if (wallOnRight)
			{
				if (sidewaysInput > float.Epsilon && forwardInput > float.Epsilon)
				{
					wallRunning = true;
				}
			}
			if (wallOnLeft)
			{
				if (sidewaysInput < -float.Epsilon && forwardInput > float.Epsilon)
				{
					wallRunning = true;
				}
			}

			isWallRunning = wallRunning;
		}
		
		private void WallRun()
		{
			if (isWallRunning)
			{
				body.useGravity = false;
				if (wallOnLeft)
				{
					
				}
			}
			else
			{
				body.useGravity = true;
			}
		}

		
		private void AddCharacterForce()
		{
			//Debug.Log((slideSpeedModifier * Mathf.Clamp01((Time.time - lastSlideTime) / _characterData.slideCooldown)));
			Vector3 dir = playerInputSpace.TransformDirection(moveDirection);
			dir = new Vector3(dir.x, 0, dir.z);
			dir.Normalize();
			Vector3 velocity = body.velocity.magnitude * slideSpeedModifier * (dir * (_characterData.slideTurnMultiplier * 0.01f) + body.velocity).normalized;
			velocity.y = adjustVerticalVelocity + -_characterData.gravityMultiplier;
			body.velocity = velocity;
			slideSpeedModifier = 1f;

		}

		private void EnterCrouch()
		{
			IsCrouching = true;
			if (body.velocity.magnitude > _characterData.slideVelocityThreshold)
			{
				isSliding = true;
				slideSpeedModifier = Mathf.Clamp01((Time.time - lastSlideTime) / _characterData.slideCooldown) * _characterData.slideBoost;

				
			}
			capsuleCollider.height = _characterData.crouchHeight;
			capsuleCollider.radius = _characterData.crouchRadius;
		}

		private void ExitCrouch()
		{
			isSliding = false;
			lastSlideTime = Time.time;
			IsCrouching = false;
			capsuleCollider.radius = originalCapsuleSize.x;
			capsuleCollider.height = originalCapsuleSize.y;
		} 

		private void SetVelocity()
		{
			Vector3 dir = playerInputSpace.TransformDirection(moveDirection);
			dir = new Vector3(dir.x, 0, dir.z);
			dir.Normalize();
			Vector3 velocity = (dir * (currentSpeed * inputAmount));
			velocity.y = adjustVerticalVelocity + -_characterData.gravityMultiplier;
			body.velocity = velocity;
		}

		private bool IsGrounded()
		{
			//Todo Make Better
			
			Debug.DrawRay(_transform.position + capsuleCollider.center, Vector3.down * maxDistance, Color.red);

			//return Physics.Raycast(_transform.position + _capsuleColider.center, Vector3.down, maxDistance);
			Ray ray = new Ray(_transform.position, Vector3.down);
			return Physics.SphereCast(ray, 0.4f, maxDistance);
		}
	}
}
