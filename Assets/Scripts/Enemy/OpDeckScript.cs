using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpDeckScript : MonoBehaviour
{
    public List<GameObject> CardList;
    public int NumCards;
    public void InitializeDeckCards(List<string> DeckListNames)
    {
        if (DeckListNames == null) return;
        for (int i = 0; i < DeckListNames.Count; i++)
        {
            GameObject CardPrefab = Resources.Load<GameObject>("Card/"+DeckListNames[i] + " - Op");
            GameObject NewCard = Instantiate(CardPrefab);
            AddCard(NewCard);
            NewCard.GetComponent<OpCardScript>().InitializeCard();
            NewCard.transform.position = new Vector3(9999, 9999, 0);
        }
    }
    public int CardsLeft(){
        return NumCards;
    }
    public GameObject TopDeck(){
        if(CardsLeft() == 0) return null;
        ShuffleDeck();
        GameObject DrawnCard = CardList[0];
        CardList.RemoveAt(0);
        NumCards--;
        return DrawnCard;
    }
    public void AddCard(GameObject NewCard){
        CardList.Add(NewCard);
        NewCard.transform.position = new Vector3(9999,9999,0);
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
}