using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public UIController uiController;
	public static int nutsAmount;
	public TextMeshProUGUI nutsAmountUI;
	public GameObject camObject;
	public float roundLengthSeconds = 100;
	[Header("ground, materials & weather")]
	public GameObject ground;
	public Material regularGroundMat;
	public Material snowyGroundMat;
	public GameObject snowParticles;


	public GameObject hole;
	public GameObject tutorialHole;
	public PostProcessVolume PostProc;
	public GameObject tutorialCagePrefab;
	public GameObject NameOffspringMenu;
	private GameObject cage;

	[Header("baby's en schedels")]
	public List<GameObject> babies = new List<GameObject>();
	public GameObject skullPrefab;

	[Header("al de fucking UI elementen")]
	public GameObject clock;
	public GameObject splashLogo;
	public GameObject aToContinue;
	public GameObject introduction;
	public GameObject introKids;
	public GameObject fallTutorialIntro;
	public GameObject winterTutorialIntro;
	public GameObject winterSuccess;
	public GameObject winterFailure;
	public GameObject squirrelFeedPanel;
	public List<GameObject> babyIcons = new List<GameObject>();
	public List<GameObject> acorns = new List<GameObject>();

	private CharacterController player;
	private bool enteringState = true;
	private CameraController camController;
	private float roundTimer;
	Image timerImage;
	Image timerBorder;

	private int roundNumber;



	//XInput 
	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	public float rumbleIntensity;
	//XInput 


	private IntroScene introScene;

	public delegate void ResetHoles();
	public static event ResetHoles holeReset;



	void Awake()
	{
		instance = this;
		uiController = GameObject.Find("Canvas").GetComponent<UIController>();
	}


	void Start()
	{
		introScene = new IntroScene();
		introScene.Initialize();
		nutsAmount = 0;
		NameOffspringMenu = GameObject.Find("InputNames");
		player = GameObject.Find("Player").GetComponent<CharacterController>();
		//nutsAmountUI = GameObject.Find("NutsAmount").GetComponent<TextMeshProUGUI>();
		timerImage = GameObject.Find("Timer").GetComponent<Image>();
		timerBorder = GameObject.Find("TimerBorder").GetComponent<Image>();
		camObject = GameObject.Find("Camera");
		PostProc = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
		camController = camObject.GetComponent<CameraController>();
		roundTimer = roundLengthSeconds;
		//NameOffspringMenu.SetActive(false);
		SpawnHoles();
		cage = Object.Instantiate(tutorialCagePrefab, Vector3.zero, Quaternion.identity);
		clock.SetActive(false);
	}

	void Update()
	{
		NutsCounter();
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			OffspringKillCheck();
		}

	}

	private void FixedUpdate()
	{
		StateMachine();
		Rumble();
	}

	public enum GameState
	{
		Start,
		nameOffspring,
		TutorialFall,
		TutorialWinter,
		PlayingFall,
		PlayingWinter,
		Starvation,
		Continue,
		Lose
	}

	public GameState currentState = GameState.Start;

	void StateMachine()
	{
		switch (currentState)
		{
			case GameState.Start:
				if (enteringState == true)
				{
					//herfstProcessing();
					introScene.EnterStartScene();
					player.EnterState(CharacterController.PlayerState.Inactive);
					enteringState = false;
				}
				introScene.StartScene();
				break;

			case GameState.nameOffspring:
				if (enteringState == true)
				{
					herfstProcessing();
					//NameOffspringMenu.gameObject.SetActive(true);
					introScene.EnterNamingScene();
					enteringState = false;
				}
				introScene.NamingScene();
				break;

			case GameState.TutorialFall:
				if (enteringState == true)
				{
					
					cage.SetActive(true);
					herfstProcessing();
					introScene.EnterTutorialScene();
					player.EnterState(CharacterController.PlayerState.Active);
					enteringState = false;
				}
				introScene.FallTutorialScene();

				break;
			case GameState.TutorialWinter:
				if (enteringState == true)
				{
					clock.SetActive(false);
					cage.SetActive(true);
					winterProcessing();
					introScene.EnterTutorialScene();
					player.EnterState(CharacterController.PlayerState.Active);
					enteringState = false;
					player.EnableSnowTrail(true);
				}
				introScene.WinterTutorialScene();
				break;

			case GameState.PlayingFall:
				if (enteringState == true)
				{
					clock.SetActive(true);
					fallTutorialIntro.SetActive(false);
					aToContinue.SetActive(false);
					cage.SetActive(false);
					herfstProcessing();
					enteringState = false;
					squirrelFeedPanel.SetActive(false);

				}
				ClockTimer();
				camController.FollowTarget();
				break;

			case GameState.PlayingWinter:
				if (enteringState == true)
				{
					clock.SetActive(true);
					winterTutorialIntro.SetActive(false);
					aToContinue.SetActive(false);
					cage.SetActive(false);
					winterProcessing();
					enteringState = false;
					player.EnableSnowTrail(true);
					squirrelFeedPanel.SetActive(true);
				}
				ClockTimer();
				DisplayNuts();
				camController.FollowTarget();
				break;

			case GameState.Starvation:
				if (enteringState == true)
				{
					enteringState = false;
					Debug.Log("STARVING");
					OffspringKillCheck();
					clock.SetActive(false);
					/*
					uiController.SetState(UIController.UIState.GAME_TO_WHITE);
					if (uiController.currentState == UIController.UIState.NONE)
					{
					}
					*/
				}
				break;

			case GameState.Continue:
				if (enteringState == true)
				{
					enteringState = false;
					AudioManager.PlayMusic("WinMusic");
					winterSuccess.SetActive(true);
					introScene.EnterNamingScene();
				}
				introScene.NamingScene();
				if (Input.GetButtonDown("Fire1"))
				{
					winterSuccess.SetActive(false);
					StartCoroutine(CoolTransition(GameState.Start, 3));
				}
				break;

			case GameState.Lose:
				if (enteringState == true)
				{
					enteringState = false;
					AudioManager.PlaySound("Death");
					introScene.EnterNamingScene();
					AudioManager.PlayMusic("LoseMusic");
					winterFailure.SetActive(true);
				}
				introScene.NamingScene();
				if (Input.GetButtonDown("Fire1"))
				{
					StartCoroutine(CoolTransition(GameState.Start, 3));
				}
				break;
		}
	}


	void Rumble()
	{
		GamePad.SetVibration(playerIndex, rumbleIntensity, rumbleIntensity);
	}

	void ResetPlayer()
	{
		player.transform.position = Vector3.zero;
		camObject.transform.position = Vector3.zero;
	}

	void ChangeGroundMat(Material mat)
	{
		ground.GetComponent<Renderer>().material = mat;
	}

	void OffspringKillCheck()
	{
		clock.SetActive(false);
		int aliveKids = 5;//GameObject.Find("NameOffspringMenu").GetComponent<OffspringMenu>().saveNames.Length;
		print("Number of nuts: " + nutsAmount);
		print("Number of alive offspring: " + nutsAmount / aliveKids);

		//print("Number of alive offspring if nuts is less than offspring amount: " + (nutsAmount - aliveKids));

		int deadKids = Mathf.Abs(nutsAmount - aliveKids);
		if ((nutsAmount - aliveKids) < 0)
		{

			EnterState(GameState.Lose);
			StartCoroutine(KillOffspring(deadKids));
		}
		else
		{
			EnterState(GameState.Continue);
		}
	}

	IEnumerator KillOffspring(int amount)
	{
		int _amount = Mathf.Abs(amount);
		if (_amount > 5)
			_amount = 5;
		Debug.Log(_amount);

		for (int i = 0; i < babies.Count; i++)
		{
			babies[i].GetComponent<SinBounce>().jumping = false;
		}
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < _amount; i++)
		{
			babies[i].transform.position = new Vector3(babies[i].transform.position.x, babies[i].transform.position.y + 1f, babies[i].transform.position.z);
			Object.Instantiate(skullPrefab, babies[i].transform.position, babies[i].transform.rotation);
			babies[i].SetActive(false);
		}

	}



	void DisplayNuts()
	{
		for (int i = 0; i < 5; i++)
		{
			if (i < nutsAmount)
			{
				acorns[i].GetComponent<Image>().color = Color.white;
				babyIcons[i].GetComponent<Image>().color = Color.white;
			}
			else
			{
				acorns[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
				babyIcons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
			}
		}
	}

	void ClockTimer()
	{
		if (uiController.currentState == UIController.UIState.NONE)
		{
			roundTimer -= Time.deltaTime;
		}

		timerImage.fillAmount = (roundTimer / roundLengthSeconds);
		if (roundTimer <= roundLengthSeconds / 4)
		{
			GameObject.Find("Timer").GetComponent<Animation>().Play();
		}

		if (roundTimer <= 0)
		{
			if (holeReset != null)
			{
				holeReset.Invoke();
				//EnterState(PlayerState.Digging);
			}
			if (currentState == GameState.PlayingFall)
			{

				if (roundNumber == 0)
				{
					StartCoroutine(CoolTransition(GameState.TutorialWinter, 2));
					//EnterState(GameState.TutorialWinter);
				}
				else
				{
					StartCoroutine(CoolTransition(GameState.PlayingWinter, 2));
					//EnterState(GameState.PlayingWinter);
				}
				roundTimer = roundLengthSeconds;
				//TransitionToWinter();
			}
			else
			{
				roundNumber++;
				tutorialHole.SetActive(false);
				OffspringKillCheck();
				//EnterState(GameState.PlayingFall);

				//EnterState(GameState.Starvation);



			}
		}
	}

	void SpawnHoles()
	{
		var holes = GameObject.FindGameObjectsWithTag("spawn hole");
		System.Random rnd = new System.Random();
		var rndHoles = holes.OrderBy(x => rnd.Next()).Take(15);

		foreach (GameObject obj in rndHoles)
		{
			var newHole = Object.Instantiate(hole, obj.transform.position, obj.transform.rotation);
		}
	}

	void NutsCounter()
	{
		//nutsAmountUI.text = nutsAmount.ToString();
	}

	void TransitionToWinter()
	{
		AudioManager.PlayAmbient("Winter");
		ChangeGroundMat(snowyGroundMat);
		ResetPlayer();
		snowParticles.SetActive(true);
	}

	void TransitionToFall()
	{
		AudioManager.PlayAmbient("Fall");
		ChangeGroundMat(regularGroundMat);
		ResetPlayer();
		snowParticles.SetActive(false);
	}

	IEnumerator CoolTransition(GameState state, int nothingFallWinter)//0 als er geen weather change is, 1 als het herfst wordt, 2 als het winter wordt, 3 RESET SCENE
	{
		uiController.SetState(UIController.UIState.GAME_TO_WHITE);
		yield return new WaitForSeconds(1.1f);
		roundTimer = roundLengthSeconds;
		switchClock();
		EnterState(state);
		if (nothingFallWinter == 1)
		{
			TransitionToFall();
		}
		else if (nothingFallWinter == 2)
		{
			TransitionToWinter();
			nutsAmount = 0;
		}
		else if (nothingFallWinter == 3)
		{
			SceneManager.LoadScene("LevelResetter");
		}
	}


	public void EnterState(GameState state)
	{
		enteringState = true;
		currentState = state;
	}

	public void herfstProcessing()
	{
		RenderSettings.ambientEquatorColor = new Color32(207, 120, 60, 0);
		PostProc.profile = Resources.Load<PostProcessProfile>("Herfst");
	}

	public void winterProcessing()
	{
		RenderSettings.ambientEquatorColor = new Color32(214, 238, 251, 0);
		PostProc.profile = Resources.Load<PostProcessProfile>("Winter");
	}
	public void switchClock()
	{
		Sprite StoreCurrentParent = timerBorder.sprite;
		timerBorder.sprite = timerImage.sprite;
		timerImage.sprite = StoreCurrentParent;
	}
}