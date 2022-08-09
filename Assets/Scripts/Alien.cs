using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D rig;
    private Animator anim;

    [SerializeField] [Range(0, 1)] float LerpConstant;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float currentRotation = GameController.instance.currentRotation;
        float xVel = rig.velocity.x;
        float yVel = rig.velocity.y;
        float movement = Input.GetAxis("Horizontal");
        switch (currentRotation)
        {
            case RotationUtils.ROTATED_BOTTOM:
                rig.velocity = new Vector2(movement * Speed, yVel);
                break;
            case RotationUtils.ROTATED_RIGHT:
                rig.velocity = new Vector2(xVel, movement * Speed); 
                break;
            case RotationUtils.ROTATED_TOP:
                rig.velocity = new Vector2(-movement * Speed, yVel); 
                break;
            case RotationUtils.ROTATED_LEFT:
                rig.velocity = new Vector2(xVel, -movement * Speed); 
                break;
        }

        if (movement > 0)
        {
            if (!anim.GetBool("walk"))
            {
                anim.SetBool("walk", true);
                transform.eulerAngles = new Vector3(0, 0, currentRotation);
            }
        }

        if (movement < 0)
        {
            if (!anim.GetBool("walk"))
            {
                anim.SetBool("walk", true);

                transform.eulerAngles = currentRotation switch
                {
                    RotationUtils.ROTATED_BOTTOM => new Vector3(0, 180, currentRotation),
                    RotationUtils.ROTATED_RIGHT => new Vector3(180, 0, currentRotation),
                    RotationUtils.ROTATED_TOP => new Vector3(0, 180, currentRotation),
                    RotationUtils.ROTATED_LEFT => new Vector3(180, 0, currentRotation),
                    _ => new Vector3(0, 0, 0)
                };
            }
        }

        if (movement == 0)
        {
            anim.SetBool("walk", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            float currentRotation = GameController.instance.currentRotation;
            Vector2 boxPosition = collision.transform.position;
            Vector2 alienPosition = transform.position;
            Debug.Log("BOX: " + boxPosition.x + ", " + boxPosition.y);
            Debug.Log("ALIEN: " + alienPosition.x + ", " + alienPosition.y);
            Debug.Log("ROTATION: " + (int) currentRotation);
            bool mustDie = currentRotation switch
            {
                RotationUtils.ROTATED_BOTTOM => boxPosition.y - alienPosition.y > 0.40,
                RotationUtils.ROTATED_RIGHT => boxPosition.x - alienPosition.x < -0.40,
                RotationUtils.ROTATED_TOP => boxPosition.y - alienPosition.y < -0.40,
                RotationUtils.ROTATED_LEFT => boxPosition.x - alienPosition.x > 0.40,
            };
            if (mustDie)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        GameController.instance.ShowGameOver();
        Destroy(gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            float currentRotation = GameController.instance.currentRotation;
            if (currentRotation == RotationUtils.ROTATED_BOTTOM || currentRotation == RotationUtils.ROTATED_TOP)
            {
                rig.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                rig.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            GameController.instance.UnlockDoor();
            Destroy(collision.gameObject);
        }
    }
}
