using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        private Matrix resultMatrix;

        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            this.resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, m1.RowCount, (i) => Multiply(i, m1, m2));

            // todo: feel free to add your code here
            return this.resultMatrix;
        }

        private void Multiply(long i, IMatrix m1, IMatrix m2)
		{
            for (byte j = 0; j < m2.ColCount; j++)
            {
                long sum = 0;
                for (byte k = 0; k < m1.ColCount; k++)
                {
                    sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                }

                this.resultMatrix.SetElement(i, j, sum);
            }
        }
    }
}
