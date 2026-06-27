using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderTxt;
    public static float SliderVal;


    private void Start()
    {
        _slider.onValueChanged.AddListener((v) =>
        {

            _sliderTxt.text = v.ToString("0");
        });

        
    }

    private void Update()
    {
        SliderVal = _slider.value;
        Debug.Log(_slider.value);
    }


    

















    public void Scenario2()
    {
        SceneManager.LoadScene(2);
    }
}
