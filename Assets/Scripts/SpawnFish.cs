using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{
    //public Vector3 start_point;

    public GameObject[] roads;
    public GameObject[] singles_obstacles;

    //public GameObject road;
    public GameObject _coin;

    public int num_coin;
    public float altezza = 0.3f;

    private float start_point;

    // Start is called before the first frame update
    void Start()
    {
        start_point = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Crea la strada
        CreateRoad();
    }

    private void CreateRoad()
    {
        // 
        GameObject road = SelectRoad();
        Vector3 road_size = road.GetComponent<Renderer>().bounds.size;

        start_point = road.transform.position.z + road_size.z;

        SpawnCoins(road_size, road);
        SpawnObstacle(road_size, road);
    }

    private GameObject SelectRoad()
    {
        int random_id_road = Random.Range(0, roads.Length);
        GameObject go = GameObject.Instantiate(roads[random_id_road], new Vector3(0f, 0f, start_point), roads[random_id_road].transform.rotation);
        return go;
    }

    private void SpawnCoins(Vector3 road_size, GameObject road)
    {
        int random_lane = Random.Range(-1, 2);

        GameObject start = GameObject.Instantiate(new GameObject(), road.transform);
        start.transform.localPosition = new Vector3(random_lane, altezza, -road_size.z / 2);
        start.name = "start";

        GameObject end = GameObject.Instantiate(new GameObject(), road.transform);
        end.transform.localPosition = new Vector3(random_lane, altezza, road_size.z / 2);
        end.name = "end";

        float delta = Vector3.Distance(start.transform.localPosition, end.transform.localPosition) / num_coin;
        float contatore = start.transform.localPosition.z;
        for (int i = 0; i < num_coin; i++)
        {
            GameObject go = GameObject.Instantiate(_coin, road.transform);
            go.transform.localPosition = new Vector3(random_lane, altezza, contatore);
            go.name = "fish-" + i;

            contatore += delta;
        }
    }

    private void SpawnObstacle(Vector3 road_size, GameObject road)
    {
        int random_id_obstacle = Random.Range(0, singles_obstacles.Length);
        int random_lane = Random.Range(-1, 2);

        float max_range = (road_size.z - 2) / 2;
        float pos_z = Random.Range(-max_range, max_range);

        GameObject go = GameObject.Instantiate(singles_obstacles[random_id_obstacle], road.transform);
        go.transform.localPosition = new Vector3(random_lane, 0, pos_z);
        go.name = "obstacle";
    }
}
