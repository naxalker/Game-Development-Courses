using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private enum ChunkShape
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four
    }

    [Header("Elements")]
    [SerializeField] private Transform world;
    private Chunk[,] grid;

    [Header("Settings")]
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;

    [Header("Data")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] chunkShapes;
    
    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangedCallback;
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangedCallback;
    }

    private void Start()
    {
        dataPath = Application.dataPath + "/worldData.txt";

        LoadWorld();
        Initialize();

        InvokeRepeating("TrySaveGame", 1f, 1f);
    }

    private void ChunkUnlockedCallback()
    {
        UpdateChunkWalls();
        UpdateGridRenderers();
        SaveWorld();
    }

    private void ChunkPriceChangedCallback()
    {
        shouldSave = true;
    }

    private void Initialize()
    {
        for (int i = 0; i < world.childCount; i++)
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);

        InititalizeGrid();
        UpdateChunkWalls();
        UpdateGridRenderers();
    }

    private void InititalizeGrid()
    {
        grid = new Chunk[gridSize, gridSize];

        for (int i = 0; i < world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale,
                                                          (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }

    private void UpdateChunkWalls()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null)
                    continue;

                Chunk frontChunk = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    configuration = configuration + 1;

                if (rightChunk != null && rightChunk.IsUnlocked())
                    configuration = configuration + 2;

                if (backChunk != null && backChunk.IsUnlocked())
                    configuration = configuration + 4;

                if (leftChunk != null && leftChunk.IsUnlocked())
                    configuration = configuration + 8;

                chunk.UpdateWalls(configuration);

                SetChunkRenderer(chunk, configuration);
            }
        }
    }

    private void SetChunkRenderer(Chunk chunk, int configuration)
    {
        switch (configuration)
        {
            case 0:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]);
                break;
            case 1:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]);
                break;
            case 2:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]);
                break;
            case 3:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]);
                break;
            case 4:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]);
                break;
            case 5:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 6:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]);
                break;
            case 7:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 8:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]);
                break;
            case 9:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]);
                break;
            case 10:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 11:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 12:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]);
                break;
            case 13:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 14:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 15:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
        }
    }

    private void UpdateGridRenderers()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null)
                    continue;

                if (chunk.IsUnlocked())
                    continue;
                
                Chunk frontChunk = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                if (frontChunk != null && frontChunk.IsUnlocked()
                    || rightChunk != null && rightChunk.IsUnlocked()
                    || backChunk != null && backChunk.IsUnlocked()
                    || leftChunk != null && leftChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
            }
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return !(x < 0 || x >= gridSize || y < 0 || y >= gridSize);
    }

    private void TrySaveGame()
    {
        if (shouldSave)
        {
            SaveWorld();
            shouldSave = false;
        }
    }

    private void LoadWorld()
    {
        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);

            worldData = new WorldData();

            for (int i = 0; i <  world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);
            }

            string worldDataString = JsonUtility.ToJson(worldData, true);

            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);

            fs.Write(worldDataBytes);

            fs.Close();
        } else
        {
            string data = File.ReadAllText(dataPath);

            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                UpdateData();
        }
    }

    private void UpdateData()
    {
        int missingData = world.childCount - worldData.chunkPrices.Count;

        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void SaveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();

            if (worldData.chunkPrices.Count > i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else
                worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(dataPath, data);

        Debug.LogWarning("World Data saved !");
    }
}
