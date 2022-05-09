using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{

    public Material matUnknown;
    public Material matMine;
    public Material matSafe;
    public Material[] matSafeNr;
    public Material[] matSimpleSwitch;

    public Transform currentRoom;

    public int remainingMines;
    public int remainingSafe;

    public void triggeredMine()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        countRoom();
    }
    public void countRoom()
    {
        mine[] neighborTiles = currentRoom.GetComponentsInChildren<mine>();
        if (neighborTiles.Length > 0)
        {
            foreach (mine tile in neighborTiles)
            {
                if (tile.isMine) remainingMines++;
                else remainingSafe++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ExecuteAfterTime(0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
