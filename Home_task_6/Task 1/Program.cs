using System.Text;

namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QuadroMatrix<int> quadroMatrix = new QuadroMatrix<int>("C:\\Users\\nena\\Desktop\\Home_task_6\\Task 1\\Data.txt");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < quadroMatrix.Size; ++i)
            {
                for (int j = 0; j < quadroMatrix.Size; ++j)
                {
                    sb.Append(quadroMatrix.Matrix[i, j] + "\t");
                }
                sb.Append("\n");
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine(quadroMatrix);
        }

    }
}