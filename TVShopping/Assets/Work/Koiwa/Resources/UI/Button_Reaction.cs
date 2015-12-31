using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Button_Reaction : MonoBehaviour
{
    public bool isClick = false;
    ScenarioSetter _scenario;

    public void OnClick()
    {
        isClick = true;

        if (this.name == "Button(0)")
        {
            _scenario.SetRoute = ScenarioSetter.Route.A;
        }

        if (this.name == "Button(1)")
        {
            _scenario.SetRoute = ScenarioSetter.Route.B;
        }

        if (this.name == "Button(2)")
        {
            _scenario.SetRoute = ScenarioSetter.Route.C;
        }
    }

    void Start()
    {
        _scenario = GameObject.FindObjectOfType<ScenarioSetter>();
    }
}
