using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGameState : MonoBehaviour
{
    [Tooltip("Input desired state to switch to")]
    [SerializeField]
    private GameState state;

    [Tooltip("Not required if button is on the same gameobject")]
    [SerializeField]
    private Button button;

    private void Start()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(delegate { GameManager.instance.UpdateGameState(state); }) ;
    }
}
