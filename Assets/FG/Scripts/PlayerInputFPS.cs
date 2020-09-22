using UnityEngine;

namespace FG
{
    public class PlayerInputFPS : MonoBehaviour
    {
        private CharacterMovement movement;

        private void Awake()
        {
            movement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            movement.forwardInput = Input.GetAxis("Vertical");
            movement.sidewaysInput = Input.GetAxis("Horizontal");
            movement.jumpInput = Input.GetButton("Jump");
            if (Input.GetButtonDown("Crouch"))
            {
                movement.crouchInput = true;
            }

            if (Input.GetButtonUp("Crouch"))
            {
                movement.crouchInput = false;
            }
            movement.runInput = Input.GetKey(KeyCode.LeftShift);
        }
    }
}

