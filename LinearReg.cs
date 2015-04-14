using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Modeler
{
    class LinearReg : Regression
    {

        static public double[] Regress(double[] Y, double[,] X, double[] W)
        {
            int M = Y.Length;             // M = Number of data points
            int N = X.Length / M;         // N = Number of linear terms
            int NDF = M - N;              // Degrees of freedom
            // If not enough data, don't attempt regression
            if (NDF < 1)
            {
                return null;
            }
            double[,] V = new double[N, N];
            double[] C = new double[N];

            double[] B = new double[N];   // Vector for LSQ

            // Clear the matrices to start out
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    V[i, j] = 0;

            // Form Least Squares Matrix
            if (W != null)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        V[i, j] = 0;
                        for (int k = 0; k < M; k++)
                            V[i, j] = V[i, j] + W[k] * X[i, k] * X[j, k];
                    }
                    B[i] = 0;
                    for (int k = 0; k < M; k++)
                        B[i] = B[i] + W[k] * X[i, k] * Y[k];
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        V[i, j] = 0;
                        for (int k = 0; k < M; k++)
                            V[i, j] = V[i, j] + X[i, k] * X[j, k];
                    }
                    B[i] = 0;
                    for (int k = 0; k < M; k++)
                        B[i] = B[i] + X[i, k] * Y[k];
                }
            }
            // V now contains the raw least squares matrix
            if (!SymmetricMatrixInvert(V))
            {
                return null;
            }
            // V now contains the inverted least square matrix
            // Matrix multpily to get coefficients C = VB
            for (int i = 0; i < N; i++)
            {
                C[i] = 0;
                for (int j = 0; j < N; j++)
                    C[i] = C[i] + V[i, j] * B[j];
            }

            return C;
        }
        //Y is actual

        static bool SymmetricMatrixInvert(double[,] V)
        {
            int N = (int)Math.Sqrt(V.Length);
            double[] t = new double[N];
            double[] Q = new double[N];
            double[] R = new double[N];
            double AB;
            int K, L, M;

            // Invert a symetric matrix in V
            for (M = 0; M < N; M++)
                R[M] = 1;
            K = 0;
            for (M = 0; M < N; M++)
            {
                double Big = 0;
                for (L = 0; L < N; L++)
                {
                    AB = Math.Abs(V[L, L]);
                    if ((AB > Big) && (R[L] != 0))
                    {
                        Big = AB;
                        K = L;
                    }
                }
                if (Big == 0)
                {
                    return false;
                }
                R[K] = 0;
                Q[K] = 1 / V[K, K];
                t[K] = 1;
                V[K, K] = 0;
                if (K != 0)
                {
                    for (L = 0; L < K; L++)
                    {
                        t[L] = V[L, K];
                        if (R[L] == 0)
                            Q[L] = V[L, K] * Q[K];
                        else
                            Q[L] = -V[L, K] * Q[K];
                        V[L, K] = 0;
                    }
                }
                if ((K + 1) < N)
                {
                    for (L = K + 1; L < N; L++)
                    {
                        if (R[L] != 0)
                            t[L] = V[K, L];
                        else
                            t[L] = -V[K, L];
                        Q[L] = -V[K, L] * Q[K];
                        V[K, L] = 0;
                    }
                }
                for (L = 0; L < N; L++)
                    for (K = L; K < N; K++)
                        V[L, K] = V[L, K] + t[L] * Q[K];
            }
            M = N;
            L = N - 1;
            for (K = 1; K < N; K++)
            {
                M = M - 1;
                L = L - 1;
                for (int J = 0; J <= L; J++)
                    V[M, J] = V[J, M];
            }
            return true;
        }
        static public double[] Solve(double[] Y)
        {
            return Solve(Regression.getArr(Y.Length), Y);
        }
        static public double[] CalcError(double []Y)
        {
            double [,]coff=Regression.getArr(Y.Length);
            double []p=Solve(coff, Y);
            if (Math.Abs(p[1]) < 0.1) return null;
            double []E= Regression.CalcError(coff, p, Y);
            double deltaY = Y.Max() - Y.Min();
            double deltaE = E.Max() - E.Min();
            if (Math.Abs(deltaE) > Math.Abs(deltaY)) return null;
            return E;
            
        }
        static public double[] Solve(double[,] X, double[] Y)
        {
            int d0 = X.GetLength(0);
            int d1 = X.GetLength(1);

            double[,] Xt = new double[X.GetLength(1), X.GetLength(0)];

            double[] W = new double[Y.Length];
            for (int i = 0; i < d0; i++)
                for (int j = 0; j < d1; j++)
                    Xt[j, i] = X[i, j];

            return Regress(Y, Xt, null);

        }

    }
}
