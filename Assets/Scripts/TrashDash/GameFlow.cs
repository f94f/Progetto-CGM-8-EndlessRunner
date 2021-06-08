using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public Transform title_obj;
    public Transform title_obj_last;
    public GameObject[] obstacles;

    private Vector3 nextTitleSpawn;
    private Vector3 nextObsSpawn;
    private int randomZ;

    // Start is called before the first frame update
    void Start()
    {
        nextTitleSpawn = title_obj_last.position;
        nextTitleSpawn.x += 30;

        StartCoroutine(SpawnTitle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnTitle()
    {
        yield return new WaitForSeconds(2);
        var tr = Instantiate(title_obj, nextTitleSpawn, title_obj.rotation);
        List<Vector3> listObsSpawn = FindChildWithTag(tr.Find("Road"), "SpawnObstacle");
        SpawnObstacles(listObsSpawn, obstacles);

        nextTitleSpawn.x += 30;
        StartCoroutine(SpawnTitle());
    }

    private void SpawnObstacles(List<Vector3> listObsSpawn, GameObject[] obstacles)
    {
        Vector3 randomSpawn = listObsSpawn[Random.Range(0, listObsSpawn.Count)];
        GameObject randomObs = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(randomObs, randomSpawn, randomObs.transform.rotation);
    }

    public List<Vector3> FindChildWithTag(Transform parent, string tag)
    {
        List<Vector3> resp = new List<Vector3>();
        foreach (Transform tr in parent)
        {
            if (tr.tag == tag)
            {
                if (tr.name.Contains("mid"))
                {
                    resp.Add(new Vector3(tr.position.x, 3, tr.position.z));
                }
                else
                {
                    resp.Add(tr.position);
                }
            }
        }
        return resp;
    }
}
