using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public event Action OnPlayerHide;

    private bool isGrounded;
    private bool canMove;


    // Start is called before the first frame update
    void Start()
    {
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
        if(canMove)
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bush")
        {
            Debug.Log("Collided");
            this.gameObject.SetActive(false);   
            OnPlayerHide?.Invoke();
            canMove = false;
        }       

            
    }
}
