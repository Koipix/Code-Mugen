using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, HealthComponent
{
    Transform target;
    Vector2 moveDirection;
    SpriteRenderer sprite;
    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D bc;

    private float moveSpeed = 1.5f;
    public float Health { get; set; } = 60f;

    //reset enemy state
    public void ResetEnemy() {
        Health = 100f;
        bc.enabled = true;
        sprite.enabled = true;

        Color spriteColor = sprite.color;
        spriteColor.a = 1f;
        sprite.color = spriteColor;
    }

    public void takeDamage(float amount) {
        Health -= amount;
        animator.ResetTrigger("Hit");

        //play death animatoin if dead
        if (Health <= 0) {

            bc.enabled = false; //stops restarting animation state

            animator.SetTrigger("Kill");
            return;
        }

        //play hit animation if still alive
        animator.SetTrigger("Hit");
    }

    //called by Death Animation State
    public void DestroyObject() {
        gameObject.SetActive(false);
    }

    void Awake() {
        target = GameObject.Find("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        rb.freezeRotation = true;
    }

    void Start() {

    }

    void Update() {
        if (target) {
            Vector2 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            //flip sprite depending on player's position
            sprite.flipX = direction.x < 0;
        }
    }

    private void FixedUpdate() {
        if (target) {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * moveDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Weapon") {
            takeDamage(20);
        }
    }
}
