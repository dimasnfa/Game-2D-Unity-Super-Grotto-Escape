using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Movement speed
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private Rigidbody2D rb; // Reference to Rigidbody2D component
// (assumed attached)
    public bool isGrounded; // Flag to track ground contact
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float rangeGround = 0f; // range character to ground
    private bool flip = false;
// public Animator animator; // Animation
    private Animator anim;
    private enum State {idle,running,jumping,falling}
    private State state = State.idle;
    private float horizontalInput;
    void Awake()
{
    anim = GetComponent<Animator>();
}
private void Update()
{
    if (Time.deltaTime != 0)
    {
AnimationState();
anim.SetInteger("state",(int)state);
}
// Get horizontal input (left/right movement)
horizontalInput = Input.GetAxisRaw("Horizontal");
// animator.SetFloat("Speed",Mathf.Abs(horizontalInput));
// Update velocity based on input and speed
rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
// Check if grounded using Physics2D.OverlapCircle
isGrounded = Physics2D.OverlapCircle(transform.position, rangeGround,
targetLayer); // Adjust radius and ground layer as needed
// Handle jumping on spacebar press and grounded state
if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
{
rb.velocity = new Vector2(rb.velocity.x, jumpForce);
state = State.jumping;
}
// Memanggil fungsi Flip() jika pemain bergerak ke arah yang berlawanan dengan
// arah yang sedang dihadapinya
if (horizontalInput < 0 && !flip )
{
Flip();
flip = true;
}
else if (horizontalInput > 0 && flip)
{
Flip();
flip = false;
}
}
void Flip(){
// Memutar objek secara horizontal dengan membalikkan nilai skala pada sumbu X
transform.localScale = new Vector3(-transform.localScale.x,
transform.localScale.y, transform.localScale.z);
}
void AnimationState(){
if(state == State.jumping){
if(rb.velocity.y < .3f){
state = State.falling;
}
}else if(state== State.falling){
if(isGrounded){
state = State.idle;
}
}else if(horizontalInput != 0){
state = State.running;
}else{
state = State.idle;
}
}
}
