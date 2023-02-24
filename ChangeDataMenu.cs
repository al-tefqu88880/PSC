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
    public TMP_Text temperatureQuestion;
    public TMP_InputField temperatureAnswer;
    public TMP_Text isothermalityQuestion;
    public TMP_InputField isothermalityAnswer;
    public TMP_Text summerTemperatureQuestion;
    public TMP_InputField summerTemperatureAnswer;
    public TMP_Text rainQuestion;
    public TMP_InputField rainAnswer;
    public TMP_Text rainVariationQuestion;
    public TMP_InputField rainVariationAnswer;
    public TMP_Text summerRainQuestion;
    public TMP_InputField summerRainAnswer;
    public Button waterButton;
    public Button desertButton;
    public Button plainButton;

    private void ClearAnswers()
    {
        rabbitAnswer.text = "";
        lynxAnswer.text = "";
        foxAnswer.text = "";
        temperatureAnswer.text = "";
        isothermalityAnswer.text = "";
        summerTemperatureAnswer.text = "";
        rainAnswer.text = "";
        rainVariationAnswer.text = "";
        summerRainAnswer.text = "";
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
        temperatureQuestion.SetText("Temperature : " + RunningBackEnd.tilemap.GetValue(position, "temperature").ToString());
        isothermalityQuestion.SetText("Isothermality : " + RunningBackEnd.tilemap.GetValue(position, "isothermality").ToString());
        summerTemperatureQuestion.SetText("Summer temperature : " + RunningBackEnd.tilemap.GetValue(position, "summerTemperature").ToString());
        rainQuestion.SetText("Precipitation : " + RunningBackEnd.tilemap.GetValue(position, "rain").ToString());
        rainVariationQuestion.SetText("Precipitation variation : " + RunningBackEnd.tilemap.GetValue(position,"rainVariation").ToString());
        summerRainQuestion.SetText("Summer precipitation : " + RunningBackEnd.tilemap.GetValue(position,"summerRain").ToString());
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
        string getTemperature = temperatureAnswer.text;
        string getIsothermality = isothermalityAnswer.text;
        string getSummerTemperature = summerTemperatureAnswer.text;
        string getRain = rainAnswer.text;
        string getRainVariation = rainVariationAnswer.text;
        string getSummerRain = summerRainAnswer.text;
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
        if (getTemperature != "")
        {
            float temperature = float.Parse(getTemperature);
            temperatureQuestion.SetText("Temperature : " + temperature.ToString());
            RunningBackEnd.tilemap.SetValue(position, "temperature", temperature);
        }
        if (getIsothermality != "")
        {
            float isothermality = float.Parse(getIsothermality);
            isothermalityQuestion.SetText("Isothermality : " + isothermality.ToString());
            RunningBackEnd.tilemap.SetValue(position, "isothermality", isothermality);
        }
        if (getSummerTemperature != "")
        {
            float summerTemperature = float.Parse(getSummerTemperature);
            summerTemperatureQuestion.SetText("Summer temperature : " + summerTemperature.ToString());
            RunningBackEnd.tilemap.SetValue(position, "summerTemperature", summerTemperature);
        }
        if (getRain != "")
        {
            float rain = float.Parse(getRain);
            rainQuestion.SetText("Precipitation : " + rain.ToString());
            RunningBackEnd.tilemap.SetValue(position, "rain", rain);
        }
        if (getRainVariation != "")
        {
            float rainVariation = float.Parse(getRainVariation);
            rainVariationQuestion.SetText("Precipitation variation : " + rainVariation.ToString());
            RunningBackEnd.tilemap.SetValue(position,"rainVariation", rainVariation);
        }
        if (getSummerRain != "")
        {
            float summerRain = float.Parse(getSummerRain);
            summerRainQuestion.SetText("Summer precipitation: " + summerRain.ToString());
            RunningBackEnd.tilemap.SetValue(position,"summerRain", summerRain);
        }
    }

}
