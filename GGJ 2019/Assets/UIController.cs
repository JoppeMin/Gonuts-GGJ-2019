using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private int duration = 1; // in seconds
    private float lerpTimer = 0;
    private float waitTimer = 0f;
    public Image screenOverlay;
	[SerializeField] private Color whiteOpaque;
	[SerializeField] private Color whiteTransparent;
	[SerializeField] private Color blackOpaque;
	[SerializeField] private Color blackTransparent;


	public enum UIState
    {
        NONE,
        GAME_TO_WHITE,
        WHITE_TO_GAME,
        GAME_TO_BLACK,
        BLACK_TO_GAME,
    }

    public UIState currentState = UIState.NONE;
    public bool enteringState = true;

    private void Awake()
    {
        screenOverlay = GameObject.Find("ScreenOverlay").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case UIState.NONE:
                break;
            case UIState.GAME_TO_WHITE:
				LerpColor(screenOverlay, whiteTransparent, whiteOpaque, UIState.WHITE_TO_GAME);
				break;
            case UIState.WHITE_TO_GAME:
				LerpColor(screenOverlay, whiteOpaque, whiteTransparent, UIState.NONE);
				break;
            case UIState.GAME_TO_BLACK:
                LerpColor(screenOverlay, blackTransparent, blackOpaque, UIState.BLACK_TO_GAME);
                break;
            case UIState.BLACK_TO_GAME:
                LerpColor(screenOverlay, blackOpaque, blackTransparent, UIState.NONE);
                break;

        }
        
    }


    private void LerpColor(Image screenFade, Color from, Color to, UIState state)
    {
        Color lerpedColor = Color.Lerp(from, to, lerpTimer);
        screenFade.color = lerpedColor;

        if (lerpTimer < 1)
        { 
            lerpTimer += Time.deltaTime / duration;
        }
        if (waitTimer < 3f)
        {
            waitTimer += Time.deltaTime / duration;
        }
        else
        {
            SetState(state);
        }
    }

    public void SetState(UIState state)
    {
        currentState = state;
        lerpTimer = 0f;
        waitTimer = 0f;
    } 

}
