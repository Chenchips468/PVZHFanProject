using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public string Type;
    public GameObject GameStateObject;
    public GameStateScript GameStateObjectScript;
    public List<GameObject> CardList;
    public int NumCards;
    public GameObject HandObject;
    public HandScript HandObjectScript;
    public GameObject HeroObject;
    public HeroScript HeroObjectScript;
    public void InitializeDeck(
        GameObject GameStateObject = null,
        List<string> DeckListNames = null,
        GameObject HandObject = null,
        GameObject HeroObject = null
    ){
        this.Type = "Deck";
        this.GameStateObject = GameStateObject;
        if(this.GameStateObject != null) this.GameStateObjectScript = this.GameStateObject.GetComponent<GameStateScript>();
        InitializeDeckCards(DeckListNames);
        NumCards = this.CardList.Count;
        this.HandObject = HandObject;
        if(HandObject != null) HandObjectScript = HandObject.GetComponent<HandScript>();
        this.HeroObject = HeroObject;
        if(HeroObject != null) HeroObjectScript = HeroObject.GetComponent<HeroScript>();
    }
    public void InitializeDeckCards(List<string> DeckListNames){
        if(DeckListNames == null) return;
        for(int i = 0; i < DeckListNames.Count; i++){
            GameObject CardPrefab = Resources.Load<GameObject>(DeckListNames[i]);
            GameObject NewCard = Instantiate(CardPrefab);
            CardList.Add(NewCard);
            NewCard.transform.position = new Vector3(9999,9999,0);
        }
    }
    public int CardsLeft(){
        return NumCards;
    }
    public GameObject CardDrawn(){
        if(CardsLeft() == 0) return null;
        ShuffleDeck();
        GameObject DrawnCard = CardList[0];
        CardList.RemoveAt(0);
        NumCards--;
        return DrawnCard;
    }
    public void AddCard(GameObject NewCard){
        CardList.Add(NewCard);
        NumCards++;
    }
    public void ShuffleDeck(){
        Shuffle<GameObject>(CardList);
    }
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    void Start(){
        List<string> DeckListNames;
        DeckListNames = new List<string> { "PeashooterCard", "PeashooterCard", "PeashooterCard", "PeashooterCard", "PeashooterCard", "PeashooterCard", "PeashooterCard", "PeashooterCard","PeashooterCard", "PeashooterCard","PeashooterCard", "PeashooterCard"};
        InitializeDeck(DeckListNames: DeckListNames);
    }
}
