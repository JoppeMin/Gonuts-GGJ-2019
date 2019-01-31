using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OffspringMenu : MonoBehaviour
{
    public string[] GuideDialog;
    public string[] saveNames;
    public GameObject[] OffspringNameObjects;
    InputField InputNames;
    TextMeshProUGUI GuideText;

    int CurrentNamingTarget = 0;

    void Start()
    {
        OffspringNameObjects = GameObject.FindGameObjectsWithTag("Names");
        InputNames = GameObject.Find("InputNames").GetComponent<InputField>();
        GuideText = GameObject.Find("GuideText").GetComponent<TextMeshProUGUI>();
        GuideText.text = GuideDialog[CurrentNamingTarget];

        InputNames.onEndEdit.AddListener(InputText);
    }

    public void InputText(string chosenName)
    {

        Debug.Log(chosenName);

        OffspringNameObjects[CurrentNamingTarget].GetComponent<TextMeshProUGUI>().text = chosenName;
        saveNames[CurrentNamingTarget] = chosenName;

        InputNames.text = "";
        InputNames.ActivateInputField();

        CurrentNamingTarget++;

        if (CurrentNamingTarget >= OffspringNameObjects.Length)
        {
            this.gameObject.transform.position = new Vector3 (10000,10000,0);
            GuideText.enabled = false;
            InputNames.enabled = false;
            GameManager.instance.EnterState(GameManager.GameState.TutorialFall);
        }
        else
        {
            InputNames.enabled = true;
            GuideText.enabled = true;
            GuideText.text = GuideDialog[CurrentNamingTarget];

        }


    }
}
