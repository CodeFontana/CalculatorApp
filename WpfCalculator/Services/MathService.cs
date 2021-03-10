using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCalculator.Services
{
    public class MathService : IMathService
    {
        /// <summary>
        /// Addition function.
        /// </summary>
        /// <param name="leftOperand">Lefthand value</param>
        /// <param name="rightOperand">Righthand value</param>
        /// <returns>Addtion of lefthand and righthand values.</returns>
        public double Add(double leftOperand, double rightOperand)
        {
            return leftOperand + rightOperand;
        }

        /// <summary>
        /// Subtraction function.
        /// </summary>
        /// <param name="leftOperand">Lefthand value</param>
        /// <param name="rightOperand">Righthand value</param>
        /// <returns>Subtraction of righthand value, from the lefthand value.</returns>
        public double Subtract(double leftOperand, double rightOperand)
        {
            return leftOperand - rightOperand;
        }

        /// <summary>
        /// Mulitplication function.
        /// </summary>
        /// <param name="leftOperand">Lefthand value</param>
        /// <param name="rightOperand">Righthand value</param>
        /// <returns>Multiplication of lefthand and righthand values.</returns>
        public double Multiply(double leftOperand, double rightOperand)
        {
            return leftOperand * rightOperand;
        }


        /// <summary>
        /// Division function.
        /// </summary>
        /// <param name="leftOperand">Lefthand value</param>
        /// <param name="rightOperand">Righthand value</param>
        /// <returns>Division of righthand value, from the lefthand value.</returns>
        public double Divide(double leftOperand, double rightOperand)
        {
            return leftOperand / rightOperand;
        }

        /// <summary>
        /// Percentage function.
        /// </summary>
        /// <param name="operand">Lefthand value</param>
        /// <returns>Percentage of provided value.</returns>
        public double Percent(double operand)
        {
            return operand / 100;
        }
    }
}
