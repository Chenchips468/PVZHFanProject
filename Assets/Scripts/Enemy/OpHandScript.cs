using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpHandScript : MonoBehaviour
{
    public List<GameObject> CardList;
    int NumCards;
    public void DrawCard(OpDeckScript Deck)
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
    float HorizontalCardSpacing = 0.3f;
    int Count = NumCards;
    if (Count == 0) return;
    float TotalWidth = (Count - 1) * HorizontalCardSpacing;
    float StartX = transform.position.x -TotalWidth / 2f;

    for (int CardIndex = 0; CardIndex < Count; CardIndex++)
    {
        Vector3 NewCardPosition = new Vector3(StartX + (CardIndex * HorizontalCardSpacing), transform.position.y, 0);
        GameObject card = CardList[CardIndex];

        // Move the card
        OpCardScript currentCard = card.GetComponent<OpCardScript>();
        currentCard.ChangeAnchor(NewCardPosition);

        // Set sorting order on all SpriteRenderers (including children)
        SpriteRenderer[] renderers = card.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = Count - CardIndex;
        }
    }
}


}