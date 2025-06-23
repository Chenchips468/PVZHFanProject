using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<GameObject> CardList = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        CardList.Add(Resources.Load<GameObject>("CardInfo"));
    }
}
