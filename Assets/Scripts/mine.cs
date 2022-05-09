using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class mine : MonoBehaviour
{

    public gameController myGC;

    public bool isMine = false;
    public bool isMarked = false;
    public int percentage = 0;

    private XRSimpleInteractable mySimple;

    public void shootLeft()
    {
        if (mySimple.selectingInteractor.transform.name == "RightHand Controller")
            markMine();
        if (mySimple.selectingInteractor.transform.name == "LeftHand Controller")
            markSafe();
    }
    public void shootRight()
    {
        if (mySimple.selectingInteractor.transform.name == "RightHand Controller")
            markMine();
        if (mySimple.selectingInteractor.transform.name == "LeftHand Controller")
            markSafe();
    }
    public void markMine()
    {
        GetComponent<Renderer>().material = myGC.matMine;
        isMarked = true;
    }
    public void markSafe()
    {
        if (isMine)
        {
            GetComponent<Renderer>().material = myGC.matMine;
            myGC.remainingMines--;
            isMarked = true;
            myGC.triggeredMine();
            return;
        }
        GetComponent<Renderer>().material = myGC.matSafe;
        mine[] neighborTiles = transform.parent.GetComponentsInChildren<mine>();
        int count = 0;
        if ( neighborTiles.Length > 0 )
        {
            foreach( mine tile in neighborTiles)
            {
                if (tile.transform.position.x >= transform.position.x -1f
                    && tile.transform.position.x <= transform.position.x + 1f
                    && tile.transform.position.y >= transform.position.y - 1f
                    && tile.transform.position.y <= transform.position.y + 1f
                    && tile.transform.position.z >= transform.position.z - 1f
                    && tile.transform.position.z <= transform.position.z + 1f)
                {
                    if (tile.isMine) count++;
                }  
            }
        }
        GetComponent<Renderer>().material = myGC.matSafeNr[count];
        myGC.remainingSafe--;
        isMarked = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if ( percentage > 0 )
        {
            if (percentage <= Random.RandomRange(0, 100))
                isMine = true;
        }
        GetComponent<Renderer>().material = myGC.matUnknown;
        mySimple = GetComponent<XRSimpleInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
