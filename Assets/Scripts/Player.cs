using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public event Action OnPlayerHide;
    public bool canMove;

    private bool isGrounded;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (canMove)
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed;
        //transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Bush")
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Collided");
                this.gameObject.SetActive(false);
                OnPlayerHide?.Invoke();
                canMove = false;
            }
        }
    }
}
