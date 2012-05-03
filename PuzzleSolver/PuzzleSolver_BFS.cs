using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleSolver
{
    class PuzzleSolver_BFS
    {
        const int PuzzleSize = 3;
        const int PuzzleLimit = PuzzleSize * PuzzleSize - 1;

        List<PuzzleStructure> Queue = new List<PuzzleStructure>();
        List<PuzzleStructure> BackTraceQueue = new List<PuzzleStructure>();

        class PuzzleStructure
        {
            public int[,] Table = new int[PuzzleSize, PuzzleSize];
            public int EmptyPositionX, EmptyPositionY;
            public int Heuristic;

            public PuzzleStructure Parent;
            public List<PuzzleStructure> Child = new List<PuzzleStructure>();
        }

        void AddQueue(PuzzleStructure Input)
        {
            Queue.Add(Input);
        }

        PuzzleStructure DelQueue()
        {
            PuzzleStructure tmp = new PuzzleStructure();

            tmp = Queue[0];
            Queue.RemoveAt(0);

            return (tmp);
        }

        void Sort(List<PuzzleStructure> Input)
        {
            PuzzleStructure tmp = new PuzzleStructure();

            for (int i = 0; i < Queue.Count - 1; i++)
                for (int j = i + 1; j < Queue.Count; j++)
                    if (Input[i].Heuristic > Input[j].Heuristic)
                    {
                        tmp = Input[i];
                        Input[i] = Input[j];
                        Input[j] = tmp;
                    }
        }

        void CalculateHeuristic(PuzzleStructure Input)
        {
            int tmp;

            for (int i = 0; i < PuzzleSize; i++)
                for (int j = 0; j < PuzzleSize; j++)
                {
                    tmp = i * PuzzleSize + j;

                    if (Input.Table[i, j] != tmp && Input.Table[i, j] != PuzzleLimit)
                    {
                        Input.Heuristic += Math.Abs(i - Input.Table[i, j] / PuzzleSize);
                        Input.Heuristic += Math.Abs(j - Input.Table[i, j] % PuzzleSize);
                    }
                }
        }

        PuzzleStructure Solve()
        {
            PuzzleStructure tmp = new PuzzleStructure();

            tmp = DelQueue();

            while (tmp.Heuristic != 0)
            {
                if (tmp.EmptyPositionX - 1 >= 0)
                {
                    PuzzleStructure ShiftLeft = new PuzzleStructure();

                    ShiftLeft.Parent = new PuzzleStructure();
                    ShiftLeft.Parent = tmp;
                    tmp.Child.Add(ShiftLeft);

                    ShiftLeft.EmptyPositionX = tmp.EmptyPositionX - 1;
                    ShiftLeft.EmptyPositionY = tmp.EmptyPositionY;

                    for (int i = 0; i < PuzzleSize; i++)
                        for (int j = 0; j < PuzzleSize; j++)
                            ShiftLeft.Table[i, j] = tmp.Table[i, j];

                    ShiftLeft.Table[tmp.EmptyPositionY, tmp.EmptyPositionX] = ShiftLeft.Table[tmp.EmptyPositionY, tmp.EmptyPositionX - 1];
                    ShiftLeft.Table[ShiftLeft.EmptyPositionY, ShiftLeft.EmptyPositionX] = PuzzleLimit;

                    CalculateHeuristic(ShiftLeft);

                    AddQueue(ShiftLeft);

                    if (ShiftLeft.Heuristic == 0)
                        return (ShiftLeft);
                }

                if (tmp.EmptyPositionX + 1 < PuzzleSize)
                {
                    PuzzleStructure ShiftRight = new PuzzleStructure();

                    ShiftRight.Parent = new PuzzleStructure();
                    ShiftRight.Parent = tmp;
                    tmp.Child.Add(ShiftRight);

                    ShiftRight.EmptyPositionX = tmp.EmptyPositionX + 1;
                    ShiftRight.EmptyPositionY = tmp.EmptyPositionY;

                    for (int i = 0; i < PuzzleSize; i++)
                        for (int j = 0; j < PuzzleSize; j++)
                            ShiftRight.Table[i, j] = tmp.Table[i, j];

                    ShiftRight.Table[tmp.EmptyPositionY, tmp.EmptyPositionX] = ShiftRight.Table[tmp.EmptyPositionY, tmp.EmptyPositionX + 1];
                    ShiftRight.Table[ShiftRight.EmptyPositionY, ShiftRight.EmptyPositionX] = PuzzleLimit;

                    CalculateHeuristic(ShiftRight);

                    AddQueue(ShiftRight);

                    if (ShiftRight.Heuristic == 0)
                        return (ShiftRight);
                }

                if (tmp.EmptyPositionY - 1 >= 0)
                {
                    PuzzleStructure ShiftDown = new PuzzleStructure();

                    ShiftDown.Parent = new PuzzleStructure();
                    ShiftDown.Parent = tmp;
                    tmp.Child.Add(ShiftDown);

                    ShiftDown.EmptyPositionX = tmp.EmptyPositionX;
                    ShiftDown.EmptyPositionY = tmp.EmptyPositionY - 1;

                    for (int i = 0; i < PuzzleSize; i++)
                        for (int j = 0; j < PuzzleSize; j++)
                            ShiftDown.Table[i, j] = tmp.Table[i, j];

                    ShiftDown.Table[tmp.EmptyPositionY, tmp.EmptyPositionX] = ShiftDown.Table[tmp.EmptyPositionY - 1, tmp.EmptyPositionX];
                    ShiftDown.Table[ShiftDown.EmptyPositionY, ShiftDown.EmptyPositionX] = PuzzleLimit;

                    CalculateHeuristic(ShiftDown);

                    AddQueue(ShiftDown);

                    if (ShiftDown.Heuristic == 0)
                        return (ShiftDown);
                }

                if (tmp.EmptyPositionY + 1 < PuzzleSize)
                {
                    PuzzleStructure ShiftUp = new PuzzleStructure();

                    ShiftUp.Parent = new PuzzleStructure();
                    ShiftUp.Parent = tmp;
                    tmp.Child.Add(ShiftUp);

                    ShiftUp.EmptyPositionX = tmp.EmptyPositionX;
                    ShiftUp.EmptyPositionY = tmp.EmptyPositionY + 1;

                    for (int i = 0; i < PuzzleSize; i++)
                        for (int j = 0; j < PuzzleSize; j++)
                            ShiftUp.Table[i, j] = tmp.Table[i, j];

                    ShiftUp.Table[tmp.EmptyPositionY, tmp.EmptyPositionX] = ShiftUp.Table[tmp.EmptyPositionY + 1, tmp.EmptyPositionX];
                    ShiftUp.Table[ShiftUp.EmptyPositionY, ShiftUp.EmptyPositionX] = PuzzleLimit;

                    CalculateHeuristic(ShiftUp);

                    AddQueue(ShiftUp);

                    if (ShiftUp.Heuristic == 0)
                        return (ShiftUp);
                }


                Sort(Queue);

                tmp = DelQueue();
            }

            return (tmp);
        }

        void BackTrace(PuzzleStructure Input)
        {

            do
            {
                BackTraceQueue.Add(Input);
                Input = Input.Parent;
            } while (Input != null);

            BackTraceQueue.Reverse();
        }

        public void GetPuzzle()
        {
            PuzzleStructure Root = new PuzzleStructure();
            Root.Parent = new PuzzleStructure();
            Root.Parent = null;

            for (int i = 0; i < PuzzleSize; i++)
                for (int j = 0; j < PuzzleSize; j++)
                {
                    Console.Write("Masukan Kondisi Awal {0},{1}: ", i, j);
                    Root.Table[i, j] = int.Parse(Console.ReadLine());

                    if (Root.Table[i, j] <= 0)
                    {
                        Root.Table[i, j] = PuzzleLimit;
                        Root.EmptyPositionY = i;
                        Root.EmptyPositionX = j;
                    }
                    else
                        Root.Table[i, j]--;
                }
            CalculateHeuristic(Root);
            Queue.Add(Root);

            BackTrace(Solve());
            PrintPuzzle();
        }

        void PrintPuzzle()
        {

            for (int counter = 0; counter < BackTraceQueue.Count; counter++)
            {
                Console.WriteLine("\nStage{0}: ", counter);

                for (int i = 0; i < PuzzleSize; i++)
                {
                    for (int j = 0; j < PuzzleSize; j++)
                    {
                        if (BackTraceQueue[counter].Table[i, j] >= PuzzleLimit)
                            Console.Write("  ");
                        else
                            Console.Write(" " + (BackTraceQueue[counter].Table[i, j] + 1).ToString());
                    }

                    Console.WriteLine("");
                }
            }
            Console.ReadLine();
        }
        
    }
}
