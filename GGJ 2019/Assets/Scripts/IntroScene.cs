using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
	private Transform StartCameraPosition;
	private Transform OffspringCameraPosition;
	private Transform TutorialCameraPosition;
	public UIController uiController;
	private bool intro1, intro2, intro3, fallIntro, winterIntro;
	Transform CurrentCameraDummy;

	float startTime;
	public bool cameraLerping;

	int state = 0;


	public void Initialize()
	{
		//CurrentCameraDummy = Camera.main.transform;
		uiController = GameObject.Find("Canvas").GetComponent<UIController>();
		StartCameraPosition = GameObject.Find("StartPosition").transform;
		OffspringCameraPosition = GameObject.Find("OffspringPosition").transform;
		TutorialCameraPosition = GameObject.Find("TutorialPosition").transform;
		uiController.SetState(UIController.UIState.WHITE_TO_GAME);
	}

	public void EnterStartScene()
	{
		CurrentCameraDummy = StartCameraPosition;
		Camera.main.transform.position = CurrentCameraDummy.position;
		Camera.main.transform.rotation = CurrentCameraDummy.rotation;

	}

	public void StartScene()
	{
		if (!GameManager.instance.splashLogo.activeSelf)
		{
			GameManager.instance.splashLogo.SetActive(true);
			GameManager.instance.aToContinue.SetActive(true);
		}
		if (Input.GetButtonDown("Fire1"))
		{
			GameManager.instance.EnterState(GameManager.GameState.nameOffspring);
			GameManager.instance.splashLogo.SetActive(false);
			GameManager.instance.aToContinue.SetActive(false);
		}
	}

	public void EnterNamingScene()
    {
		CurrentCameraDummy.position = Camera.main.transform.position;
		CurrentCameraDummy.rotation = Camera.main.transform.rotation;
		setStartTime();
	}

	public void NamingScene()
	{
		if (cameraLerping)
		{
			LerpCamera(CurrentCameraDummy, OffspringCameraPosition, 35f);
		}
        
		if (!cameraLerping && !intro1 )
		{
			GameManager.instance.introduction.SetActive(true);
			GameManager.instance.aToContinue.SetActive(true);
			intro1 = true;
		}
		else if (Input.GetButtonDown("Fire1") && !cameraLerping && intro1 && !intro2)
		{
			GameManager.instance.introduction.SetActive(false);
			GameManager.instance.introKids.SetActive(true);
			intro2 = true;
		}
		else if (Input.GetButtonDown("Fire1") && !cameraLerping && intro2)
		{
			GameManager.instance.introKids.SetActive(false);
			GameManager.instance.aToContinue.SetActive(false);
			GameManager.instance.EnterState(GameManager.GameState.TutorialFall);
			
		}
	}

	public void EnterTutorialScene()
	{
		setStartTime();
	}

	public void FallTutorialScene()
	{
		if (cameraLerping)
		{
			LerpCamera(OffspringCameraPosition, TutorialCameraPosition, 1.5f);
		}
		else if (!fallIntro)
		{
			GameManager.instance.fallTutorialIntro.SetActive(true);
			fallIntro = true;
		}
		else if (Input.GetButtonDown("Fire1") && GameManager.instance.fallTutorialIntro.activeSelf)
		{
			GameManager.instance.fallTutorialIntro.SetActive(false);
		}
	}

	public void WinterTutorialScene()
	{
		if (cameraLerping)
		{
			LerpCamera(OffspringCameraPosition, TutorialCameraPosition, 1.5f);
		}
		else if (!winterIntro)
		{
			GameManager.instance.winterTutorialIntro.SetActive(true);
			winterIntro = true;
		}
		else if(Input.GetButtonDown("Fire1") && GameManager.instance.winterTutorialIntro.activeSelf)
		{
			GameManager.instance.winterTutorialIntro.SetActive(false);
		}
	}

	void setStartTime()
	{
		startTime = Time.time;
		cameraLerping = true;
	}
	
	void LerpCamera(Transform from, Transform to, float lerpSpeed)
	{
		CurrentCameraDummy.position = Vector3.Slerp(from.position, to.position, (Time.time - startTime) / lerpSpeed);
		CurrentCameraDummy.rotation = Quaternion.Slerp(from.rotation, to.rotation, (Time.time - startTime) /lerpSpeed);

		Camera.main.transform.position = CurrentCameraDummy.position;
		Camera.main.transform.rotation = CurrentCameraDummy.rotation;


		if (cameraLerping && Vector3.Distance(CurrentCameraDummy.position, to.position) < 0.1f)
		{
			cameraLerping = false;
		}
	}
	
}

