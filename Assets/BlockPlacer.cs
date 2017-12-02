//https://pastebin.com/140bv3kW

using UnityEngine;
using System.Collections;

public class BlockPlacer : MonoBehaviour
{

    //Handles # of blocks the user has
    public bool unlimitedBlocks = false;
    public int blocksLeft = 0;

    //If the player wants to lock the cursor somewhere else instead, they can do that.
    public bool lockCursor = true;

    //static reference to the script. This is so blocks can be added via a static method.
    static BlockPlacer obj;

    //The block prefab to instantiate
    public GameObject blockPrefab;

    private ArrayList gameObjects = new ArrayList();
    private ArrayList blkToRemove = new ArrayList();

    //Maximum range that the player can place a block from
    public float range = 7f;

    void Start()
    {
        //This assigns the static reference
        BlockPlacer.obj = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Locks the cursor. Running this in update is only done because of weird unity editor shenanigans with cursor locking
        if (Input.GetKeyDown(KeyCode.C))
        {

            if (!lockCursor) {
                /// ERRORS? Are you getting errors on the following line of code?
                /// If so, you're probably using Unity 4 or earlier.
                /// To fix this, add // to the start of the following line
                Cursor.lockState = CursorLockMode.Locked;
                lockCursor = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                lockCursor = false;
            }

        }
        /// And remove // from the start of the next line of code
        //Cursor.lockCursor = true;

        //Place blocks using the LMB
        if (Input.GetMouseButtonDown(0) && lockCursor)
        {
            //Make a ray and raycasthit for a raycast
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            RaycastHit hit;

            //Perform the raycast
            if (Physics.Raycast(ray, out hit, range))
            {
                //The raycast is backed up so that placing works and won't place blocks inside of the ground.
                //After testing, 0.2 units back had the best result
                Vector3 backup = ray.GetPoint(hit.distance - 0.2f);

                //Round the placement so they place like blocks should
                Vector3 placeAt = new Vector3(
                    Mathf.RoundToInt(backup.x), 
                    Mathf.RoundToInt(backup.y), 
                    Mathf.RoundToInt(backup.z));

                //Instantiate the block and save it so that we can do other stuff with it later
                GameObject block = (GameObject)GameObject.Instantiate(blockPrefab, placeAt, Quaternion.Euler(Vector3.zero));
                gameObjects.Add(block);
                
            }
        }

        else if (Input.GetMouseButtonDown(1) && lockCursor)
        {            
            //Make a ray and raycasthit for a raycast
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            RaycastHit hit;

            //Perform the raycast
            if (Physics.Raycast(ray, out hit, range))
            {
                //The raycast is backed up so that placing works and won't place blocks inside of the ground.
                //After testing, 0.2 units back had the best result
                Vector3 backup = ray.GetPoint(hit.distance + 0.2f);

                //Round the placement so they place like blocks should
                Vector3 removeAt = new Vector3(
                    Mathf.RoundToInt(backup.x),
                    Mathf.RoundToInt(backup.y),
                    Mathf.RoundToInt(backup.z));

                int i = 0;

                foreach (GameObject blk in gameObjects)
                {
                    if (removeAt == blk.transform.position)
                    {
                        print("Destroyed Block");
                        blkToRemove.Add(blk);
                        break;
                    }
                    i++;
                }
                if (blkToRemove.Count == 1)
                {
                    gameObjects.RemoveAt(i);
                    foreach (GameObject dblk in blkToRemove)
                    {
                        Destroy(dblk);
                    }
                    blkToRemove = new ArrayList();
                }
            }
        }
    }

    void OnGUI()
    {
        //This is the crosshair
        GUI.Box(new Rect(Screen.width / 2 - 5, Screen.height / 2 - 5, 5, 5), "");
    }
}