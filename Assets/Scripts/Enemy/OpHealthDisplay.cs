using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OpHealthDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI hText = GetComponentInChildren<TextMeshProUGUI>();
        hText.text =  GameManager.Instance.getOpHealth().ToString();// + "\nS: " +GameManager.Instance.getOpShield().ToString();
    }
}
