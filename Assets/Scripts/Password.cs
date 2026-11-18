using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Password : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text[] passwordDisplay;
    public GameObject displayBoard;
    public GameObject puzzle4;

    private XRSimpleInteractable[] buttons;
    private string entered = "";
    private bool completed;

    private void OnEnable()
    {
        entered = "";
        UpdateDisplay();
        buttons = GetComponentsInChildren<XRSimpleInteractable>();
        foreach (var button in buttons)
            button.selectEntered.AddListener(OnNumberSelected);
    }

    private void OnDisable()
    {
        if (buttons == null)
            return;

        foreach (var button in buttons)
            button.selectEntered.RemoveListener(OnNumberSelected);
    }

    private void OnNumberSelected(SelectEnterEventArgs args)
    {
        if (completed || !(args.interactableObject is XRSimpleInteractable button))
            return;

        var label = button.GetComponentInChildren<TMP_Text>();
        if (label == null)
            return;

        var digit = label.text.Trim();
        if (digit.Length != 1 || digit[0] < '0' || digit[0] > '9')
            return;

        if (entered.Length == 4)
            entered = "";

        entered += digit;
        UpdateDisplay();

        if (entered == "1130")
        {
            completed = true;
            gameManager.CompletePuzzle(2);
            if (puzzle4 != null)
                puzzle4.SetActive(true);
            displayBoard.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void UpdateDisplay()
    {
        for (var i = 0; i < passwordDisplay.Length; i++)
            passwordDisplay[i].text = i < entered.Length ? entered[i].ToString() : "";
    }
}
