using UnityEngine;

public class TutorialUIClickHandler : MonoBehaviour
{
    private System.Action onClick;

    public void Setup(System.Action onClickAction)
    {
        onClick = onClickAction;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClick?.Invoke();
        }
    }
}
