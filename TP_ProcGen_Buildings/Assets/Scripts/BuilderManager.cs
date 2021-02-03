using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    public Vector3Int gridSize;
    public GameObject buildPreview;
    public GameObject buildBlockPrefab;
    public ProcGrid Grid { get; private set; }
    public int buildingLentgh = 5;
    public bool startGeneration = false;


    private new Camera camera;
    private List<GameObject> previewBlocks;

    void Start()
    {
        camera = Camera.main;
        Grid = new ProcGrid(gridSize);
        previewBlocks = new List<GameObject>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); //Create ray from mouse position

        if (Physics.Raycast(ray, out hit))  //Trace ray
        {
            Vector3Int? cell = Grid.GetCellByWorldPos(hit.point + hit.normal * 0.1f); //Try to get grid cell
            if (cell != null)
            {
                buildPreview.SetActive(true);   //Show build preview
                buildPreview.transform.position = Grid.CellCenter((Vector3Int)cell); //Move build preview to cell position
            }
            else
                buildPreview.SetActive(false);  //Hide build preview if mouse is outside the grid
        }
        else
            buildPreview.SetActive(false);//Hide build preview if nothing was hit


        if (Input.GetKeyDown(KeyCode.Mouse0) && buildPreview.activeSelf) //If user mouse click
        {
            Vector3Int? cell = Grid.GetCellByWorldPos(buildPreview.transform.position); //Get the cell of the build preview 
            if (cell != null)
            {
                PlacePreviewBlock((Vector3Int)cell); //Place a preview block
            }
        }

        if (startGeneration)
        {
            startGeneration = false;
            AutoGenerate();
        }
    }

    private void AutoGenerate()
    {
        int x = gridSize.x / 2;         //get the middle of x axis in grid
        int z = gridSize.z / 2;         //get the middle of z axis in grid

        for (int i = 0; i < buildingLentgh; i++)    //for i = 0 to the number of desired iterations
        {
            PlacePreviewBlock(new Vector3Int(x, 0, z));     //Place the block at x and z

            int axis = Random.Range(0, 2); //Random between 0 and 1
            int dir = Random.Range(-1, 2); //Random between -1 and 1
            if (axis == 0)  //0 = x  
                x += dir;   //move on x axis
            else            //1 = z
                z += dir;   //move on z axis
        }

        int y = Random.Range(2, gridSize.y - 1);    //Get random between minimal height and max height - 1
        int count = previewBlocks.Count;            //Save the number of actual preview blocks placed on ground
        for (int k = 0; k < count; k++)             //For each preview blocks on ground
        {
            for (int i = 1; i < y; i++)             //For 1 to desired height
            {
                Vector3Int? groundCellPos = Grid.GetCellByWorldPos(previewBlocks[k].transform.position);
                Vector3Int cellPos = (Vector3Int)groundCellPos + Vector3Int.up * i; //Get cell position in height
                if (groundCellPos != null && cellPos.y < gridSize.y)
                    PlacePreviewBlock(cellPos);
            }
        }
    }

    private void PlacePreviewBlock(Vector3Int cellPos)
    {
        Cell cell = Grid.GetCell(cellPos);
        if (cell != null && !cell.enabled)
        {
            GameObject go = Instantiate(buildBlockPrefab, Grid.GetCell(cellPos).GetCenter(), Quaternion.identity);
            previewBlocks.Add(go);
            Grid.EnableCell(cellPos);
        }
    }

    public void ClearPreview()
    {
        for (int i = 0; i < previewBlocks.Count; i++)
        {
            Destroy(previewBlocks[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((float)gridSize.x / 2, (float)gridSize.y / 2, (float)gridSize.z / 2), gridSize);

    }
}
