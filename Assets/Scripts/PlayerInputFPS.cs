using System;
using UnityEngine;

namespace FG
{
    public class PlayerInputFPS : MonoBehaviour
    {
        private CharacterMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            _movement.forwardInput = Input.GetAxis("Vertical");
            _movement.sidewaysInput = Input.GetAxis("Horizontal");
            _movement.jumpInput = Input.GetButton("Jump");
            _movement.crouchInput = Input.GetButtonDown("Crouch");
            _movement.runInput = Input.GetKey(KeyCode.LeftShift);
        }
    }
}

