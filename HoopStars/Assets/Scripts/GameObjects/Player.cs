using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    bool isJumping;
    [SerializeField] float jumpForce = 0.6f;
    [SerializeField] float moveForce = 0.3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJumping) {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    private void pointersPressedHandler(object sender, PointerEventArgs e)
    {
        if (e.Pointers[0].Position.x > Screen.width / 2) {
            rb.AddForce(moveForce * 1.0f, jumpForce * 1.0f, 0.0f, ForceMode.Impulse);
        } else {
            rb.AddForce(moveForce * -1.0f, jumpForce * 1.0f, 0.0f, ForceMode.Impulse);
        }
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    private void OnEnable()
    {
        if (TouchManager.Instance != null) 
            TouchManager.Instance.PointersPressed += pointersPressedHandler;
    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null) 
            TouchManager.Instance.PointersPressed -= pointersPressedHandler;
    }
}
