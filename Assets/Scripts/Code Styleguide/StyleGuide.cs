using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleGuide : MonoBehaviour
{
    //private variable, but exposed to the editor through "SerializeField"
    [SerializeField]
    private string styleName;

    //public variable, but not available to be changed from other scripts, only-read
    public string publicName { get; private set; }

    //Event to be called when the style changes
    //It passes a "string" variable when invoked, meaning that any function that you subscribe to it
    //has to accept a string as input and can therefore use this variable on excecution
    //you can pass any variable type through this.
    public static event Action<string> onStyleChange;

    //You dont have to pass a variable through your events, simply leave out the "<variable>"
    public static event Action onStyleChangeNoArgs;

    //Assigns the Example funtion to be called when the event is invoked
    //and then invokes the event, passing the variable "styleName" as its argument.
    private void Start()
    {
        onStyleChange += ExampleFunction;
        onStyleChange?.Invoke(styleName);
    }

    //Example function that is subscribed to the event
    //it would log the given styleName in the console when onStyleChange is invoked.
    private void ExampleFunction(string styleName)
    {
        Debug.Log(styleName);
    }
}
