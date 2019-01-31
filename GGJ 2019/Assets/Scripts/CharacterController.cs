using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float moveSpeed;
	public GameObject camObject;
	private Rigidbody rb;
	private Vector3 velocity;
	private CameraController camController;
	private Animator anim;
	private bool enteringState;
	private ParticleSystem snowTrail;
	GameObject rumbleHole;
	private bool rumbleEngaged;
	private float rumbleDistance = 1.5f;
	int entercounter;

	public delegate void HoleEvent();
	public static event HoleEvent DigHole;

	public enum PlayerState
	{
		Inactive,
		Active,
		Digging
	}

	public PlayerState currentState = PlayerState.Inactive;

	void Start()
	{
		rb = this.gameObject.GetComponent<Rigidbody>();
		camController = camObject.GetComponent<CameraController>();
		anim = gameObject.GetComponent<Animator>();
		snowTrail = this.gameObject.transform.Find("Particle System").GetComponent<ParticleSystem>();
		snowTrail.Stop();
		snowTrail.Pause();
	}

	void FixedUpdate()
	{
		StateMachine();
		Rumble();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "hole")
		{
			rumbleHole = other.gameObject;
			entercounter++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "hole")
		{
			entercounter--;

		}
	}

	void Rumble()
	{

		if (entercounter > 0)
		{ 
			GameManager.instance.rumbleIntensity = 1f - Vector3.Distance(transform.position, rumbleHole.transform.position) / rumbleDistance;
		}
		else
		{
			GameManager.instance.rumbleIntensity = 0f;
		}
	}

	void StateMachine()
	{
		switch (currentState)
		{
			case PlayerState.Inactive:
				if (enteringState == true)
				{
					enteringState = false;
				}
				SetCameraTarget();
				//Chill effe hey, jezus
				break;

			case PlayerState.Active:
				if (enteringState == true)
				{
					enteringState = false;
				}
				PlayerControls();
				SetCameraTarget();
				break;

			case PlayerState.Digging:
				if (enteringState == true)
				{
					enteringState = false;
				}
				SetCameraTarget();
				break;
		}
	}

	void Dig()
	{
		if (DigHole != null)
		{
			DigHole.Invoke();
			//EnterState(PlayerState.Digging);
		}
	}

	void PlayerControls()
	{
		velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
		rb.velocity = velocity;
		if (rb.velocity.magnitude > 0.5f)
			transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		if (velocity.magnitude > 0.5f)
		{

			anim.SetBool("Moving", true);
			if (!AudioManager.audioEventSrc.isPlaying)
			{
				AudioManager.PlaySound("Walk");
			}
		}
		else
		{
			anim.SetBool("Moving", false);
			//anim.SetFloat("Velocity", velocity.magnitude);
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Dig();
		}
	}

	void SetCameraTarget()
	{
		camController.target = transform.position + rb.velocity;
	}

	public void EnterState(PlayerState state)
	{
		enteringState = true;
		currentState = state;
	}

	public void EnableSnowTrail(bool b)
	{
		Debug.Log("snowtrail");
		snowTrail.Stop();
		snowTrail.Play();
		/*
		if (b)
		{
			snowTrail.Play();
		}
		else
		{
			snowTrail.Stop();
		}
		*/
	}
}
