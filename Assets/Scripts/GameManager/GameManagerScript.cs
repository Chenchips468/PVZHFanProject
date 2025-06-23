using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isZombies = false;
    public bool isMultiplayer = false;
    private int phasenum = 1;
    private int turnnum = 1;
    public GameObject InfoArea;
    public bool DisplayingInfo = false;
    public GameObject MyHero;
    public GameObject Enemy;
    public GameObject currField;
    public List<GameObject> infoList;
    public void DisplayInfo(List<GameObject> cardList, GameObject currentCard = null)
    {
        infoList = new List<GameObject>();
        CardSwiper md = InfoArea.GetComponent<CardSwiper>();
        infoList.Clear();
        int currentCardNum = 0;
        for (int i = 0; i < cardList.Count; i++)
        {
            //GameObject card = Instantiate(Resources.Load<GameObject>("CardInfo"), transform);
            GameObject card = Resources.Load<GameObject>("CardInfoPage/CardInfo");
            infoList.Add(card);
            if (currentCard == cardList[i])
            {
                currentCardNum = i;
            }
        }
        //Debug.Log(currentCardNum);
        md.SetCards(infoList, cardList, currentCardNum);
    }
    public bool isChangingPhase = false;
    public bool canChangePhase(bool myClick)
    {
        if (gameOver) return false;
        if (isChangingPhase) return false;
        if (isZombies && (phasenum == 2)) return !myClick;
        else if (!isZombies && (phasenum == 1 || phasenum == 3)) return !myClick;
        return myClick;
    }
    public void NextPhase(TurnButtonScript button, bool myClick = true)
    {
        if (phasenum >= 4 || !canChangePhase(myClick)) return;
        isChangingPhase = true;
        StartCoroutine(NextPhaseCoroutine(button, myClick));
    }

    private IEnumerator NextPhaseCoroutine(TurnButtonScript button, bool myClick = true)
    {
        if (phasenum == 3)
        {
            phasenum = 4;
            button.ChangePhase(phasenum);
            yield return StartCoroutine(BattlePhaseCoroutine());
            button.ChangePhase(0);
            yield return new WaitForSeconds(1f); // Wait inside NextPhase
            phasenum = 1;
            NextTurn();
        }
        else
        {
            phasenum += 1;
        }
        button.ChangePhase(phasenum);
        isChangingPhase = false;
    }
    public int randomShieldIncrement()
    {
        return Random.Range(1, 4);
    }
    private IEnumerator BattlePhaseCoroutine()
    {
        Debug.Log("Battle Phase triggered.");
        for (int i = 0; i < 5; i++)
        {
            // Insert whatever per-step logic you want here
            Debug.Log($"lane {i + 1}");
            yield return new WaitForSeconds(0.25f);
            yield return currField.GetComponent<FieldScript>().LaneFightCoroutine(i);
            if (getMyHealth() == 0 || getOpHealth() == 0)
            {
                GameOver();
                yield return new WaitForSeconds(9999f);
            }
        }

        Debug.Log("Battle Phase complete.");
    }


    public void NextTurn()
    {
        turnnum++;
        MyHero.GetComponent<HeroScript>().DrawCard(getMyDeckScript());
        Enemy.GetComponent<EnemyScript>().DrawCard(getEnemyDeckScript());
        setMyDailyResource(getMyDailyResource() + 1);
        setOpDailyResource(getOpDailyResource() + 1);
        setMyResource(getMyDailyResource());
        setOpResource(getOpDailyResource());
    }
    public OpDeckScript getEnemyDeckScript()
    {
        return Enemy.GetComponent<EnemyScript>().getDeckScript();
    }
    public int getOpResource()
    {
        return Enemy.GetComponent<EnemyScript>().getResource();
    }
    public void setOpResource(int v)
    {
        Enemy.GetComponent<EnemyScript>().setResource(v);
    }
    public void setOpHealth(int v)
    {
        Enemy.GetComponent<EnemyScript>().setHealth(v);
    }
    public void setOpShield(int v)
    {
        Enemy.GetComponent<EnemyScript>().setShield(v);
    }
    public int getOpDailyResource()
    {
        //Debug.Log("kek");
        Enemy.GetComponent<EnemyScript>();
        //Debug.Log("hi");
        return Enemy.GetComponent<EnemyScript>().getDailyResource();
    }
    public void setOpDailyResource(int v)
    {
        Enemy.GetComponent<EnemyScript>().setDailyResource(v);
    }
    public int getOpHealth()
    {
        return Enemy.GetComponent<EnemyScript>().getHealth();
    }
    public int getOpShield()
    {
        return Enemy.GetComponent<EnemyScript>().getShield();
    }
    public HeroScript getMyHeroScript()
    {
        return MyHero.GetComponent<HeroScript>();
    }
    public int getMyHealth()
    {
        return MyHero.GetComponent<HeroScript>().getHealth();
    }
    public void setMyHealth(int v)
    {
        MyHero.GetComponent<HeroScript>().setHealth(v);
    }
    public int getMyShield()
    {
        return MyHero.GetComponent<HeroScript>().getShield();
    }
    public void setMyShield(int v)
    {
        MyHero.GetComponent<HeroScript>().setShield(v);
    }
    public int getMyResource()
    {
        return MyHero.GetComponent<HeroScript>().getResource();
    }
    public void setMyResource(int v)
    {
        MyHero.GetComponent<HeroScript>().setResource(v);
    }
    public int getMyDailyResource()
    {
        return MyHero.GetComponent<HeroScript>().getDailyResource();
    }
    public void setMyDailyResource(int v)
    {
        MyHero.GetComponent<HeroScript>().setDailyResource(v);
    }
    public int getPhase()
    {
        return phasenum;
    }
    public List<GameObject> getMyHand()
    {
        return MyHero.GetComponent<HeroScript>().getHand();
    }
    
    public DeckScript getMyDeckScript()
    {
        return MyHero.GetComponent<HeroScript>().getDeckScript();
    }
    public GameObject TopMyDeck()
    {
        return MyHero.GetComponent<HeroScript>().TopDeck();
    }
    public void MyCardPlayed(GameObject PlayedCard)
    {
        MyHero.GetComponent<HeroScript>().CardPlayed(PlayedCard);
    }
    public void OpCardPlayed(GameObject PlayedCard)
    {
        Enemy.GetComponent<EnemyScript>().CardPlayed(PlayedCard);
    }
    public bool CanDeploy(GameObject card, GameObject location, bool myCard = true)
    {
        if (myCard)
        {
            CardScript cs = card.GetComponent<CardScript>();
            if (cs.deploytype == "Zones")
            {
                if (isZombies && getPhase() != 1 && getPhase() != 3) return false;
                if (!isZombies && getPhase() != 2) return false;

                return location.GetComponent<ZoneScript>().CanDeploy(card, myCard) && cs.CanDeploy(location);
            }
            return false;
        }
        else
        {
            OpCardScript cs = card.GetComponent<OpCardScript>();
            bool AIisZombies = !isZombies;
            if (cs.deploytype == "Zones")
            {
                //Debug.Log("Here too:");
                //Debug.Log(card.name);
                if (AIisZombies && getPhase() != 1 && getPhase() != 3) return false;
                if (!AIisZombies && getPhase() != 2) return false;
                //Debug.Log("Made it bruh:");
                //Debug.Log(card.name);
                return location.GetComponent<ZoneScript>().CanDeploy(card, myCard) && cs.CanDeploy(location);
            }
            return false;
        }
    }
    public void Deploy(GameObject card, GameObject location, bool myCard = true)
    {
        if (myCard)
        {
            if (card.GetComponent<CardScript>().deploytype == "Zones")
            {
                location.GetComponent<ZoneScript>().Deploy(card.GetComponent<CardScript>().minionPrefab);
                MyCardPlayed(card);
                Destroy(card);
                setMyResource(getMyResource() - card.GetComponent<CardScript>().getCost());
            }
        }
        else
        {
            if (card.GetComponent<OpCardScript>().deploytype == "Zones")
            {
                //Debug.Log("hi");
                location.GetComponent<ZoneScript>().Deploy(card.GetComponent<OpCardScript>().minionPrefab);
                OpCardPlayed(card);
                Destroy(card);
                Debug.Log("Cost:"+card.GetComponent<OpCardScript>().getCost());
                setOpResource(getOpResource() - card.GetComponent<OpCardScript>().getCost());
            }
        }
    }
    public GameObject getField()
    {
        return currField;
    }
    public List<GameObject> getOrderedAllMinions()
    {
        return currField.GetComponent<FieldScript>().getOrderedAllMinions();
    }
    public List<GameObject> retval;
    public List<GameObject> getOrderedAllMinionsCards()
    {
        List<GameObject> minions = getOrderedAllMinions();
        retval = new List<GameObject>();
        for (int i = 0; i < minions.Count; i++)
        {
            retval.Add(minions[i].GetComponent<MinionScript>().Card);
        }
        return retval;
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: keep it across scenes
    }
    void Start()
    {
        List<string> myDeckNames = new List<string> { "Card","Card","Card","Card","Science Zombie Card","Science Zombie Card","Science Zombie Card","Science Zombie Card" };
        MyHero.GetComponent<HeroScript>().InitializeDeckCards(myDeckNames);
        for (int i = 0; i < 4; i++)
        {
            MyHero.GetComponent<HeroScript>().DrawCard(getMyDeckScript());
        }
        List<string> opDeckNames = new List<string> { "Card","Card","Card","Card","Science Zombie Card","Science Zombie Card","Science Zombie Card","Science Zombie Card" };
        Enemy.GetComponent<EnemyScript>().InitializeDeckCards(opDeckNames);
        for (int i = 0; i < 4; i++)
        {
            Enemy.GetComponent<EnemyScript>().DrawCard(getEnemyDeckScript());
        }
    }
    public bool gameOver = false;
    public void GameOver()
    {
        Debug.Log("Game Over!");
        //Time.timeScale = 0f;
        gameOver = true;
    }
}
