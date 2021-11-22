using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private PlayerAttributes playerAttributes;

    public static PlayerControl Instance;

    //jump variables
    [Header("Ground Check")]
    public float distance = 0.6f;
    public LayerMask whatIsGround;

    //combonent variables
    private Rigidbody2D rb;


    private void Start()
    {
        Instance = this;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(x * playerAttributes.Speed, rb.velocity.y);
        if (Input.GetKeyDown("w") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, playerAttributes.Jump);
            PopUpManager.Instance.Open("sdadas", "sdasdas", 3f);
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, whatIsGround);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
