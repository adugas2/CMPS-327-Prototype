using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float jumpForce = 400f;                           
	[Range(0, .3f)] [SerializeField] private float smoothing = .05f;  
	[SerializeField] private bool airControl = false;                        
	[SerializeField] private LayerMask whatIsGround;                          
	[SerializeField] private Transform groundCheck;                                                                   

	const float groundedRadius = .2f; 
	private bool isGrounded;             
	private Rigidbody2D rb;
	private bool isFacingRight = true;  
	private Vector3 velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = isGrounded;
		isGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump)
	{
		if (isGrounded || airControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

			if (move > 0 && !isFacingRight)
			{
				Flip();
			}
			else if (move < 0 && isFacingRight)
			{
				Flip();
			}
		}

		if (isGrounded && jump)
		{
			isGrounded = false;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip()
	{
		isFacingRight = !isFacingRight;
		transform.Rotate(0f, 180f, 0f);
	}
}