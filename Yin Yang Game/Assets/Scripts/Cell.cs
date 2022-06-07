using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool CellColor = false; // False - Cell is black; True - Cell is white
    public bool IsAlive = false;
    public int whiteNeighbors = 0;
    public int blackNeighbors = 0;
    public int numNeighbors = 0;
    public void SetAlive(bool alive, bool cellColor)
    {
        CellColor = cellColor;
        if (CellColor)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }


        IsAlive = alive;
        if (alive)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
