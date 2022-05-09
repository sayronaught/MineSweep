using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class player : MonoBehaviour
{
    public Transform xrHeadPosition;
    public gameController myGC;
    public Transform playerStandingOn;
    public Vector3 playerOrientation = Vector3.up;
    public float whenMovedDelay = 0.5f;

    public hud myHUD;

    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;
    private float updateControllerTimer = 0f;
    private float delayMove = 0f;

    private GameObject objectFindAt(Vector3 pos)
    {
        var cols = Physics.OverlapSphere(pos, 0.1f);
        var dist = Mathf.Infinity;
        GameObject nearest = null;
        foreach (Collider col in cols)
        {
            var d = Vector3.Distance(pos, col.transform.position);
            if (d < dist)
            { // if closer...
                dist = d; // save its distance... 
                nearest = col.gameObject; // and its gameObject
            }
        }
        return nearest;
 }
    private void updateController()
    {
        if (Application.isEditor) return;
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        leftController = leftHandDevices[0];
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        rightController = rightHandDevices[0];
    }

    private bool testWalkable( Transform testWalkableLocation )
    {
        if (testWalkableLocation.gameObject.GetComponent<mine>()) return true;
        if (testWalkableLocation.tag == "WalkAbleBlock") return true;
        return false;
    }
    private void testMine( Transform testMineLocation )
    {
        mine mineScript = testMineLocation.gameObject.GetComponent<mine>();
        if (!mineScript) return;
        if (!mineScript.isMine) return;
        myGC.triggeredMine();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = playerStandingOn.position + playerOrientation*0.5f;
        //transform.RotateAround(transform.position, transform.right, -90f);
    }

    // Update is called once per frame
    void Update()
    {
        delayMove -= Time.deltaTime;
        if (delayMove > 0f) return;
        bool goUp = false;
        leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out goUp);
        bool goDown = false;
        leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out goDown);
        bool goLeft = false;
        leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out goLeft);
        bool goRight = false;
        leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out goRight);
        bool turnLeft = false;
        rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out turnLeft);
        bool turnRight = false;
        rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out turnRight);
        if ( goUp )
        {
            var direction = transform.forward;
            direction = Snapping.Snap(direction, new Vector3(1f, 1f, 1f), SnapAxis.All);
            /*if (playerOrientation == Vector3.up || playerOrientation == Vector3.down)
                direction = new Vector3(direction.x, 0f, direction.z);
            if (direction == Vector3.zero) return; // not looking up or down*/
            GameObject inFront = objectFindAt(playerStandingOn.position + direction);
            GameObject stepUp = objectFindAt(playerStandingOn.position + direction + playerOrientation);
            delayMove = whenMovedDelay;
            //transform.Rotate(transform.up * 1f);
            if ( stepUp && testWalkable(stepUp.transform) )
            {
                transform.RotateAround(transform.position, transform.right, -90f);
                playerOrientation = transform.up;
                playerStandingOn = stepUp.transform;
                transform.position = playerStandingOn.position + playerOrientation*0.5f;
                testMine(playerStandingOn);
                return;
            }
            if ( inFront && testWalkable(inFront.transform))
            {
                playerStandingOn = inFront.transform;
                transform.position = playerStandingOn.position + playerOrientation*0.5f;
                testMine(playerStandingOn);
                return;
            }
        }
        if (goDown)
        {
            var direction = transform.forward*-1f;
            direction = Snapping.Snap(direction, new Vector3(1f, 1f, 1f), SnapAxis.All);
            GameObject inFront = objectFindAt(playerStandingOn.position + direction);
            GameObject stepUp = objectFindAt(playerStandingOn.position + direction + playerOrientation);
            delayMove = whenMovedDelay;
            if (stepUp)
            {
                return;
            }
            if (inFront && testWalkable(inFront.transform))
            {
                playerStandingOn = inFront.transform;
                transform.position = playerStandingOn.position + playerOrientation * 0.5f;
                testMine(playerStandingOn);
                return;
            }
        }
        if (goLeft)
        {
            var direction = transform.right*-1f;
            direction = Snapping.Snap(direction, new Vector3(1f, 1f, 1f), SnapAxis.All);
            GameObject inFront = objectFindAt(playerStandingOn.position + direction);
            GameObject stepUp = objectFindAt(playerStandingOn.position + direction + playerOrientation);
            delayMove = whenMovedDelay;
            if (stepUp)
            {
                return;
            }
            if (inFront && testWalkable(inFront.transform))
            {
                playerStandingOn = inFront.transform;
                transform.position = playerStandingOn.position + playerOrientation * 0.5f;
                testMine(playerStandingOn);
                return;
            }
        }
        if (goRight)
        {
            var direction = transform.right;
            direction = Snapping.Snap(direction, new Vector3(1f, 1f, 1f), SnapAxis.All);
            GameObject inFront = objectFindAt(playerStandingOn.position + direction);
            GameObject stepUp = objectFindAt(playerStandingOn.position + direction + playerOrientation);
            delayMove = whenMovedDelay;
            if (stepUp)
            {
                return;
            }
            if (inFront && testWalkable(inFront.transform))
            {
                playerStandingOn = inFront.transform;
                transform.position = playerStandingOn.position + playerOrientation * 0.5f;
                testMine(playerStandingOn);
                return;
            }
        }
        if ( turnLeft )
        {
            transform.RotateAround(transform.position, transform.up, -90f);
            delayMove = whenMovedDelay;
            return;
        }
        if (turnRight)
        {
            transform.RotateAround(transform.position, transform.up, 90f);
            delayMove = whenMovedDelay;
            return;
        }
        updateControllerTimer -= Time.deltaTime;
        if (!(updateControllerTimer < 0f)) return;
        updateController();
        updateControllerTimer = 2f;
    }
}
