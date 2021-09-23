using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.5f;
    private Rigidbody rb;
    float smoothinputMagnitude;
    float smoothmoveVelocity;
    float smoothmoveTime = .1f;
    public float turnspeed = 8;
    float angle;
    Vector3 Velocity;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        smoothinputMagnitude = Mathf.SmoothDamp(smoothinputMagnitude, inputMagnitude, ref smoothmoveVelocity, smoothmoveTime);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnspeed * inputMagnitude);
        Velocity = transform.forward * speed * smoothinputMagnitude;
        if (smoothinputMagnitude > 0.01)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("isRunning", true);
            speed = speed * 2;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRunning", false);
            speed = speed / 2;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("isJumping");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("isAttacking");
        }
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + Velocity * Time.deltaTime);
    }

}
