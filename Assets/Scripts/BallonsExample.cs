using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonsExample : MonoBehaviour
{
    private float ballons;
    private float ballonForce;
    private float previusBallonAmount;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.AddForce(Vector2.up * (ballonForce * ballons * Time.deltaTime));
    }
}