using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaximalSquare
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] matrix = { "10100", "10111", "11111", "10010" };
            Console.WriteLine(MaximalSquare(matrix));
            Console.ReadLine();
        }
        public static int MaximalSquare(string[] matrix)
        {
            int currentLevel = 0;
            int nextLevel = 1;
            foreach (string s in matrix)
            {
                if (s.Contains(nextLevel.ToString()))
                {
                    currentLevel = nextLevel;
                    nextLevel++;
                    break;
                }
            }
            if (currentLevel == 0) return 0;

            int rows = matrix.Length;
            int collumns = matrix[0].Length;
            int maxLevel = Math.Min(rows, collumns);
            Dictionary<int, List<string>> consistentSections = new Dictionary<int, List<string>>();
            for (int r = 0; r < rows; r++)
            {
                List<string> elements = new List<string>();
                string toAdd = "";
                for (int c = 0; c < matrix[r].Length; c++)
                {
                    if (matrix[r][c] != '0')
                    {
                        toAdd = c.ToString();
                    }
                    else toAdd = "n" + c;
                    elements.Add(toAdd);
                }                
                if (!elements.All(x => x.Contains('n')))
                {
                    consistentSections.Add(r, elements);
                }
            }
            while (currentLevel < maxLevel)
            {
                List<int[]> rowsArray = GetNConsistentValues(consistentSections.Keys.ToList(), nextLevel);
                if (rowsArray.Count() == 0) return (int)Math.Pow(currentLevel,2);
                foreach (int[] array in rowsArray)
                {
                    List<string> intersection = consistentSections[array[0]].Intersect(consistentSections[array[1]]).ToList();
                    List<int> mediator = new List<int>();
                    int[] squareMatrix;
                    for (int row = 1; row < array.Length - 1; row++)
                    {
                        intersection = intersection.Intersect(consistentSections[array[row + 1]]).ToList();
                    }
                    mediator = intersection.Where(x => !x.Contains('n')).ToList().ConvertAll(x => int.Parse(x)).ToList();
                    List<int[]> matricesArray = GetNConsistentValues(mediator, nextLevel);
                    squareMatrix = matricesArray.FirstOrDefault(x => x.Length == nextLevel)?.ToArray();
               
                    if (squareMatrix != null)
                    {
                        currentLevel = nextLevel;
                        break;
                    }
                }
                if (nextLevel > currentLevel)
                {
                    return (int)Math.Pow(currentLevel,2);
                }
                nextLevel++;
            }
            return (int)Math.Pow(currentLevel, 2);
        }       

        public static List<int[]> GetNConsistentValues(List<int> values, int consistentNum)
        {
            List<int[]> rowsSections = new List<int[]>();
            if (values.Count() < consistentNum) return rowsSections;
            int reserve = consistentNum - 1;
            int sectionsQuan = values.Count() - reserve;            
            for (int rowIndex=0;rowIndex<sectionsQuan;rowIndex++)
            {
                int counter = consistentNum - 1;
                int currentIndex = rowIndex;
                List<int> currentSection = new List<int>();
                while (counter != 0)
                {
                    if (values[currentIndex] +1 == values[currentIndex + 1])
                    {
                        currentSection.Add(values[currentIndex]);
                    }
                    else
                    {
                        currentIndex++;
                        break;
                    }
                    counter--;                    
                    if (counter == 0)
                        currentSection.Add(values[currentIndex + 1]);
                    currentIndex++;
                }
                if (currentSection.Count()==consistentNum)
                    rowsSections.Add(currentSection.ToArray());
            }
            return rowsSections;
        }
    }
}
