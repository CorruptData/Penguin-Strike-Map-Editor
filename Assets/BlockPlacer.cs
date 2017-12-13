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

    //Current block data
    private string blockMaterial = "Steel";
    private string blockShape = "Cube";
    private string blockDirection = "";
    private string blockSize = "";

    //These keep track of the blocks
    private ArrayList gameObjects = new ArrayList();
    private ArrayList saveObjects = new ArrayList();
    private ArrayList blkToRemove = new ArrayList();

    //Undo Tool
    //TODO: Not implemented
    private ArrayList gameObjectsUndo = new ArrayList();
    private ArrayList saveObjectsUndo = new ArrayList();
    private int a;

    //Maximum range that the player can place a block from
    public float range = 7f;
    
    //Fill tool flags
    private bool fill = false;
    private bool secondFill = false;

    void Start()
    {
        //This assigns the static reference
        BlockPlacer.obj = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Test code for block switching
        //TODO: Implement fully and properly
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            blockPrefab = Resources.Load("Prefabs/"+blockMaterial+blockShape+blockDirection, typeof(GameObject)) as GameObject;
        }

        //Locks the cursor. Running this in update is only done because of weird unity editor shenanigans with cursor locking
        if (Input.GetKeyDown(KeyCode.C))
        {
            lockCursor = !lockCursor;
        }

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
                    //Place block
                    CreateBlock(placeAt);
                }
                else
                {
                    if (!secondFill)
                    {
                        //Place block, and prepare to fill the area
                        CreateBlock(placeAt);
                        //Flag for the fill tool
                        secondFill = true;
                    }
                    else
                    {
                        //Fill the area between the previous block placed and the place clicked
                        GameObject previous = (GameObject) gameObjects[gameObjects.Count-1];
                        Vector3 second = previous.transform.position;

                        FillRegion(placeAt, second);

                        secondFill = false;
                    }
                }
            }
        }

        //Right click to remove a block
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

    //Toggle the fill tool
    public void ToggleFill()
    {
        fill = !fill;
        secondFill = false;
    }

    //Create a block at the given location
    //Uses the block type class-wide
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

    //Destroy a block at the given index in the gameObject list
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

    //Remove blocks from a paticular area
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

    //Fill an area with a paticular block type
    private void FillRegion(Vector3 first, Vector3 second)
    {
        //Get rid of the blocks in that area first
        //This keeps blocks from overlapping
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

    //Save the current map
    public void SaveData()
    {
        Map save = new Map();

        int num_blocks = saveObjects.Count;
        save.Blocks = new WorldBlock[num_blocks];

        for(int i = 0; i < num_blocks; i++)
        {
            save.Blocks[i] = (WorldBlock)saveObjects[i];
        }

        //Create json and write to file
        string js = JsonUtility.ToJson(save);
        File.WriteAllText("write.txt", js);
    }

    //Load a map from json
    public void LoadData()
    {
        string f = File.ReadAllText("write.txt");
        Map save = JsonUtility.FromJson<Map>(f);

        //First, remove all the blocks
        int del = gameObjects.Count;
        for (int i = 0; i < del; i++)
        {
            RemoveBlock(0);
        }

        gameObjects = new ArrayList();
        saveObjects = new ArrayList();
        
        //Create the loaded blocks
        foreach(WorldBlock w in save.Blocks)
        {
            CreateBlock(w.coords);
        }
    }
}

//Data for saving for each block
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

//Map data + blocks
[Serializable]
public class Map
{
    public WorldBlock[] Blocks;
}

//Mickey JP McCargish, Neutral Space, 2017
//Thanks to https://pastebin.com/140bv3kW for the block placement location code
