using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchMechanic : MonoBehaviour
{

    public GameObject focusLight;
    public gameController myGC;
    public GameObject onOffObject;

    public bool isOn = false;
    private Renderer myRend;

    public void hoverOn()
    {
        focusLight.SetActive(true);
        myRend.material = myGC.matSimpleSwitch[2];
    }
    public void hoverOff()
    {
        focusLight.SetActive(false);
        if ( isOn) myRend.material = myGC.matSimpleSwitch[0];
        else myRend.material = myGC.matSimpleSwitch[1];
    }
    public void flipSwitch()
    {
        if ( isOn)
        { // is on
            onOffObject.SetActive(false);
            myRend.material = myGC.matSimpleSwitch[1];
        } else { // is off
            onOffObject.SetActive(true);
            myRend.material = myGC.matSimpleSwitch[0];
        }
        isOn = !isOn;
    }

    // Start is called before the first frame update
    void Start()
    {
        myRend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
