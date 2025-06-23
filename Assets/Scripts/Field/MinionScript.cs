using UnityEngine;
using System;
using TMPro;

public class MinionScript : MonoBehaviour
{
    private bool wasPressed = false;
    public Collider2D Collider;
    public GameObject CardPrefab;
    public GameObject Card;
    public string name;
    public bool isSelected = true;
    public int baseatk;
    public int curratk;
    public int basehp;
    public int currhp;
    GameObject hpt;
    GameObject atkt;
    void Start()
    {
        Card = Instantiate(CardPrefab);
        Debug.Log("Created");
        Card.transform.position = new Vector3(9999, 9999, 0);
        InitializeMinion();
    }
    void InitializeMinion()
    {
        CardScript cs = Card.GetComponent<CardScript>();
        baseatk = cs.BaseAttack;
        basehp = cs.BaseHealth;
        curratk = baseatk;
        currhp = basehp;
        atkt = transform.Find("UI/Atk/TMP").gameObject;
        Renderer ratk = atkt.GetComponent<Renderer>();
        ratk.sortingLayerName = "MinionLayer";   // Or your desired layer
        ratk.sortingOrder = 5;
        atkt.GetComponent<TextMeshPro>().text = curratk.ToString();
        hpt = transform.Find("UI/Hp/TMP").gameObject;
        hpt.GetComponent<TextMeshPro>().text = currhp.ToString();
        Renderer rhp = hpt.GetComponent<Renderer>();
        rhp.sortingLayerName = "MinionLayer";   // Or your desired layer
        rhp.sortingOrder = 5;
    }
    void HandleMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && OnCard() && GameManager.Instance.DisplayingInfo == false && canSelect())
        {
            isSelected = true;
        }
    }
    bool canSelect() {
        return GameManager.Instance.DisplayingInfo == false && GameManager.Instance.gameOver == false;
    }
    void HandleMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isSelected && OnCard())
            {
                DisplayCardInfos();
            }
            isSelected = false;
        }
    }
    void DisplayCardInfos()
    {
        if (!GameManager.Instance.DisplayingInfo) GameManager.Instance.DisplayInfo(GameManager.Instance.getOrderedAllMinionsCards(), Card);
    }
    bool OnCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Collider.OverlapPoint(mousePos);
    }
    void Update()
    {
        HandleMouseDown();
        HandleMouseUp();
    }
    public void takeDamage(int v)
    {
        currhp = Math.Max(0, currhp - v);
        updatehpttext();
        
    }
    public void updatehpttext()
    {
        hpt.GetComponent<TextMeshPro>().text = currhp.ToString();
        if (currhp < basehp)
        {
            hpt.GetComponent<TextMeshPro>().color = new Color(210f / 255f, 60f / 255f, 60f / 255f);
        }
    }
    public bool checkDeath()
    {
        return currhp <= 0;
    }
}
