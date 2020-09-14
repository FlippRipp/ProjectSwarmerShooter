using UnityEngine;
using System;
using System.Xml;

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


		private Vector2 _originalCapsuleSize;
		private Vector3 _moveDirection;
		private float _CurrentSpeed;
		private float _adjustVerticalVelocity;
		private float _inputAmount;

		private Transform _transform;
		private Rigidbody _body;
		private CapsuleCollider _capsuleColider;
		private bool isSliding;
		private float lastSlideTime;
		private bool isGrounded;

		private bool isWallRunning;
		private bool wallOnLeft, wallOnRight;

		

		private float slideSpeedModifier = 1;
		
		[SerializeField] private float maxDistance;

		public bool isCrouching { get; private set; }

		private void Awake()
		{
			_transform = transform;
			_body = GetComponent<Rigidbody>();
			_capsuleColider = GetComponent<CapsuleCollider>();
			_originalCapsuleSize.Set(_capsuleColider.radius, _capsuleColider.height);
			
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
			_body.MoveRotation(_body.rotation * Quaternion.Euler(Vector3.up * turnInput));
			_moveDirection = (sidewaysInput * _transform.right + forwardInput * _transform.forward).normalized;
			_inputAmount = Mathf.Clamp01(Mathf.Abs(forwardInput) + Mathf.Abs(sidewaysInput));

			_adjustVerticalVelocity = _body.velocity.y;

			if (isGrounded)
			{
					if (isCrouching)
					{
						_CurrentSpeed = _characterData.crouchSpeed;
					}
					else
					{
						_CurrentSpeed = runInput ? _characterData.runSpeed : _characterData.walkingSpeed;
					}
			}
			else
			{
				_CurrentSpeed *= _characterData.inAirMovementMultiplier;
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
					_adjustVerticalVelocity = _characterData.jumpForce;
					SetVelocity();
				}
			}
		}

		private void Crouch()
		{
			
			if (crouchInput)
			{
				if (isCrouching)
				{
					ExitCrouch();
				}
				else
				{
					EnterCrouch();
				}
			}
		}

		private void Slide()
		{
			if (isSliding && isGrounded)
			{
				_body.drag = _characterData.slideDrag;
			}
			else
			{
				_body.drag = 0f;
			}

			if (_body.velocity.magnitude < _characterData.slideVelocityThreshold && isSliding)
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
				_body.useGravity = false;
				if (wallOnLeft)
				{
					
				}
			}
			else
			{
				_body.useGravity = true;
			}
		}

		
		private void AddCharacterForce()
		{
			//Debug.Log((slideSpeedModifier * Mathf.Clamp01((Time.time - lastSlideTime) / _characterData.slideCooldown)));
			Vector3 dir = playerInputSpace.TransformDirection(_moveDirection);
			dir = new Vector3(dir.x, 0, dir.z);
			dir.Normalize();
			Vector3 velocity = _body.velocity.magnitude * slideSpeedModifier * (dir * (_characterData.slideTurnMultiplier * 0.01f) + _body.velocity).normalized;
			velocity.y = _adjustVerticalVelocity * _characterData.gravityMultiplier;
			_body.velocity = velocity;
			slideSpeedModifier = 1f;

		}

		private void EnterCrouch()
		{
			isCrouching = true;
			if (_body.velocity.magnitude > _characterData.slideVelocityThreshold)
			{
				isSliding = true;
				slideSpeedModifier = Mathf.Clamp01((Time.time - lastSlideTime) / _characterData.slideCooldown) * _characterData.slideBoost;

				
			}
			_capsuleColider.height = _characterData.crouchHeight;
			_capsuleColider.radius = _characterData.crouchRadius;
		}

		private void ExitCrouch()
		{
			isSliding = false;
			lastSlideTime = Time.time;
			isCrouching = false;
			_capsuleColider.radius = _originalCapsuleSize.x;
			_capsuleColider.height = _originalCapsuleSize.y;
		} 

		private void SetVelocity()
		{
			Vector3 dir = playerInputSpace.TransformDirection(_moveDirection);
			dir = new Vector3(dir.x, 0, dir.z);
			dir.Normalize();
			Vector3 velocity = (dir * (_CurrentSpeed * _inputAmount));
			velocity.y = _adjustVerticalVelocity * _characterData.gravityMultiplier;
			_body.velocity = velocity;
		}

		private bool IsGrounded()
		{
			//Todo Make Better
			
			Debug.DrawRay(_transform.position + _capsuleColider.center, Vector3.down * maxDistance, Color.red);

			//return Physics.Raycast(_transform.position + _capsuleColider.center, Vector3.down, maxDistance);
			Ray ray = new Ray(_transform.position, Vector3.down);
			return Physics.SphereCast(ray, 0.4f, maxDistance);
		}
	}
}
