using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardScript : MonoBehaviour
{
    // Card Attributes
    public string CardName;
    public string CardDescription;
    public string CardFlavorText;
    public int Cost;
    public int BaseHealth;
    public int BaseAttack;
    public Texture2D minionSprite;
    public List<string> Tribes;
    public string ClassShortened;
    public bool IsMinion = true;
    public string deploytype;
    public GameObject minionPrefab;
    public string set;
    public string rarity;
    public bool isZombie;
    public int x, y;
    public float scale;
    public GameObject hpt;
    public GameObject atkt;
    public GameObject res;

    // Other Variables
    public Collider2D Collider;
    public bool IsSelected = false;
    private Vector3 AnchorPosition;
    private Vector3 mouseDownPosition;
    private float clickThreshold = 0.1f; // how far the mouse can move before it's considered a drag
    public Vector2 boxSize = new Vector2(1f, 1f);
    public LayerMask layerToCheck;

    private bool isDragging = false;

    public void InitializeCard()
    {
        AnchorPosition = transform.position;
        Collider = GetComponent<Collider2D>();

        atkt = transform.Find("UICard/Atk/TMP").gameObject;
        Renderer ratk = atkt.GetComponent<Renderer>();
        ratk.sortingLayerName = "CardLayer";   // Or your desired layer
        ratk.sortingOrder = 5;
        atkt.GetComponent<TextMeshPro>().text = BaseAttack.ToString();
        hpt = transform.Find("UICard/Hp/TMP").gameObject;
        hpt.GetComponent<TextMeshPro>().text = BaseHealth.ToString();
        Renderer rhp = hpt.GetComponent<Renderer>();
        rhp.sortingLayerName = "CardLayer";   // Or your desired layer
        rhp.sortingOrder = 5;
        res = transform.Find("UICard/Res/TMP").gameObject;
        res.GetComponent<TextMeshPro>().text = Cost.ToString();
        Renderer rres = res.GetComponent<Renderer>();
        rres.sortingLayerName = "CardLayer";   // Or your desired layer
        rres.sortingOrder = 5;
    }

    void Start()
    {
        InitializeCard();
    }

    // CardMovement
    void Update()
    {
        HandleMouseDown();
        HandleMouseUp();
        HandleLayerUpdate();

        if (IsSelected)
        {
            if (!isDragging)
            {
                Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePos.z = 0;

                float dragDistance = Vector2.Distance(mouseDownPosition, currentMousePos);
                if (dragDistance > clickThreshold)
                {
                    isDragging = true;
                }
            }

            if (isDragging)
            {
                DragCard();
            }
        }
    }
    bool test = true;
    public GameObject sm;
    void HandleLayerUpdate()
    {
        string layerName = isDragging ? "SelectedLayer" : "CardLayer";

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            sr.sortingLayerName = layerName;
        }
        Renderer ratk = atkt.GetComponent<Renderer>();
        ratk.sortingLayerName = layerName;
        Renderer rhp = hpt.GetComponent<Renderer>();
        rhp.sortingLayerName = layerName;
        Renderer rres = res.GetComponent<Renderer>();
        rres.sortingLayerName = layerName;
        SpriteMask spriteMask = GetComponentInChildren<SpriteMask>();
        spriteMask.frontSortingLayerID = SortingLayer.NameToID(layerName);
        spriteMask.frontSortingOrder = 100;
        spriteMask.backSortingLayerID = SortingLayer.NameToID(layerName);
        spriteMask.backSortingOrder = -1;
    }

    void HandleMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && OnCard() && canSelect())
        {
            mouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseDownPosition.z = 0;
            IsSelected = true;
            isDragging = false;
        }
    }

    bool canSelect()
    {
        if (GameManager.Instance == null) return true;
        return GameManager.Instance.DisplayingInfo == false && GameManager.Instance.gameOver == false;
    }

    void HandleMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (IsSelected)
            {
                Vector3 mouseUpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseUpPosition.z = 0;

                float distance = Vector2.Distance(mouseDownPosition, mouseUpPosition);
                if (distance < clickThreshold)
                {
                    // It's a click, not a drag
                    DisplayCardInfos();
                }

                IsSelected = false;
                isDragging = false;

                GameObject DeployLocation = CheckOverlap(CheckDistanceToNearestTarget());
                if (DeployLocation != null && GameManager.Instance.CanDeploy(gameObject, DeployLocation))
                {
                    GameManager.Instance.Deploy(gameObject, DeployLocation);
                }
                else
                {
                    ReturnCard();
                }
            }
        }
    }

    void DragCard()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it stays in 2D
        mousePos.y += 0.3f;
        transform.position = mousePos;
    }

    void ReturnCard()
    {
        transform.position = AnchorPosition;
    }

    public void ChangeAnchor(Vector3 newPosition)
    {
        AnchorPosition = newPosition;
        transform.position = AnchorPosition;
    }

    bool OnCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Collider.OverlapPoint(mousePos);
    }

    // Deploying Card
    GameObject CheckOverlap(GameObject Place, string Target = "Zones")
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, LayerMask.GetMask(Target));
        foreach (var hit in hits)
        {
            if (Place == hit.gameObject)
            {
                return Place;
            }
        }
        return null;
    }

    GameObject CheckDistanceToNearestTarget(string Target = "Zones")
    {
        Collider2D[] laneColliders = Physics2D.OverlapCircleAll(transform.position, 100f, LayerMask.GetMask(Target));
        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;

        foreach (var laneCollider in laneColliders)
        {
            float distance = Vector2.Distance(transform.position, laneCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = laneCollider.gameObject;
            }
        }
        return closestTarget;
    }

    public bool CanDeploy(GameObject Location)
    {
        Debug.Log(Cost);
        return GameManager.Instance.getMyResource() - Cost >= 0;
    }

    public int getCost()
    {
        return Cost;
    }

    // Displaying Card Info
    void DisplayCardInfos()
    {
        if (!GameManager.Instance.DisplayingInfo)
            GameManager.Instance.DisplayInfo(GameManager.Instance.getMyHand(), gameObject);
    }
}
