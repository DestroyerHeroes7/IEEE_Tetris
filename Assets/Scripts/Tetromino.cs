using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Vector3 rotationPoint;
    private Coroutine moveDown;
    private static Transform[,] grid = new Transform[18, 32];
    private void Start()
    {
         moveDown = StartCoroutine(MoveDown());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if(!isValid())
                transform.position -= Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!isValid())
                transform.position -= Vector3.right;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if (!isValid())
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
        }
    }
    private void AddGrid()
    {
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            grid[roundedX, roundedY] = child;
        }
    }
    private IEnumerator MoveDown()
    {
        while(true)
        {
            transform.position += Vector3.down;
            if (!isValid())
            {
                transform.position -= Vector3.down;
                AddGrid();
                CheckForLine();
                this.enabled = false;
                TetrominoSpawner.Instance.SpawnTetromino();
                StopCoroutine(moveDown);
            }
            if (Input.GetKey(KeyCode.DownArrow))
                yield return new WaitForSeconds(0.1f);
            else
                yield return new WaitForSeconds(1);
        }
    }
    private void CheckForLine()
    {
        for(int i = 0; i < Global.gridHeight;i++)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }
    private bool HasLine(int line)
    {
        for(int i = 0; i < Global.gridWidth;i++)
        {
            if (grid[i, line] == null)
                return false;
        }
        return true;
    }
    private void DeleteLine(int line)
    {
        for(int i = 0; i < Global.gridWidth;i++)
        {
            Destroy(grid[i, line].gameObject);
            grid[i, line] = null;
        }
    }
    private void RowDown(int line)
    {
        for(int y = line;y < Global.gridHeight;y++)
        {
            for(int j = 0;j < Global.gridWidth;j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= Vector3.up; 
                }
            }
        }
    }
    private bool isValid()
    {
        foreach (Transform t in transform)
        {
            int roundedX = Mathf.RoundToInt(t.position.x);
            int roundedY = Mathf.RoundToInt(t.position.y);
            if (roundedX >= Global.gridWidth || roundedX < 0 || roundedY < 0)
                return false;
            Debug.Log(roundedX + "  " + roundedY);
            if (grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }
}
