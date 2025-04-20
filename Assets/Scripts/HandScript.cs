using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public string Type;
    public GameObject GameStateObject;
    public GameStateScript GameStateObjectScript;
    public List<GameObject> CardList;
    public int NumCards;
    public GameObject DeckObject;
    public DeckScript DeckObjectScript;
    public GameObject HeroObject;
    public HeroScript HeroObjectScript;
    public void InitializeHand(
        GameObject GameStateObject = null,
        List<string> HandListNames = null,
        GameObject DeckObject = null,
        GameObject HeroObject = null
    ){
        this.Type = "Hand";
        this.GameStateObject = GameStateObject;
        if(this.GameStateObject != null) this.GameStateObjectScript = this.GameStateObject.GetComponent<GameStateScript>();
        Debug.Log("hi");
        InitializeHandCards(HandListNames);
        this.NumCards = this.CardList.Count;
        this.DeckObject = DeckObject;
        if(DeckObject != null) DeckObjectScript = DeckObject.GetComponent<DeckScript>();
        this.HeroObject = HeroObject;
        if(HeroObject != null) HeroObjectScript = HeroObject.GetComponent<HeroScript>();
    }
    public void InitializeHandCards(List<string> HandListNames){
        if(HandListNames == null) return;
        for(int i = 0; i < HandListNames.Count; i++){
            GameObject CardPrefab = Resources.Load<GameObject>(HandListNames[i]);
            GameObject NewCard = Instantiate(CardPrefab);
            CardList.Add(NewCard);
            NewCard.transform.position = new Vector3(9999,9999,0);
        }
    }
    public int CardsLeft(){
        return NumCards;
    } 
    public void DrawMultCards(int NumCards){
        for(int i = 0; i < NumCards; i++){
            DrawCard();
        }
    }
    public void DrawCard(){
        GameObject NewCard = DeckObjectScript.CardDrawn();
        Debug.Log(NewCard);
        if(NewCard == null) return;
        AddCard(NewCard);
    }
    public void AddCard(GameObject NewCard){
        CardList.Add(NewCard);
        NumCards++;
        ReorganizeHand();
    }
    public void ReorganizeHand(){
        float HorizontalCardSpacing = 1.1f;
        float VerticalCardSpacing = 0.4f;
        int Count = NumCards;
        if (Count == 0) return;
        if(Count <= 4){
            float TotalWidth = (Count - 1) * HorizontalCardSpacing;
            float StartX = -TotalWidth / 2f;
            for (int CardIndex = 0; CardIndex < Count; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(StartX + (CardIndex * HorizontalCardSpacing), transform.position[1], 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition); 
            }
        }
        else{
            int Top = Count/2;
            int Bot = Count/2;
            if(Count%2 == 1){
                Top++;
            }
            float TopWidth = (Top - 1) * HorizontalCardSpacing;
            float BotWidth = (Bot - 1) * HorizontalCardSpacing;
            float TopStartX = -TopWidth / 2f;
            float BotStartX = -BotWidth / 2f;
            int TopInd = 0;
            for (int CardIndex = 0; CardIndex < Top; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(TopStartX + (TopInd++ * HorizontalCardSpacing), transform.position[1]+VerticalCardSpacing, 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition); 
            }
            int BotInd = 0;
            for (int CardIndex = Top; CardIndex < Count; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(BotStartX + (BotInd++ * HorizontalCardSpacing), transform.position[1]-VerticalCardSpacing, 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition); 
            }
        }
    }
    public void CardPlayed(GameObject PlayedCard){
        RemoveCardFromHand(PlayedCard);
        ReorganizeHand();
    }
    public void RemoveCardFromHand(GameObject RemovedCard){
        for(int i = 0; i < NumCards; i++){
            if(CardList[i] == RemovedCard){
                CardList.RemoveAt(i);
            }
        }
    }
    public void testDraw(){
        if (Input.GetKeyDown(KeyCode.C)){
            DrawCard();
            Debug.Log(CardList.Count);
        }
    }
    void Start(){
        InitializeHand(DeckObject: GameObject.Find("DeckObject"));
    }
    void Update(){
        testDraw();
    }
}
