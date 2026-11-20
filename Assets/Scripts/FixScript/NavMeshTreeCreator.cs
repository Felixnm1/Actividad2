using UnityEngine;

public class NavMeshTreeCreator : MonoBehaviour
{
    Terrain[] terrain;
    [SerializeField] GameObject obstacleObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        terrain = FindObjectsByType<Terrain>(FindObjectsSortMode.None);
        if (terrain != null && obstacleObj != null)
        {
            foreach (Terrain terr in terrain)
            {
                generateObstacles(terr);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateObstacles(Terrain terrain)
    {
        TerrainData data = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        foreach(TreeInstance tree in data.treeInstances)
        {
            GameObject obs = Instantiate(obstacleObj);
            obs.transform.position = Vector3.Scale(tree.position, data.size) + terrainPos;
            obs.transform.parent = transform;

        }
    }
    
}
