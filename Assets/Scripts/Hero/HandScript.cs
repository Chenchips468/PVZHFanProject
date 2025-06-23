using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public List<GameObject> CardList;
    int NumCards;
    public void DrawCard(DeckScript Deck)
    {
        GameObject NewCard = Deck.TopDeck();
        //Debug.Log(NewCard);
        if (NewCard == null) return;
        AddCard(NewCard);
    }
    public void AddCard(GameObject NewCard){
        CardList.Add(NewCard);
        NumCards++;
        ReorganizeHand();
    }
    public void CardPlayed(GameObject PlayedCard){
        RemoveCardFromHand(PlayedCard);
        ReorganizeHand();
    }
    public void RemoveCardFromHand(GameObject RemovedCard)
    {
        //Debug.Log(NumCards);
        for (int i = 0; i < NumCards; i++)
        {
            //Debug.Log(i);
            if (CardList[i] == RemovedCard)
            {
                CardList.RemoveAt(i);
                break;
            }
        }
        NumCards--;
    }
    public void ReorganizeHand()
    {
        float HorizontalCardSpacing = 1.08f;
        float VerticalCardSpacing = 0.38f;
        int Count = NumCards;
        if (Count == 0) return;
        if (Count <= 4)
        {
            float TotalWidth = (Count - 1) * HorizontalCardSpacing;
            float StartX = -TotalWidth / 2f;
            for (int CardIndex = 0; CardIndex < Count; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(StartX + (CardIndex * HorizontalCardSpacing), transform.position[1], 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition);
            }
        }
        else
        {
            int Top = Count / 2;
            int Bot = Count / 2;
            if (Count % 2 == 1)
            {
                Top++;
            }
            float TopWidth = (Top - 1) * HorizontalCardSpacing;
            float BotWidth = (Bot - 1) * HorizontalCardSpacing;
            float TopStartX = -TopWidth / 2f;
            float BotStartX = -BotWidth / 2f;
            int TopInd = 0;
            for (int CardIndex = 0; CardIndex < Top; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(TopStartX + (TopInd++ * HorizontalCardSpacing), transform.position[1] + VerticalCardSpacing, 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition);
            }
            int BotInd = 0;
            for (int CardIndex = Top; CardIndex < Count; CardIndex++)
            {
                Vector3 NewCardPosition = new Vector3(BotStartX + (BotInd++ * HorizontalCardSpacing), transform.position[1] - VerticalCardSpacing, 0);
                CardScript CurrentCard = CardList[CardIndex].GetComponent<CardScript>();
                CurrentCard.ChangeAnchor(NewCardPosition);
            }
        }
    }
}