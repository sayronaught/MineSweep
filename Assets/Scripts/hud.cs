using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hud : MonoBehaviour
{
    public gameController myCG;
    public TextMesh myTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myTxt.text = "M" + myCG.remainingMines + " S" + myCG.remainingSafe;
    }
}
