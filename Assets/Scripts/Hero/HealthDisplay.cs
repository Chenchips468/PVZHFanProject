using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI hText = GetComponentInChildren<TextMeshProUGUI>();
        hText.text = GameManager.Instance.getMyHealth().ToString(); //+ "\nS: " +GameManager.Instance.getMyShield().ToString();
    }
}
