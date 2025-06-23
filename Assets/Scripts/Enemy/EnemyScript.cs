using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsBot = true;
    public GameObject button;
    public int health = 20;
    public int shield = 0;
    public int resource = 1;
    public int dailyresource = 1;
    public GameObject hand;
    private List<string> myDeckNames;
    public GameObject deck;
    private bool botHasActedThisPhase = false;
    public void setHealth(int v)
    {
        health = Math.Max(v,0);
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
        return dailyresource;
    }
    public void setDailyResource(int v)
    {
        dailyresource = v;
    }
    public void DrawCard(DeckScript deck)
    {
        return;
    }
    public List<GameObject> getHand()
    {
        return hand.GetComponent<HandScript>().CardList;
    }
    public OpHandScript getHandScript()
    {
        return hand.GetComponent<OpHandScript>();
    }
     public OpDeckScript getDeckScript()
    {
        return deck.GetComponent<OpDeckScript>();
    }
    
    public GameObject TopDeck()
    {
        GameObject Card = deck.GetComponent<OpDeckScript>().TopDeck();
        return Card;
    }
    public void CardPlayed(GameObject PlayedCard){
        hand.GetComponent<OpHandScript>().CardPlayed(PlayedCard);
    }
    public void DrawCard(OpDeckScript deck)
    {
        getHandScript().DrawCard(deck);
    }
    public void InitializeDeckCards(List<string> DeckListNames)
    {
        getDeckScript().InitializeDeckCards(DeckListNames);
    }
    void botTurn()
    {
        bool botisZombies = !GameManager.Instance.isZombies;
        if (botisZombies && (GameManager.Instance.getPhase() == 1 || GameManager.Instance.getPhase() == 3))
        {
            //Debug.Log("Bot turn: " + GameManager.Instance.getPhase());
            if (!botHasActedThisPhase)
            {
                botHasActedThisPhase = true;

                if (GameManager.Instance.getPhase() == 1)
                    StartCoroutine(botPlacementAndEndTurn());
                else
                    StartCoroutine(botTurnEndCoroutine(button.GetComponent<TurnButtonScript>()));
            }
        }
        else if (!botisZombies && GameManager.Instance.getPhase() == 2)
        {
            //Debug.Log("Bot turn: " + GameManager.Instance.getPhase());
            if (!botHasActedThisPhase)
            {
                botHasActedThisPhase = true;

                if (GameManager.Instance.getPhase() == 2)
                    StartCoroutine(botPlacementAndEndTurn());
                else
                    StartCoroutine(botTurnEndCoroutine(button.GetComponent<TurnButtonScript>()));
            }
        }
        else
        {
            botHasActedThisPhase = false;
        }
    }

private IEnumerator botPlacementAndEndTurn()
{
    yield return new WaitForSeconds(1f);
    yield return StartCoroutine(botPlaceRandomCoroutine()); // Wait for placement to finish
    yield return StartCoroutine(botTurnEndCoroutine(button.GetComponent<TurnButtonScript>())); // Then end turn
}

private IEnumerator botTurnEndCoroutine(TurnButtonScript button)
{
    yield return new WaitForSeconds(1f);
    GameManager.Instance.NextPhase(button.GetComponent<TurnButtonScript>(), false);
}

private IEnumerator botPlaceRandomCoroutine()
{
    int placedone = UnityEngine.Random.Range(1, 4);
    int numplaced = 0;
        while (true)
        {
            List<GameObject> availabletargets = new List<GameObject>(getHandScript().CardList);
            List<GameObject> allLocations = new List<GameObject>(GameManager.Instance.getField().GetComponent<FieldScript>().getAllLanes());

            Shuffle<GameObject>(availabletargets);
            Shuffle<GameObject>(allLocations);
            bool hasplace = false;
            for (int i = 0; i < allLocations.Count; i++)
            {
                if (hasplace == true)
                    break;

                Debug.Log("Searching available lane...");
                GameObject opLocation = allLocations[i].GetComponent<LaneScript>().opZone;

                for (int j = 0; j < availabletargets.Count; j++)
                {
                    //Debug.Log(availabletargets[j].name+ " "+ GameManager.Instance.CanDeploy(availabletargets[j], opLocation, false));
                    if (GameManager.Instance.CanDeploy(availabletargets[j], opLocation, false))
                    {
                        GameManager.Instance.Deploy(availabletargets[j], opLocation, false);
                        numplaced++;
                        hasplace = true;
                        yield return new WaitForSeconds(1f); // Wait after each placement
                        break;
                    }
                }
            }
            if (numplaced == placedone || hasplace == false)
            {
                break;
            }
        }
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

    // Update is called once per frame
    void Update()
    {
        botTurn();
    }
}
