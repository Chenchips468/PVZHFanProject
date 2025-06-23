using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject myZone;
    public GameObject opZone;
    public bool CanDeploy(GameObject card)
    {
        return true;
    }
    public List<GameObject> getOrderedAllMinions()
    {
        List<GameObject> retval = new List<GameObject>();
        if (GameManager.Instance.isZombies)
        {
            if (myZone.GetComponent<ZoneScript>().Minion != null) retval.Add(myZone.GetComponent<ZoneScript>().Minion);
            if (opZone.GetComponent<ZoneScript>().Minion != null) retval.Add(opZone.GetComponent<ZoneScript>().Minion);
        }
        else
        {
            if (opZone.GetComponent<ZoneScript>().Minion != null) retval.Add(opZone.GetComponent<ZoneScript>().Minion);
            if (myZone.GetComponent<ZoneScript>().Minion != null) retval.Add(myZone.GetComponent<ZoneScript>().Minion);
        }
        return retval;
    }
    public GameObject getMyMinion()
    {
        return myZone.GetComponent<ZoneScript>().getMinion();
    }
    public GameObject getOpMinion()
    {
        return opZone.GetComponent<ZoneScript>().getMinion();
    }
    public GameObject getMyZone()
    {
        return myZone;
    }
    public GameObject getOpZone()
    {
        return opZone;
    }
}
