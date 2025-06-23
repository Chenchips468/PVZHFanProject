using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReDisplay : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject sun;
    public GameObject brain;
    public GameObject sun;
    void Start()
    {
        sun = transform.Find("Sun").gameObject;
        sun.gameObject.SetActive(false);
        brain = transform.Find("Brain").gameObject;
        brain.gameObject.SetActive(false);
        if (GameManager.Instance.isZombies)
        {
            brain.gameObject.SetActive(true);
        }
        else
        {
            sun.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI hText = GetComponentInChildren<TextMeshProUGUI>();
        hText.text = GameManager.Instance.getMyResource().ToString();
    }
}
