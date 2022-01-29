using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        theRB.velocity = moveInput * moveSpeed;

        if (moveInput.x > 0f)
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (moveInput.x < 0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tree" && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Hello");
            collision.GetComponent<HarvestScript>().Chopped();
            collision.GetComponent<HarvestScript>().Harvesting();
        }
    }
}
