using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int dailyresource = 1;
    private int health = 20;
    private int shield = 0;
    private int resource = 1;
    public GameObject hand;
    private List<string> myDeckNames;
    public GameObject deck;
    public void setHealth(int v)
    {
        health = Math.Max(v, 0);
    }
    public int getHealth()
    {
        return health;
    }
    public int getShield()
    {
        return shield;
    }
    public void setShield(int v)
    {
        shield = Math.Min(8, v);
    }
    public int getResource()
    {
        return resource;
    }
    public void setResource(int v)
    {
        resource = v;
    }
    public int getDailyResource()
    {
        //Debug.Log("bruh");
        return dailyresource;
    }
    public void setDailyResource(int v)
    {
        Debug.Log(v);
        dailyresource = v;
    }
    public List<GameObject> getHand()
    {
        return hand.GetComponent<HandScript>().CardList;
    }
    public HandScript getHandScript()
    {
        return hand.GetComponent<HandScript>();
    }
     public DeckScript getDeckScript()
    {
        return deck.GetComponent<DeckScript>();
    }
    
    public GameObject TopDeck()
    {
        GameObject Card = deck.GetComponent<DeckScript>().TopDeck();
        return Card;
    }
    public void CardPlayed(GameObject PlayedCard){
        hand.GetComponent<HandScript>().CardPlayed(PlayedCard);
    }
    public void DrawCard(DeckScript deck)
    {
        getHandScript().DrawCard(deck);
    }
    public void InitializeDeckCards(List<string> DeckListNames)
    {
        getDeckScript().InitializeDeckCards(DeckListNames);
    }
}
