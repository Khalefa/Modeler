using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Modeler
{
    class Program
    {
        static void Main(string[] args)
        {
            ChebReg.Test();
           
            double[,] A = new double[,] { { 1, 1 }, { 1, 3 }, { 1, 4 } };
            double[] B = new double[] { 6, 7, 10 };
            double[] CC = ChebReg. Solve(A, B);
            double[] CL = LinearReg.Solve(A, B);
            double[] E1 = Regression.CalcError(A, CC, B);
            double[] E2 = Regression.CalcError(A, CL, B);
        }
    }
}
