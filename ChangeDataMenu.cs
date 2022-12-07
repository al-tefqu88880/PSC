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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) & !canvas.enabled)
        {
            canvas.enabled = true;
            position = GridCoordinates.MouseAtTile(mainCamera);
            int tile = RunningBackEnd.tilemap.GetTile(position);
            Button button = tile switch
            {
                0 => waterButton,
                1 => desertButton,
                _ => plainButton,
            };
            button.GetComponent<Image>().color = buttonColor;
            selectedButton = tile;
            bearQuestion.SetText("Bears : " + RunningBackEnd.tilemap.GetBear(position).ToString());
            lynxQuestion.SetText("Lynx : " + RunningBackEnd.tilemap.GetLynx(position).ToString());
            voleQuestion.SetText("Voles : " + RunningBackEnd.tilemap.GetVole(position).ToString());
            biomassQuestion.SetText("Biomass : " + RunningBackEnd.tilemap.GetBiomass(position).ToString());
            humidityQuestion.SetText("Humidity : " + RunningBackEnd.tilemap.GetHumidity(position).ToString());
            sunlightQuestion.SetText("Sunlight : " + RunningBackEnd.tilemap.GetSunlight(position).ToString());
        }

        if (Input.GetKeyDown(KeyCode.Escape) & canvas.enabled)
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

        if (Input.GetKeyDown(KeyCode.Return) & canvas.enabled)
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
                RunningBackEnd.tilemap.SetBear(position, bear);
            }
            if (getLynx != "")
            {
                int lynx = int.Parse(getLynx);
                lynxQuestion.SetText("Lynx : " + lynx.ToString());
                RunningBackEnd.tilemap.SetLynx(position, lynx);
            }
            if (getVole != "")
            {
                int vole = int.Parse(getVole);
                voleQuestion.SetText("Voles : " + vole.ToString());
                RunningBackEnd.tilemap.SetVole(position, vole);
            }
            if (getBiomass != "")
            {
                float biomass=float.Parse(getBiomass);
                biomassQuestion.SetText("Biomass : " + biomass.ToString());
                RunningBackEnd.tilemap.SetBiomass(position, biomass);
            }
            if (getHumidity != "")
            {
                float humidity = float.Parse(getHumidity);
                humidityQuestion.SetText("Humidity : " + humidity.ToString());
                RunningBackEnd.tilemap.SetHumidity(position, humidity);
            }
            if (getSunlight!= "")
            {
                float sunlight = float.Parse(getSunlight);
                sunlightQuestion.SetText("Sunlight : " + sunlight.ToString());
                RunningBackEnd.tilemap.SetSunlight(position, sunlight);
            }
        }
    }
}
