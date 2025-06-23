using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public int num_minions;
    public int num_my;
    public int num_op;
    public GameObject l1;
    public GameObject l2;
    public GameObject l3;
    public GameObject l4;
    public GameObject l5;
    public List<GameObject> AllLanes;
    public List<GameObject> allMinionsOrdered()
    {
        return new List<GameObject>();
    }
    public List<GameObject> getOrderedAllMinions()
    {
        List<GameObject> retval = new List<GameObject>();
        AllLanes = new List<GameObject> { l1, l2, l3, l4, l5 };
        for (int i = 0; i < AllLanes.Count; i++)
        {
            retval.AddRange(AllLanes[i].GetComponent<LaneScript>().getOrderedAllMinions());
        }
        return retval;
    }
    public List<GameObject> getAllLanes()
    {
        AllLanes = new List<GameObject> { l1, l2, l3, l4, l5 };
        return AllLanes;
    }
    /*
    public void LaneFight(int lane)
    {
        StartCoroutine(LaneFightCoroutine(lane));
    }
    */
    public IEnumerator LaneFightCoroutine(int lane)
    {
        AllLanes = new List<GameObject> { l1, l2, l3, l4, l5 };
        GameObject MyMinion = AllLanes[lane].GetComponent<LaneScript>().getMyMinion();
        GameObject OpMinion = AllLanes[lane].GetComponent<LaneScript>().getOpMinion();
        int myMinionDmg = 0;
        int opMinionDmg = 0;

        if (MyMinion == null && OpMinion == null) yield break;

        if (MyMinion == null)
        {
            opMinionDmg = OpMinion.GetComponent<MinionScript>().curratk;
            int shieldadd = GameManager.Instance.randomShieldIncrement();
            GameManager.Instance.setMyShield(GameManager.Instance.getMyShield() + shieldadd);

            if (GameManager.Instance.getMyShield() >= 8)
            {
                yield return StartCoroutine(BlockCoroutine());
                GameManager.Instance.setMyShield(0);
            }
            else
            {

                GameManager.Instance.setMyHealth(GameManager.Instance.getMyHealth() - opMinionDmg);
                yield return new WaitForSeconds(0.75f);
            }
            yield break;
        }

        if (OpMinion == null)
        {
            myMinionDmg = MyMinion.GetComponent<MinionScript>().curratk;
            int shieldadd = GameManager.Instance.randomShieldIncrement();
            GameManager.Instance.setOpShield(GameManager.Instance.getOpShield() + shieldadd);

            if (GameManager.Instance.getOpShield() >= 8)
            {
                yield return StartCoroutine(BlockCoroutine());
                GameManager.Instance.setOpShield(0);
            }
            else
            {

                GameManager.Instance.setOpHealth(GameManager.Instance.getOpHealth() - myMinionDmg);
                yield return new WaitForSeconds(0.75f);
            }
            yield break;
        }

        myMinionDmg = MyMinion.GetComponent<MinionScript>().curratk;
        opMinionDmg = OpMinion.GetComponent<MinionScript>().curratk;

        if (!GameManager.Instance.isZombies)
        {
            MyMinion.GetComponent<MinionScript>().takeDamage(opMinionDmg);
            OpMinion.GetComponent<MinionScript>().takeDamage(myMinionDmg);
        }
        else
        {
            OpMinion.GetComponent<MinionScript>().takeDamage(myMinionDmg);
            MyMinion.GetComponent<MinionScript>().takeDamage(opMinionDmg);
        }

        if (MyMinion.GetComponent<MinionScript>().checkDeath())
        {
            Destroy(MyMinion);
            AllLanes[lane].GetComponent<LaneScript>().getMyZone().GetComponent<ZoneScript>().isFilled = false;
        }

        if (OpMinion.GetComponent<MinionScript>().checkDeath())
        {
            Destroy(OpMinion);
            AllLanes[lane].GetComponent<LaneScript>().getOpZone().GetComponent<ZoneScript>().isFilled = false;
        }
        yield return new WaitForSeconds(0.75f);
        yield return null;
    }

    private IEnumerator BlockCoroutine()
    {
        yield return new WaitForSeconds(1f);
    }

}
