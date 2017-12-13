//https://pastebin.com/140bv3kW

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;


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
    private bool fill = false;
    private bool secondFill = false;
    private string blockMaterial = "Steel";
    private string blockShape = "Cube";
    private string blockDirection = "";
    private string blockSize = "";

    private ArrayList gameObjects = new ArrayList();
    private ArrayList saveObjects = new ArrayList();
    private ArrayList blkToRemove = new ArrayList();

    //Undo Tool
    private ArrayList gameObjectsUndo = new ArrayList();
    private ArrayList saveObjectsUndo = new ArrayList();
    private int a;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            blockPrefab = Resources.Load("Prefabs/"+blockMaterial+blockShape+blockDirection, typeof(GameObject)) as GameObject;
        }
        //Locks the cursor. Running this in update is only done because of weird unity editor shenanigans with cursor locking
        if (Input.GetKeyDown(KeyCode.C))
        {

            if (!lockCursor) {
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
                
                if (!fill)
                {
                    CreateBlock(placeAt);
                }
                else
                {
                    if (!secondFill)
                    {
                        CreateBlock(placeAt);
                        secondFill = true;
                    }
                    else
                    {
                        GameObject last = (GameObject) gameObjects[gameObjects.Count-1];
                        Vector3 second = last.transform.position;

                        FillRegion(placeAt, second);

                        secondFill = false;
                    }
                }
                
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
                        RemoveBlock(i);
                        break;
                    }
                    i++;
                }
            }
        }
    }

    void OnGUI()
    {
        //This is the crosshair
        GUI.Box(new Rect(Screen.width / 2 - 5, Screen.height / 2 - 5, 5, 5), "");
    }

    public void ToggleFill()
    {
        fill = !fill;
        secondFill = false;
    }

    private void CreateBlock(Vector3 placeAt)
    {
        //Instantiate the block and save it so that we can do other stuff with it later
        GameObject block = (GameObject)GameObject.Instantiate(blockPrefab, placeAt, Quaternion.Euler(Vector3.zero));

        gameObjects.Add(block);

        WorldBlock wblock = new WorldBlock(){
            name = block.name,
            type = blockShape,
            material = blockMaterial,
            direction = blockDirection,
            size = blockSize,
            coords = block.transform.position,
        };

        saveObjects.Add(wblock);
    }

    private void RemoveBlock(int indx)
    {
        blkToRemove.Add(gameObjects[indx]);
        gameObjects.RemoveAt(indx);
        saveObjects.RemoveAt(indx);

        foreach (GameObject wblk in blkToRemove)
        {
            Destroy(wblk);
        }

        blkToRemove = new ArrayList();
    }

    private void RemoveRegion(Vector3 first, Vector3 second)
    {
        Vector3 min = Vector3.Min(first, second);
        Vector3 max = Vector3.Max(first, second);

        ArrayList remove_indexes = new ArrayList();

        for (int i = saveObjects.Count - 1; i >= 0; i--)
        {
            Vector3 blk = ((WorldBlock)saveObjects[i]).coords;
            if (blk.x >= min.x && blk.x <= max.x &&
                blk.y >= min.y && blk.y <= max.y &&
                blk.z >= min.z && blk.z <= max.z)
            {
                remove_indexes.Add(i);
            }
        }
        foreach(int j in remove_indexes)
        {
            RemoveBlock(j);
        }        
    }

    private void FillRegion(Vector3 first, Vector3 second)
    {
        RemoveRegion(first, second);
        Vector3 min = Vector3.Min(first, second);
        Vector3 max = Vector3.Max(first, second);

        for (int x = (int)min.x; x <= max.x; x++)
        {
            for (int y = (int)min.y; y <= max.y; y++)
            {
                for (int z = (int)min.z; z <= max.z; z++)
                {
                    CreateBlock(new Vector3(x, y, z));
                }
            }
        }
    }

    public void SaveData()
    {
        Map save = new Map();
        int num_blocks = saveObjects.Count;
        save.Blocks = new WorldBlock[num_blocks];
        for(int i = 0; i < num_blocks; i++)
        {
            save.Blocks[i] = (WorldBlock)saveObjects[i];
        }
        string js = JsonUtility.ToJson(save);
        File.WriteAllText("write.txt", js);
    }

    public void LoadData()
    {
        string f = File.ReadAllText("write.txt");
        Map save = JsonUtility.FromJson<Map>(f);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            RemoveBlock(0);
        }
        gameObjects = new ArrayList();
        saveObjects = new ArrayList();

        foreach(WorldBlock w in save.Blocks)
        {
            CreateBlock(w.coords);
        }
    }
}

[Serializable]
public class WorldBlock
{
    public string type;
    public string material;
    public string direction;
    public string size;
    public string name;
    public Vector3 coords;
}

[Serializable]
public class Map
{
    public WorldBlock[] Blocks;
}