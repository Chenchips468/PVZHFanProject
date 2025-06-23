using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TurnButtonScript : MonoBehaviour
{
    public void OnMyButtonClick()
    {
        Debug.Log("Next Phase");
        GameManager.Instance.NextPhase(this);
        // Add your logic here
    }
    public void Start()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Phase "+GameManager.Instance.getPhase().ToString();
    }
    public void ChangePhase(int np)
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Phase "+np.ToString();
    }
}
