using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
	[Header("Nut settings lmao")]
	public int storageSize;
	public int storedNuts;
	public float rumbleDistance;
	[Header("Hole depth/height offsettings")]
	public float digTime = 2;
	public float holeDepth;
	[Header("Effects & juice")]
	public GameObject diggingParticles;
	public AudioClip diggingSFX;
    private GameObject acorn;
    private SpriteRenderer sprite;

	private float lerpTimer;
    private float pingPongTimer;
    private float spriteTimer;
    private bool lerpActive;
    private bool acornTriggered = false;
    private bool enteredCollider = false;
	private float startHeight;
	private GameObject player;
	private Collider col;
	private AudioSource audioSrc;

    private Color transparent = new Color(1f, 1f, 1f, 0f);

	private void OnEnable()
    {
        acorn = this.gameObject.transform.Find("acorn").gameObject;
        sprite = acorn.gameObject.transform.Find("sprite-a").GetComponent<SpriteRenderer>();
        GameManager.holeReset += ResetHole;
        sprite.color = transparent;
    }

	private void OnDisable()
	{
		GameManager.holeReset -= ResetHole;
        sprite.color = transparent;
    }

	void Start()
	{
		startHeight = transform.position.y;
		player = GameObject.Find("Player");
		col = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
	{
		LerpPosition();
        PingPongAcorn();

        LerpSpriteColor();

	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			CharacterController.DigHole += DigTheHole;
            enteredCollider = true;
        }
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			CharacterController.DigHole -= DigTheHole;
            enteredCollider = false;
        }
    }

	private void DigTheHole()
	{
		CharacterController.DigHole -= DigTheHole;
		col.enabled = false;
		Instantiate(diggingParticles, transform);
		AudioManager.PlaySound("Digging");
		diggingParticles.GetComponent<ParticleKiller>().killTimer = digTime;
		lerpTimer = 0f;
		lerpActive = true;


		if (GameManager.instance.currentState == GameManager.GameState.PlayingFall || GameManager.instance.currentState == GameManager.GameState.TutorialFall)
		{
			storedNuts = 1;

            sprite.color = transparent;


            if (GameManager.instance.currentState == GameManager.GameState.TutorialFall)
			{
				GameManager.instance.EnterState(GameManager.GameState.PlayingFall);
			}
		}
		else if (GameManager.instance.currentState == GameManager.GameState.PlayingWinter || GameManager.instance.currentState == GameManager.GameState.TutorialWinter)
        {
            sprite.color = transparent;
            if (storedNuts > 0)
			{
				acornTriggered = false;
				GameManager.nutsAmount += storedNuts;
                AudioManager.PlaySound("FindNut");
			}

			storedNuts = 0;
			if (GameManager.instance.currentState == GameManager.GameState.TutorialWinter)
			{
				GameManager.instance.EnterState(GameManager.GameState.PlayingWinter);
			}
		}

	}

	public void ResetHole()
    {
        sprite.color = transparent;
        if (GameManager.instance.currentState == GameManager.GameState.PlayingWinter)
		{
			acornTriggered = true;
			ResetForFall();
		}
		else
		{
			ResetForWinter();
		}

	}

	public void ResetForWinter()
	{
		col.enabled = true;
        acornTriggered = true;
        transform.position = new Vector3(transform.position.x, startHeight - holeDepth, transform.position.z);

		acorn.transform.localPosition = new Vector3(0f, -0.25f, 0f);

		//hole mesh in de grond zodat hij naar boven kan poppen
	}

	public void ResetForFall()
	{
		col.enabled = true;
        acornTriggered = false;
		transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
		acorn.transform.localPosition = new Vector3(0f, 1.25f, 0f);
		storedNuts = 0;
	}

	private void LerpPosition()//lerp lerp baby
	{
		if (lerpActive)
		{
			lerpTimer += Time.deltaTime;
			if (GameManager.instance.currentState == GameManager.GameState.PlayingFall || GameManager.instance.currentState == GameManager.GameState.TutorialFall)
			{
				transform.position = Vector3.Lerp(new Vector3(transform.position.x, startHeight, transform.position.z), //hoogste punt
					new Vector3(transform.position.x, startHeight - holeDepth, transform.position.z), lerpTimer / digTime);//laagste punt

                acorn.transform.localPosition = Vector3.Lerp(new Vector3(0f, 1.25f, 0f), //hoogste punt
                       new Vector3(0f, -0.2f, 0f), lerpTimer / digTime);//laagste punt


                sprite.color = transparent;

            }
			else if (GameManager.instance.currentState == GameManager.GameState.PlayingWinter || GameManager.instance.currentState == GameManager.GameState.TutorialWinter)
            {
                sprite.color = transparent;
                transform.position = Vector3.Lerp(new Vector3(transform.position.x, startHeight - holeDepth, transform.position.z), //laagste punt
					new Vector3(transform.position.x, startHeight, transform.position.z), lerpTimer / digTime);//hoogste punt

				if(storedNuts > 0)
				{
					acorn.transform.localPosition = Vector3.Lerp(new Vector3(0f, -0.2f, 0f), //hoogste punt
						   new Vector3(0f, 1.25f, 0f), lerpTimer / digTime);//laagste punt
				}
            }

			if (lerpTimer >= digTime)
			{
				lerpTimer = 0f;
				lerpActive = false;
                sprite.color = transparent;
            }
		}
	}

    private void PingPongAcorn()
    {
        if(!acornTriggered)
        {
            pingPongTimer += Time.deltaTime;
            acorn.transform.localPosition = Vector3.Lerp(new Vector3(0f, 1.25f, 0f), //hoogste punt
                   new Vector3(0f, 1.75f, 0f), Mathf.PingPong(pingPongTimer / digTime, 1f));//laagste punt
        }

        if (lerpActive)
        {
            acornTriggered = true;
        }
    }




    private Color LerpColor(Color from, Color to)
    {
        Color lerpedColor = Color.Lerp(from, to, spriteTimer);

        if (spriteTimer < 1)
        {
            spriteTimer += Time.deltaTime / ( digTime / 3 );
        }

        return lerpedColor;
    }

    private void LerpSpriteColor()
    {
        if (!enteredCollider)
        {
            if (!lerpActive)
            {
                if (GameManager.instance.currentState != GameManager.GameState.PlayingWinter)
                {
                    sprite.color = Color.Lerp(transparent, Color.white, Mathf.PingPong(spriteTimer / digTime, 1f));
                }
            }
        }
        else
        {
            if (GameManager.instance.currentState != GameManager.GameState.PlayingWinter)
            {
                sprite.color = Color.Lerp(Color.white, transparent,spriteTimer / digTime);
            }
        }
      
    }
}
