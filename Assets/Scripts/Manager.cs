using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public enum GameState
    {
        pong,
        freeroam,
        cutsceneA,
        cutsceneB,
        cutsceneC
    }
    public GameState state = GameState.pong;

    public List<string> introText;
    public List<string> cutsceneAText;
    public List<string> cutsceneBText;
    public List<string> cutsceneCText;

    public int introTextState = 0;

    public bool showtext = false;
    public bool textactive = false;

    public Text txt;
    public GameObject txtField;

    public static Manager instance;

    // Start is called before the first frame update
    void Start()
    {
        txtField = transform.Find("TextField").gameObject;
        txtField.SetActive(false);

        txt = txtField.transform.Find("Text").GetComponent<Text>();
        txt.text = introText[introTextState];

        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.pong){
            if (Input.anyKeyDown)
            {
                showtext = !showtext;
                if (!showtext)
                    if ((introTextState + 1) < introText.Count)
                    {
                        introTextState = introTextState + 1;
                        txt.text = introText[introTextState];
                    }
                    else
                        state = GameState.freeroam;
            }
        }
        if (state == GameState.cutsceneA)
        {
            if (!showtext)
            {
                showtext = true;
                introTextState = 0;
                txt.text = cutsceneAText[introTextState];
            }
            if (Input.anyKeyDown)
            {
                    if ((introTextState + 1) < cutsceneAText.Count)
                    {
                        introTextState = introTextState + 1;
                        txt.text = cutsceneAText[introTextState];
                    }
                else
                {
                    state = GameState.freeroam;
                    GameObject.Find("Ball").GetComponent<Ball>().trigger += 1;
                    showtext = false;
                }
                        
            }
        }
        if (state == GameState.cutsceneB)
        {
            if (!showtext)
            {
                showtext = true;
                introTextState = 0;
                txt.text = cutsceneBText[introTextState];
            }
            if (Input.anyKeyDown)
            {
                if ((introTextState + 1) < cutsceneAText.Count)
                {
                    introTextState = introTextState + 1;
                    txt.text = cutsceneBText[introTextState];
                }
                else
                {
                    state = GameState.freeroam;
                    GameObject.Find("Ball").GetComponent<Ball>().trigger += 1;
                    showtext = false;
                }

            }
        }
        if (state == GameState.cutsceneC)
        {
            if (!showtext)
            {
                showtext = true;
                introTextState = 0;
                txt.text = cutsceneAText[introTextState];
            }
            if (Input.anyKeyDown)
            {
                if ((introTextState + 1) < cutsceneCText.Count)
                {
                    introTextState = introTextState + 1;
                    txt.text = cutsceneCText[introTextState];
                }
                else
                {
                    state = GameState.freeroam;
                    GameObject.Find("Ball").GetComponent<Ball>().trigger += 1;
                    showtext = false;
                }

            }
        }

        UpdateText();
    }

    void UpdateText()
    {
        if (showtext && !textactive)
        {
            txtField.SetActive(true);
            textactive = true;
        }
        else if (!showtext && textactive)
        {
            txtField.SetActive(false);
            textactive = false;
        }
    }
}
