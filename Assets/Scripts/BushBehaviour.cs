using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushBehaviour : MonoBehaviour
{
    Player player;
    bool shouldMove = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnPlayerHide += OnPlayerHide;
    }

    private void OnPlayerHide()
    {
        shouldMove = true;
        player.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            Movement();
            if (Input.GetMouseButtonDown(0))
            {
                player.gameObject.SetActive(true);
                player.transform.SetParent(null);
                shouldMove = false;
                player.canMove = true;
            }
        }
    }

    void Movement()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 3f * Time.deltaTime;
    }
}
