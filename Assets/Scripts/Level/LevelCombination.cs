using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCombination : MonoBehaviour
{
    private const int gridSize = 5;  // Grid boyutu
    private List<Vector2Int> pieces = new List<Vector2Int>
    {
        new Vector2Int(1, 1), new Vector2Int(2, 2), new Vector2Int(3, 3), new Vector2Int(4, 4),
        new Vector2Int(1, 2), new Vector2Int(2, 1), new Vector2Int(1, 3), new Vector2Int(3, 1),
        new Vector2Int(2, 3), new Vector2Int(3, 2), new Vector2Int(3, 4), new Vector2Int(4, 3),
        new Vector2Int(2, 6), new Vector2Int(6, 2), new Vector2Int(1, 5), new Vector2Int(5, 1),
        new Vector2Int(2, 5), new Vector2Int(5, 2)
    };

    private int[,] grid = new int[gridSize, gridSize]; // 5x5 grid

    void Start()
    {
        // Başlangıçta grid'i yazdır
        Debug.Log("Başlangıç grid'i:");
        PrintGrid();

        // Kombinasyonları bul ve yazdır
        FindCombinations(0);
    }

    void FindCombinations(int idx)
    {
        if (idx == pieces.Count)
        {
            Debug.Log("Geçerli bir kombinasyon bulundu:");
            PrintGrid();
            return;
        }

        Vector2Int piece = pieces[idx];
        int width = piece.x;
        int height = piece.y;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (CanPlace(x, y, width, height))
                {
                    Place(x, y, width, height, 1); // Parçayı yerleştir
                    Debug.Log($"Yerleştirilen {width}x{height} parçası: ({x}, {y})");
                    FindCombinations(idx + 1); // Sonraki parçayı dene
                    Remove(x, y, width, height); // Parçayı çıkar
                }
            }
        }
    }

    bool CanPlace(int x, int y, int width, int height)
    {
        if (x + width > gridSize || y + height > gridSize)
            return false;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[y + j, x + i] != 0) // Eğer çakışma varsa
                {
                    return false;
                }
            }
        }

        return true;
    }

    void Place(int x, int y, int width, int height, int value)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[y + j, x + i] = value;
            }
        }
    }

    void Remove(int x, int y, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[y + j, x + i] = 0;
            }
        }
    }

    void PrintGrid()
    {
        string gridRepresentation = "";
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                gridRepresentation += grid[y, x] + " ";
            }
            gridRepresentation += "\n";
        }
        Debug.Log(gridRepresentation);  // Unity konsolunda yazdır
    }
}
    
