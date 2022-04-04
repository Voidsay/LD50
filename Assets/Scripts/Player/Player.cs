using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] Transform cam;
    [SerializeField] Vector3 gravity;
    [SerializeField] float jumpforce;
    [SerializeField] Vector3 groundCheck;

    public bool isGrounded = true;

    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.OverlapSphere(transform.position + groundCheck, 0.1f).Length > 0)
        {
            isGrounded = true;
        }
        rb.velocity = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * speed + Vector3.up * rb.velocity.y;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(jumpforce * Vector3.up, ForceMode.Impulse);
            isGrounded = false;
        }
        rb.AddForce(gravity);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * Manager.instance.sensX, 0);
        var temp = cam.rotation;
        cam.rotation *= Quaternion.Euler(Input.GetAxis("Mouse Y") * Manager.instance.sensY, 0, 0);
        if (Vector3.Angle(transform.forward, cam.forward) > 90)
        {
            cam.rotation = temp;
        }
    }
}
