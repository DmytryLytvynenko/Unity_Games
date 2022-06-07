using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIDTH = 64;//- 1024 pixels
    private static int SCREEN_HEIGHT = 48;//- 768 pixels

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
    public float speed = 0.1f;

    private float timer = 0f;

    public bool simulationEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;
                CountNeighbors();
                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        UserInput();
    }


    void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePpoint.x);
            int y = Mathf.RoundToInt(mousePpoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
            {
                //  Мы внутри экрана
                grid[x, y].SetAlive(!grid[x, y].IsAlive, false);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePpoint.x);
            int y = Mathf.RoundToInt(mousePpoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
            {
                //  Мы внутри экрана

                grid[x, y].SetAlive(!grid[x, y].IsAlive, true);

            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            // Pause
            simulationEnabled = false;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            // Begin(resume)
            simulationEnabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !simulationEnabled)
        {
            // Step
            CountNeighbors();
            PopulationControl();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            // Clear
            simulationEnabled = false;

            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    grid[x, y].SetAlive(false, grid[x, y].CellColor);
                }
            }
        }
        if (Input.GetKey(KeyCode.Escape))  // Quit
        {
            Application.Quit();    // закрыть приложение
        }
    }
    void PlaceCells()
    {
        bool rand;
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                rand = RandomCellColor();

                Cell cell = Instantiate(Resources.Load("PreFabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                grid[x, y].SetAlive(RandomAliveCell(), rand);
            }
        }
    }

    void PopulationControl()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                // Rules
                // 1.Любая живая клетка с 2 или 3 живыми соседями выживает.
                // 2.Любая мертвая клетка с 3 живыми соседями становится живой
                // 3.Все другие живые без достаточного количества соседей клетки умирают на следующем ходу,
                // все мертвые без достаточного количества соседей остаются мертвыми
                if (grid[x, y].IsAlive)
                {
                    // Клетка жива
                    if (grid[x, y].numNeighbors < 2 || grid[x, y].numNeighbors > 4)
                    {
                        grid[x, y].SetAlive(false, grid[x, y].CellColor);
                    }

                    if (grid[x, y].CellColor && grid[x, y].blackNeighbors >= 3)
                    {
                        grid[x, y].SetAlive(false, grid[x, y].CellColor);
                    }
                    if (!grid[x, y].CellColor && grid[x, y].whiteNeighbors >= 3)
                    {
                        grid[x, y].SetAlive(false, grid[x, y].CellColor);
                    }
                }
                else
                {
                    // Клетка мертва
                    if (grid[x, y].numNeighbors == 3 && grid[x, y].whiteNeighbors == 2)
                    {
                        grid[x, y].SetAlive(true, false);
                    }
                    if (grid[x, y].numNeighbors == 3 && grid[x, y].blackNeighbors == 2)
                    {
                        grid[x, y].SetAlive(true, true);
                    }
                }
            }
        }
    }
    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int whiteNeighbors = 0;
                int blackNeighbors = 0;

                // North (Up)
                if (y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x, y + 1].IsAlive)
                    {
                        if (grid[x, y + 1].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                // East (Right)
                if (x + 1 < SCREEN_WIDTH)
                {
                    if (grid[x + 1, y].IsAlive)
                    {
                        if (grid[x + 1, y].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                // South (Down)
                if (y - 1 >= 0)
                {
                    if (grid[x, y - 1].IsAlive)
                    {
                        if (grid[x, y - 1 ].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                // West (Left)
                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].IsAlive)
                    {
                        if (grid[x - 1, y].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                //NorthEast (Up Right)
                if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x + 1, y + 1].IsAlive)
                    {
                        if (grid[x + 1, y + 1].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                //NorthWest (Up Left)
                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x - 1, y + 1].IsAlive)
                    {
                        if (grid[x - 1, y + 1].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                //SouthEast (Down Right)
                if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                {
                    if (grid[x + 1, y - 1].IsAlive)
                    {
                        if (grid[x + 1, y - 1].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                //SouthWest (Down Left)
                if (x - 1  >= 0 && y - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].IsAlive)
                    {
                        if (grid[x - 1, y - 1].CellColor)
                        {
                            whiteNeighbors++;
                        }
                        else
                        {
                            blackNeighbors++;
                        }
                    }
                }

                grid[x, y].blackNeighbors = blackNeighbors;
                grid[x, y].whiteNeighbors = whiteNeighbors;
                grid[x, y].numNeighbors = blackNeighbors + whiteNeighbors;
            }
        }
    }
    bool RandomAliveCell()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > 75)
        {

            return true;
        }
       return false;
    }
    bool RandomCellColor()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > 50)
        {

            return true;
        }
        return false;
    }
}

