using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    public Vector3Int gridSize;
    public GameObject buildPreview;
    public GameObject buildBlockPrefab;
    public ProcGrid Grid { get; private set; }

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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            //Transform objectHit = hit.transform;
            Vector3Int? cell = Grid.GetCellByWorldPos(hit.point + hit.normal * 0.1f);
            if (cell != null)
            {
                buildPreview.SetActive(true);
                buildPreview.transform.position = Grid.CellCenter((Vector3Int)cell);
            }
            else
                buildPreview.SetActive(false);
        }
        else
            buildPreview.SetActive(false);


        if (Input.GetKeyDown(KeyCode.Mouse0) && buildPreview.activeSelf)
        {
            Vector3Int? cell = Grid.GetCellByWorldPos(buildPreview.transform.position);
            if (cell != null)
            {
                GameObject go = Instantiate(buildBlockPrefab, buildPreview.transform.position, Quaternion.identity);
                previewBlocks.Add(go);
                Vector3Int cellPos = (Vector3Int)cell;
                Grid.EnableCell(cellPos);
            }
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
