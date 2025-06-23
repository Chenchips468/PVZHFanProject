using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfoPageScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cmbot;
    public GameObject rrbot;
    public GameObject bricon;
    public GameObject smicon;
    public GameObject sun;
    public GameObject brain;

    void Start()
    {
        
        
    }
    void InitLogos()
    {
        cmbot = transform.Find("Image/RarityPack/CommonBot").gameObject;
        cmbot.gameObject.SetActive(false);
        rrbot = transform.Find("Image/RarityPack/RareBot").gameObject;
        rrbot.gameObject.SetActive(false);

        bricon = transform.Find("Image/Class/BrainyIcon").gameObject;
        bricon.gameObject.SetActive(false);
        smicon = transform.Find("Image/Class/SmartyIcon").gameObject;
        smicon.gameObject.SetActive(false);

        sun = transform.Find("Image/Resource/Sun").gameObject;
        sun.gameObject.SetActive(false);
        brain = transform.Find("Image/Resource/Brain").gameObject;
        brain.gameObject.SetActive(false);
    }
    public GameObject Card;
    // Update is called once per frame
    public void writeinfo(GameObject card)
    {
        InitLogos();
        Card = card;
        CardScript cs = card.GetComponent<CardScript>();
        string name = cs.CardName;
        string ability = cs.CardDescription;
        string flavortext = cs.CardFlavorText;
        List<string> tribes = cs.Tribes;
        string tribecombined = "";
        bool isZombie = cs.isZombie;
        for (int i = 0; i < tribes.Count; i++)
        {
            tribecombined += tribes[i] + " ";
        }
        Texture2D sprite = cs.minionSprite;
        int atk = cs.BaseAttack;
        int hp = cs.BaseHealth;
        int cost = cs.Cost;
        string myclass = cs.ClassShortened;
        string set = cs.set;
        string rarity = cs.rarity;

        GameObject head = transform.Find("Image/Head").gameObject;
        TextMeshProUGUI hText = head.GetComponentInChildren<TextMeshProUGUI>();
        hText.text = name;
        GameObject body = transform.Find("Image/Body").gameObject;
        TextMeshProUGUI bText = body.GetComponentInChildren<TextMeshProUGUI>();
        bText.text = ability;
        GameObject flavor = transform.Find("Image/FlavorText").gameObject;
        TextMeshProUGUI fText = flavor.GetComponentInChildren<TextMeshProUGUI>();
        fText.text = flavortext;
        GameObject mtribes = transform.Find("Image/Tribes").gameObject;
        TextMeshProUGUI tText = mtribes.GetComponentInChildren<TextMeshProUGUI>();
        tText.text = tribecombined;
        GameObject mclass = transform.Find("Image/Class").gameObject;
        GameObject mfr = transform.Find("Image/CardFrame").gameObject;
        RawImage mfrimage = mfr.GetComponent<RawImage>();
        if (myclass == "BRAINY")
        {
            bricon.gameObject.SetActive(true);
            mfrimage.color = new Color(255f / 255f, 85f / 255f, 240f / 255f);

        }
        else if (myclass == "SMARTY")
        {
            smicon.gameObject.SetActive(true);
            mfrimage.color = Color.white;
        }
        else
        {
            smicon.gameObject.SetActive(true);
            mfrimage.color = Color.white;
        }
        
        GameObject mSprite = transform.Find("Image/Sprite").gameObject;
        RawImage rimage = mSprite.GetComponent<RawImage>();
        rimage.texture = sprite;
        RectTransform rt = mSprite.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(cs.x, cs.y); 
        rt.localScale = new Vector3(cs.scale, cs.scale, cs.scale);
        GameObject mres = transform.Find("Image/Resource").gameObject;
        TextMeshProUGUI rText = mres.GetComponentInChildren<TextMeshProUGUI>();
        rText.text = cost.ToString();
        if (isZombie == true)
        {
            brain.gameObject.SetActive(true);
        }
        else
        {
            sun.gameObject.SetActive(true);
        }
        GameObject matk = transform.Find("Image/Atk").gameObject;
        TextMeshProUGUI aText = matk.GetComponentInChildren<TextMeshProUGUI>();
        aText.text = atk.ToString();
        GameObject mhp = transform.Find("Image/Hp").gameObject;
        TextMeshProUGUI hpText = mhp.GetComponentInChildren<TextMeshProUGUI>();
        hpText.text = hp.ToString();
        GameObject mbot = transform.Find("Image/RarityPack").gameObject;
        TextMeshProUGUI botText = mbot.GetComponentInChildren<TextMeshProUGUI>();
        botText.text = "<b><cspace=-1>" + set + " - " + rarity;
        if (rarity == "COMMON")
        {
            Debug.Log("Peasant");
            cmbot.gameObject.SetActive(true);
            botText.color = Color.white;
        }
        else if (rarity == "RARE")
        {
            rrbot.gameObject.SetActive(true);
            botText.color = new Color(255f / 255f, 245f / 255f, 165f / 255f);
        }
        else
        {
            cmbot.gameObject.SetActive(true);
        }

    }
    void Update()
    {
        
    }
}
