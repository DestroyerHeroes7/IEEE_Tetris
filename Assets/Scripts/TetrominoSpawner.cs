using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public static TetrominoSpawner Instance;
    public List<GameObject> tetrominos;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SpawnTetromino();
    }
    void Update()
    {
        
    }

    public void SpawnTetromino()
    {
        Instantiate(tetrominos[Random.Range(0, tetrominos.Count)], transform.position, Quaternion.identity);
    }
}
