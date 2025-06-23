using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public GameObject Minion = null;
    public bool isFilled = false;
    public bool myZone;
    public GameObject lane;
    public bool CanDeploy(GameObject card, bool myCard)
    {
        return lane.GetComponent<LaneScript>().CanDeploy(card) && !isFilled && !(myCard ^ myZone);
    }
    public void Deploy(GameObject minionPrefab)
    {
        Minion = Instantiate(minionPrefab, transform);
        Minion.transform.position = transform.position;
        isFilled = true;
    }
    public GameObject getMinion()
    {
        return Minion;
    }
}
