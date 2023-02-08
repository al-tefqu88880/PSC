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
    public TMP_Text rabbitQuestion;
    public TMP_InputField rabbitAnswer;
    public TMP_Text lynxQuestion;
    public TMP_InputField lynxAnswer;
    public TMP_Text foxQuestion;
    public TMP_InputField foxAnswer;
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
        rabbitAnswer.text = "";
        lynxAnswer.text = "";
        foxAnswer.text = "";
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
        rabbitQuestion.SetText("Rabbits : " + RunningBackEnd.tilemap.GetValue(position, "rabbit").ToString());
        lynxQuestion.SetText("Lynx : " + RunningBackEnd.tilemap.GetValue(position, "lynx").ToString());
        foxQuestion.SetText("Foxes : " + RunningBackEnd.tilemap.GetValue(position, "fox").ToString());
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
        string getRabbit = rabbitAnswer.text;
        string getLynx = lynxAnswer.text;
        string getFox = foxAnswer.text;
        string getBiomass = biomassAnswer.text;
        string getHumidity = humidityAnswer.text;
        string getSunlight = sunlightAnswer.text;
        ClearAnswers();
        if (getRabbit != "")
        {
            int rabbit = int.Parse(getRabbit);
            rabbitQuestion.SetText("Rabbits : " + rabbit.ToString());
            RunningBackEnd.tilemap.SetValue(position,"rabbit", rabbit);
        }
        if (getLynx != "")
        {
            int lynx = int.Parse(getLynx);
            lynxQuestion.SetText("Lynx : " + lynx.ToString());
            RunningBackEnd.tilemap.SetValue(position,"lynx", lynx);
        }
        if (getFox != "")
        {
            int fox = int.Parse(getFox);
            foxQuestion.SetText("Foxes : " + fox.ToString());
            RunningBackEnd.tilemap.SetValue(position,"fox", fox);
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
