using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public CharacterController controller;

	public event System.Action OnReachedEndOfLevel;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	// Update is called once per frame
	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

	}

	void FixedUpdate()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Finish")
		{
			if (OnReachedEndOfLevel != null)
			{
				OnReachedEndOfLevel();
			}
		}
	}
}