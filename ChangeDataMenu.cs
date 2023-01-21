using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using BackEnd;
using TMPro;


public class ChangeDataMenu : MonoBehaviour
{
    Canvas canvas;
    Vector3Int position;
    Color buttonColor = new Color(0.56f, 0.93f, 0.56f, 0.5f);
    int selectedButton = -1;
    public GameObject mainCamera;
    public TMP_Text bearQuestion;
    public TMP_InputField bearAnswer;
    public TMP_Text lynxQuestion;
    public TMP_InputField lynxAnswer;
    public TMP_Text voleQuestion;
    public TMP_InputField voleAnswer;
    public TMP_Text biomassQuestion;
    public TMP_InputField biomassAnswer;
    public TMP_Text humidityQuestion;
    public TMP_InputField humidityAnswer;
    public TMP_Text sunlightQuestion;
    public TMP_InputField sunlightAnswer;
    public Button waterButton;
    public Button desertButton;
    public Button plainButton;

    private void ClearAnswers()
    {
        bearAnswer.text = "";
        lynxAnswer.text = "";
        voleAnswer.text = "";
        biomassAnswer.text = "";
        humidityAnswer.text = "";
        sunlightAnswer.text = "";
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        waterButton.onClick.AddListener(() => buttonClicked(0));
        desertButton.onClick.AddListener(() => buttonClicked(1));
        plainButton.onClick.AddListener(() => buttonClicked(2));
    }

    void buttonClicked(int tile)
    {
        if (tile != selectedButton)
        {
            RunningBackEnd.tilemap.SetTile(position, tile);
            Button button = selectedButton switch
            {
                0 => waterButton,
                1 => desertButton,
                _ => plainButton,
            };
            button.GetComponent<Image>().color = Color.white;
            button = tile switch
            {
                0 => waterButton,
                1 => desertButton,
                _ => plainButton,
            };
            button.GetComponent<Image>().color = buttonColor;
            selectedButton = tile;
        }        
    }

    public void ToggleMenu(Vector3Int pos)
    {
        position = pos;
        canvas.enabled = true;
        int tile = RunningBackEnd.tilemap.GetTile(position);
        Button button = tile switch
        {
            0 => waterButton,
            1 => desertButton,
            _ => plainButton,
        };
        button.GetComponent<Image>().color = buttonColor;
        selectedButton = tile;
        bearQuestion.SetText("Bears : " + RunningBackEnd.tilemap.GetValue(position, "bear").ToString());
        lynxQuestion.SetText("Lynx : " + RunningBackEnd.tilemap.GetValue(position, "lynx").ToString());
        voleQuestion.SetText("Voles : " + RunningBackEnd.tilemap.GetValue(position, "vole").ToString());
        biomassQuestion.SetText("Biomass : " + RunningBackEnd.tilemap.GetValue(position,"biomass").ToString());
        humidityQuestion.SetText("Humidity : " + RunningBackEnd.tilemap.GetValue(position,"humidity").ToString());
        sunlightQuestion.SetText("Sunlight : " + RunningBackEnd.tilemap.GetValue(position,"sunlight").ToString());
    }

    public void CloseMenu()
    {
        ClearAnswers();
        Button button = selectedButton switch
        {
            0 => waterButton,
            1 => desertButton,
            _ => plainButton,
        };
        button.GetComponent<Image>().color = Color.white;
        canvas.enabled = false;
    }

    public void SubmitAnswers()
    {
        string getBear = bearAnswer.text;
        string getLynx = lynxAnswer.text;
        string getVole = voleAnswer.text;
        string getBiomass = biomassAnswer.text;
        string getHumidity = humidityAnswer.text;
        string getSunlight = sunlightAnswer.text;
        ClearAnswers();
        if (getBear != "")
        {
            int bear = int.Parse(getBear);
            bearQuestion.SetText("Bears : " + bear.ToString());
            RunningBackEnd.tilemap.SetValue(position,"bear", bear);
        }
        if (getLynx != "")
        {
            int lynx = int.Parse(getLynx);
            lynxQuestion.SetText("Lynx : " + lynx.ToString());
            RunningBackEnd.tilemap.SetValue(position,"lynx", lynx);
        }
        if (getVole != "")
        {
            int vole = int.Parse(getVole);
            voleQuestion.SetText("Voles : " + vole.ToString());
            RunningBackEnd.tilemap.SetValue(position,"vole", vole);
        }
        if (getBiomass != "")
        {
            float biomass = float.Parse(getBiomass);
            biomassQuestion.SetText("Biomass : " + biomass.ToString());
            RunningBackEnd.tilemap.SetValue(position,"biomass", biomass);
        }
        if (getHumidity != "")
        {
            float humidity = float.Parse(getHumidity);
            humidityQuestion.SetText("Humidity : " + humidity.ToString());
            RunningBackEnd.tilemap.SetValue(position,"humidity", humidity);
        }
        if (getSunlight != "")
        {
            float sunlight = float.Parse(getSunlight);
            sunlightQuestion.SetText("Sunlight : " + sunlight.ToString());
            RunningBackEnd.tilemap.SetValue(position,"sunlight", sunlight);
        }
    }

}
