using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Player player;

    public LevelPice[] levelPices;
    public Transform _camera;
    public int drawDistance;

    public float speed;
    private float pieceLength;

    private Queue<GameObject> activatePieces = new Queue<GameObject>();
    private List<int> probabilityList = new List<int>();
    private List<int> probabilityListObstacles = new List<int>();

    private int currentCamStep = 0;
    private int lastCamStep = 0;

    // Componenti interne a un Piece
    public LevelObstacle[] levelObstacles;
    public GameObject _coin;

    public int num_coin;
    public float altezza = 0.3f;

    public int stradeLibere;
    public float[] xPos;

    private GameObject lastObstacle;
    private float lastObstacleX;

    void Start()
    {
        BuildProbabilityList();
        for (int i = 0; i < drawDistance; i++)
        {
            SpawnNewLevelPiece();
        }

        currentCamStep = (int)(_camera.transform.position.z / pieceLength);
        lastCamStep = currentCamStep;
    }

    void Update()
    {
        if (player.hasDie)
            return;

        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _camera.transform.position + Vector3.forward, Time.deltaTime * speed);
        currentCamStep = (int)(_camera.transform.position.z / pieceLength);
        if (currentCamStep != lastCamStep)
        {
            lastCamStep = currentCamStep;
            DespawnLevelPiece();
            SpawnNewLevelPiece();
        }
    }

    /**
     * Crea una nuovo instance di un segmento di una strada
     * Sceglie in maniera randomica quale strada creare
     */
    private void SpawnNewLevelPiece()
    {
        int piceIndex = probabilityList[Random.Range(0, probabilityList.Count)];
        pieceLength = levelPices[piceIndex].prefab.GetComponent<Renderer>().bounds.size.z;
        // X -> 0 perche si
        // Y -> 0 anche perche si
        // Z -> posizione attuale della telecamera (la prima strada + la quantitàa totale di strade che ci sono) * la lunghezza della strada
        GameObject newLevelPiece = Instantiate(levelPices[piceIndex].prefab, new Vector3(0f, 0f, (currentCamStep + activatePieces.Count) * pieceLength), Quaternion.identity);
        activatePieces.Enqueue(newLevelPiece);

        // Conto le strade a cui non creare oggetti
        if (stradeLibere < 1)
        {
            // Compargono prima gli ostacoli per dopo non far comparire le monete in quel posto
            SpawnObstacle(newLevelPiece);
            SpawnCoins(newLevelPiece);
        }
        stradeLibere--;
    }

    /**
     * Togli dalla lista il primo segmento della strada e lo distrugge
     */
    private void DespawnLevelPiece()
    {
        GameObject oldLevelPice = activatePieces.Dequeue();
        Destroy(oldLevelPice);
    }

    /**
     * Crea una lista con gli id dei segmenti di strada a creare, per ogni quantità di probabilità
     */
    private void BuildProbabilityList()
    {
        int index = 0;
        foreach (LevelPice piece in levelPices)
        {
            for (int i = 0; i < piece.probability; i++)
            {
                probabilityList.Add(index);
            }
            index++;
        }

        index = 0;
        foreach (LevelObstacle obstacle in levelObstacles)
        {
            for (int i = 0; i < obstacle.probability; i++)
            {
                probabilityListObstacles.Add(index);
            }
            index++;
        }
    }

    /**
     * Crea le monete a raccogliere dentro la strada
     */
    private void SpawnCoins(GameObject piece)
    {
        // Seleziono in maniera randomica la line a popolare
        float random_lane = xPos[Random.Range(0, xPos.Length)];

        // Prendo i punti di start ed end dentro il Piece dove creerò le mie monete
        Vector3 start = new Vector3(random_lane, altezza, piece.transform.position.z - pieceLength / 2);
        Vector3 end = new Vector3(random_lane, altezza, piece.transform.position.z + pieceLength / 2);

        // Il la distanza tra ogni moneta
        float delta = Vector3.Distance(start, end) / num_coin;
        // La posizione in z per ogni moneta
        float contatore = start.z;
        for (int i = 0; i < num_coin; i++)
        {
            // Se lostacolo non esiste o non coincide la lane, asse x
            if (lastObstacle == null || lastObstacleX != random_lane)
            {
                // Instanzo una nuova moneta nella nuova posizione
                // la moneta e figlia del Piece
                GameObject go = Instantiate(_coin, piece.transform);
                go.transform.position = new Vector3(random_lane, altezza, contatore);
                go.name = "coin-" + i;
            }
            else // Esiste un ostacolo e non èstato gia trovato
            {
                float obstacle_z = lastObstacle.transform.position.z;
                var obstacle_dim = lastObstacle.GetComponent<Renderer>().bounds.size.z / 2;
                // Se l'ostacolo non coincide con la moneta, la moneta viene dipinta
                if (contatore > (obstacle_z + obstacle_dim + delta) || contatore < (obstacle_z - obstacle_dim - delta))
                {
                    // Instanzo una nuova moneta nella nuova posizione
                    // la moneta e figlia del Piece
                    GameObject go = Instantiate(_coin, piece.transform);
                    go.transform.position = new Vector3(random_lane, altezza, contatore);
                    go.name = "coin-" + i;
                }
            }
            // Vado alla seguente posizione
            contatore += delta;
        }
    }

    /**
     * Crea gli ostacoli dentro la strada
     */
    private void SpawnObstacle(GameObject piece)
    {
        lastObstacle = null;

        // Seleziono in maniera randomica la line a popolare
        float random_lane = xPos[Random.Range(0, xPos.Length)];
        // Seleziono in maniera randomica l'ostacolo a creare
        int obstacleIndex = probabilityListObstacles[Random.Range(0, probabilityListObstacles.Count)];

        // Aggiusto il range ha creare i miei ostacoli
        float max_range = (pieceLength - 2) / 2;
        // Prendo una posizione random dove creare il mio ostacolo
        float pos_z = Random.Range(-max_range, max_range);

        // Vedo se esiste l'ostacolo
        if (levelObstacles[obstacleIndex].prefab != null)
        {
            // Instanzo l'ostacolo come figlio della Piece
            GameObject go = Instantiate(levelObstacles[obstacleIndex].prefab, piece.transform);
            go.transform.localPosition = new Vector3(random_lane, 0, pos_z);
            go.name = "obstacle";

            lastObstacle = go;
            lastObstacleX = random_lane;
        }
    }
}

[System.Serializable]
public class LevelPice {
    public string name;
    public GameObject prefab;
    public int probability = 1;
}

[System.Serializable]
public class LevelObstacle
{
    public string name;
    public GameObject prefab;
    public int probability = 1;
    public string type;
}