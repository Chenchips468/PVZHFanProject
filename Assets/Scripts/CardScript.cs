using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public string Type;
    public GameObject GameStateObject;
    public GameStateScript GameStateObjectScript;
    public string Name;
    public int Cost;
    public string CardType;
    public List<string> CardTribes;
    public GameObject DeckObject;
    public DeckScript DeckObjectScript;
    public GameObject HandObject;
    public HandScript HandObjectScript;
    public GameObject HeroObject;
    public HeroScript HeroObjectScript;
    public GameObject DeployedMinion;
    public Vector3 AnchorPosition;
    public bool IsSelected;
    public Collider2D Collider;
    public void InitializeCard(
        GameObject GameStateObject = null,
        string Name = "",
        int Cost = 0,
        string CardType = "",
        List<string> CardTribes = null,
        GameObject DeckObject = null,
        GameObject HandObject = null,
        GameObject HeroObject = null,
        Vector3 AnchorPosition = default,
        GameObject DeployedMinion = null
    ){
        this.Type = "Card";
        this.GameStateObject = GameStateObject;
        if(this.GameStateObject != null) this.GameStateObjectScript = this.GameStateObject.GetComponent<GameStateScript>();
        this.Name = Name;
        this.Cost = Cost;
        this.CardType = CardType;
        this.CardTribes = CardTribes;
        this.DeckObject = DeckObject;
        if(DeckObject != null) DeckObjectScript = DeckObject.GetComponent<DeckScript>();
        this.HandObject = HandObject;
        if(HandObject != null) HandObjectScript = HandObject.GetComponent<HandScript>();
        this.HeroObject = HeroObject;
        if(HeroObject != null) HeroObjectScript = HeroObject.GetComponent<HeroScript>();
        this.DeployedMinion = DeployedMinion;
        this.AnchorPosition = AnchorPosition;
        IsSelected = false;
        Collider = gameObject.GetComponent<Collider2D>();
    }

    void CardChosen(){
        if(Input.GetMouseButtonDown(0)){
            if(OnCard()){
                IsSelected = true;
            }
        }
    }
    bool OnCard(){
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Collider.OverlapPoint(MousePos))
        {
            return true;
        }
        return false;
    }
    void DragCard(){
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = MousePos;
    }
    void ReleaseCard(){
        if(Input.GetMouseButtonUp(0)){
            IsSelected = false;
            ReturnCard();
        }
    }

    void ReturnCard(){
        transform.position = AnchorPosition;
    }
    public void ChangeAnchor(Vector3 NewPosition){
        AnchorPosition = NewPosition;
        transform.position = AnchorPosition;
    }
    bool CanDeploy(GameObject Lane, int Position){
        return false;
    }
    bool CanDeploy(GameObject Entity){
        return false;
    }
    void Deploy(GameObject Lane, int Position){

    }
    void Deploy(GameObject Entity){
        
    }
    void Update(){
        CardChosen();
        ReleaseCard();
        if(IsSelected)
        {
            DragCard();
        }
    }
    void Start(){
        InitializeCard(AnchorPosition: transform.position);
    }
}
