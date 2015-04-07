using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeler
{
    class Regression
    {
       
        public double[] CalcError(double[,] X, double[] W, double[] C, double[] Y)
        {
            int M = Y.Length;             // M = Number of data points
            int N = X.Length / M;         // N = Number of linear terms

            double[] Ycalc = new double[M];
            double[] E = new double[M];
            for (int k = 0; k < M; k++)
            {
                Ycalc[k] = 0;
                for (int i = 0; i < N; i++)
                    Ycalc[k] = Ycalc[k] + C[i] * X[i, k];
                E[k] = Ycalc[k] - Y[k];

            }
            return E;
        }
    }
}
