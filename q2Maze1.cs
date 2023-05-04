//made by 
//Amber Ahmed #0680481
//Dharam Raj Patel #0719581
//Rocky sharaf 

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class Maze
{
    // 2D array to store the maze
    private char[,] M;

    // Size of the maze
    private int size;

    // Dimension of the maze (n x n)
    private int n;

    // Constructor that takes in the dimension of the maze
    public Maze(int n)
    {
        // Store the dimension in the instance variable
        this.n = n;

        // Call the Initialize method
        Initialize();

        // Call the Create method to build the maze
        Create();
    }

    private void Initialize()
    {
        // Calculate the size of the maze
        size = 2 * n + 3;

        // Create the 2D array to store the maze
        M = new char[size, size];

        // Loop through the first half of the size of the maze
        for (int i = 0; i < size / 2; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // If j is odd, fill with vertical wall ('|')
                if (j % 2 != 0)
                {
                    M[i, j] = '|';
                }
                // If i is odd, fill with horizontal wall ('-')
                else if (i % 2 != 0)
                {
                    M[i, j] = '-';
                }
                // If i equals the n-th row, fill with horizontal wall ('-')
                else if ((i == n) & (M[(n), j] != '-'))
                {
                    M[(n), j] = '-';
                }
                // Fill with space otherwise
                else
                {
                    M[i, j] = ' ';
                }
            }
        }
    }





    private void Create()
    {
        // Initialize visited array with size of the matrix
        bool[,] visited = new bool[size, size];

        // Mark cells that are not inside the matrix border as unvisited
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (j % 2 == 0 && i % 2 == 0 && i != 0 && i != size - 1 && j != 0 && j != size - 1)
                {
                    visited[i, j] = false;
                }
                else
                {
                    visited[i, j] = true;
                }
            }
        }

        // Iterate through each cell of the matrix and perform DFS if the cell is unvisited
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (!visited[i, j])
                {
                    DepthFirstSearch(i, j, visited);
                }
            }
        }
    }



    // This method implements the Depth First Search algorithm on a 2D grid
    // to generate a random maze.
    private void DepthFirstSearch(int i, int j, bool[,] visited)
    {
        // Mark the current cell as visited
        visited[i, j] = true;

        // Create a random number generator
        Random r = new Random();

        // Store the current position
        int a = i;
        int b = j;

        // Create a list to store the possible directions
        List<char> directions = new List<char>();

        // If the cell to the right is within bounds and unvisited, add direction 'A' to the list
        if (j + 2 < size && !visited[i, j + 2])
        {
            directions.Add('A');
        }

        // If the cell to the left is within bounds and unvisited, add direction 'B' to the list
        if (j - 2 >= 0 && !visited[i, j - 2])
        {
            directions.Add('B');
        }

        // If the cell above is within bounds and unvisited, add direction 'C' to the list
        if (i - 2 >= 0 && !visited[i - 2, j])
        {
            directions.Add('C');
        }

        // If the cell below is within bounds and unvisited, add direction 'D' to the list
        if (i + 2 < size && !visited[i + 2, j])
        {
            directions.Add('D');
        }

        // If there are any possible directions, choose a random direction
        if (directions.Count > 0)
        {
            char dir = directions[r.Next(directions.Count)];

            // Move to the chosen direction and remove the wall in between
            if (dir == 'A')
            {
                M[i, j + 1] = ' ';
                b = j + 2;
            }
            else if (dir == 'B')
            {
                M[i, j - 1] = ' ';
                b = j - 2;
            }
            else if (dir == 'C')
            {
                M[i - 1, j] = ' ';
                a = i - 2;
            }
            else if (dir == 'D')
            {
                M[i + 1, j] = ' ';
                a = i + 2;
            }

            // Recursively call the Depth First Search algorithm from the new position
            DepthFirstSearch(a, b, visited);
        }
    }



    // Method to print the maze
    public void Print()
    {
        // Loop through all the rows in the maze (excluding the first and last row)
        for (int i = 1; i < size - 1; i++)
        {
            // Loop through all the columns in the current row (excluding the first and last column)
            for (int j = 1; j < size - 1; j++)
            {
                // Print the value of the current cell
                Console.Write(M[i, j]);
            }
            // Move to the next line after printing the current row
            Console.WriteLine();
        }
    }



    static void Main(string[] args)
    {
        // Test for n = 0, 1, 2, 5, 10, 20
        int[] testCases = new int[] { 0, 1, 2, 5, 10, 20 };

        foreach (int n in testCases)
        {
            Console.WriteLine("Maze for n = " + n);

            // Create a Maze object with the given n
            Maze maze = new Maze(n);

            // Print the maze
            maze.Print();

            Console.WriteLine();
        }
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

}