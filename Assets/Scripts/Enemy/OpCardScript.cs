using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpCardScript : MonoBehaviour
{
//Card Attributes
    public string CardName;
    public string CardDescription;
    public string CardFlavorText;
    [SerializeField]private int Cost;
    public bool IsMinion;
    public string deploytype;
    public GameObject minionPrefab;


//Other Variables
    public Collider2D Collider;
    public bool IsSelected;
    private Vector3 AnchorPosition;
    private Vector3 mouseDownPosition;
    private float clickThreshold = 0.1f; // how far the mouse can move before it's considered a drag
    public Vector2 boxSize = new Vector2(1f, 1f);
    public LayerMask layerToCheck;

    public void InitializeCard()
    {
        IsSelected = false;
        Collider = GetComponent<Collider2D>();
        AnchorPosition = transform.position;
        bool OpCardbackZombie = !GameManager.Instance.isZombies;
        if (OpCardbackZombie)
        {
            GameObject zcb = transform.Find("ZCB").gameObject;
            zcb.gameObject.SetActive(true);
            GameObject pcb = transform.Find("PCB").gameObject;
            pcb.gameObject.SetActive(false);
        }
        else
        {
            GameObject zcb = transform.Find("ZCB").gameObject;
            zcb.gameObject.SetActive(false);
            GameObject pcb = transform.Find("PCB").gameObject;
            pcb.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        InitializeCard();
    }

//CardMovement
    void Update()
    {
        //HandleMouseDown();
        //HandleMouseUp();
        //HandleLayerUpdate();
        if (IsSelected)
        {
            DragCard();
        }
    }
    void HandleLayerUpdate()
    {
        if (IsSelected == true)
        {
            transform.Find("Square").GetComponent<SpriteRenderer>().sortingLayerName = "SelectedLayer";
        }
        else
        {
            transform.Find("Square").GetComponent<SpriteRenderer>().sortingLayerName = "CardLayer";
        }
    }
    void HandleMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && OnCard() && GameManager.Instance.DisplayingInfo == false)
        {
            mouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            IsSelected = true;
        }
    }
    void HandleMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (IsSelected)
            {
                Vector3 mouseUpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                float distance = Vector2.Distance(mouseDownPosition, mouseUpPosition);
                if (distance < clickThreshold)
                {
                    //Debug.Log("Display Card Info");
                    DisplayCardInfos();
                }

                IsSelected = false;
                GameObject DeployLocation = CheckOverlap(CheckDistanceToNearestTarget());
                if (DeployLocation != null && GameManager.Instance.CanDeploy(gameObject,DeployLocation))
                {
                    GameManager.Instance.Deploy(gameObject, DeployLocation);
                    
                }
                else ReturnCard();
            }
        }
    }
    void DragCard()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it stays in 2D
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

//Deploying Card
    GameObject CheckOverlap(GameObject Place, string Target = "Zones")
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, LayerMask.GetMask(Target));
        foreach (var hit in hits)
        {
            if (Place == hit.gameObject)
            {
                //Debug.Log("Trying to deploy to: " + hit.gameObject.name);
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

        if (closestTarget != null)
        {
            //Debug.Log("Closest Target: " + closestTarget.name + ", Distance: " + closestDistance);
        }
        else
        {
            //Debug.Log("No target detected nearby.");
        }
        return closestTarget;
    }
    public bool CanDeploy(GameObject Location)
    {
        Debug.Log("R Here:");
        Debug.Log(Cost);
        return GameManager.Instance.getOpResource() - Cost >= 0;
    }
    public int getCost()
    {
        return Cost;
    }

//Displaying Card Info
    void DisplayCardInfos()
    {
        //if (!GameManager.Instance.DisplayingInfo) GameManager.Instance.DisplayInfo(GameManager.Instance.getOpHand(),gameObject);
    }

}
