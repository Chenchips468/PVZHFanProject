using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroScript : MonoBehaviour
{
    public string Type;
    public GameObject GameStateObject;
    public GameStateScript GameStateObjectScript;
    public string Name;
    public int Health;
    public int MaxHealth;
    public int Resource;
    public int ResourcePerTurn;
    public List<string> HeroClasses;
    public int BlockMeter;
    public int MaxBlockMeter;
    public GameObject HandObject;
    public HandScript HandObjectScript;
    public GameObject DeckObject;
    public DeckScript DeckObjectScript;
    public List<GameObject> Supers;
    public void InitializeHero(
        GameObject GameStateObject = null,
        string HeroName  = "",
        int Health = 20,
        int MaxHealth = 20, 
        int Resource = 0,
        int ResourcePerTurn = 1,
        List<string> HeroClasses = null,
        int BlockMeter = 0, 
        int MaxBlockMeter = 10,
        GameObject HandObject = null, 
        GameObject DeckObject = null, 
        List<GameObject> Supers = null
    ){
        this.Type = "Hero";
        this.GameStateObject = GameStateObject;
        if(this.GameStateObject != null) this.GameStateObjectScript = this.GameStateObject.GetComponent<GameStateScript>();
        this.Health = Health;
        this.MaxHealth = MaxHealth;
        this.Resource = Resource;
        this.ResourcePerTurn = ResourcePerTurn;
        this.HeroClasses = HeroClasses;
        this.BlockMeter = BlockMeter;
        this.MaxBlockMeter = MaxBlockMeter;
        this.HandObject = HandObject;
        if(this.HandObject != null) this.HandObjectScript = this.HandObject.GetComponent<HandScript>();
        this.DeckObject = DeckObject;
        if(this.DeckObject != null) this.DeckObjectScript = this.DeckObject.GetComponent<DeckScript>();
        this.Supers = Supers;
    }
    public int BlockRNG(int MaxBlockGained = 3){
        return UnityEngine.Random.Range(1,MaxBlockGained+1);
    }
    public void GainBlock(int AddedBlock = 0){
        BlockMeter+=AddedBlock;
        BlockMeter=Math.Min(BlockMeter,MaxBlockMeter);
    }
    public void BlockedAttack(){
        BlockMeter = 0;
        GainSuper();
    }
    public GameObject GainSuper(){
        Shuffle<GameObject>(Supers);
        GameObject GainedSuper = Supers[0];
        Supers.RemoveAt(0);
        return GainedSuper;
    }
    public bool AttackGetsBlocked(){
        return BlockMeter == MaxBlockMeter;
    }
    public void TakeDamage(int AttackDamage = 0, int BlockGained = 0){
        GainBlock(BlockGained);
        if(AttackGetsBlocked()){
            BlockedAttack();
        }
        else{
            Health-=AttackDamage;
            Health = Math.Max(Health,0);
        }
    }
    public bool HeroDeath(){
        return Health == 0;
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
