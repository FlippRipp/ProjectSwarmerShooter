using UnityEngine;

namespace FG
{
	[CreateAssetMenu(fileName = "Character Data", menuName = "FG/CharacterData")]
	public class CharacterData : ScriptableObject
	{
		[Header("Moving")]
		public float walkingSpeed = 6.75f;
		public float runSpeed = 10f;
		
		[Header("Crouching")]
		public float crouchSpeed = 4f;
		public float crouchHeight = 4f;
		public float crouchRadius = 4f;
		
		[Header("Sliding")]
		public float slideVelocityThreshold = 2;
		public float slideTurnMultiplier = 1;
		public float slideBoost = 1.4f;
		public float slideDrag = 10f;
		public float slideCooldown = 2;

		[Header("Wall Running")]
		public float maxDistanceFromWall = 1;
		public float wallRunningSpeed = 6.75f;

		[Header("Jumping")]
		public float jumpForce = 8f;
		public float gravityMultiplier = 1f;
		public float inAirMovementMultiplier = 1f;
	}
}