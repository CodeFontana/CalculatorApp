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
        /// <param name="leftHand">Lefthand value</param>
        /// <param name="rightHand">Righthand value</param>
        /// <returns>Addtion of lefthand and righthand values.</returns>
        public double Add(double leftHand, double rightHand)
        {
            return leftHand + rightHand;
        }

        /// <summary>
        /// Subtraction function.
        /// </summary>
        /// <param name="leftHand">Lefthand value</param>
        /// <param name="rightHand">Righthand value</param>
        /// <returns>Subtraction of righthand value, from the lefthand value.</returns>
        public double Subtract(double leftHand, double rightHand)
        {
            return leftHand - rightHand;
        }

        /// <summary>
        /// Mulitplication function.
        /// </summary>
        /// <param name="leftHand">Lefthand value</param>
        /// <param name="rightHand">Righthand value</param>
        /// <returns>Multiplication of lefthand and righthand values.</returns>
        public double Multiply(double leftHand, double rightHand)
        {
            return leftHand * rightHand;
        }


        /// <summary>
        /// Division function.
        /// </summary>
        /// <param name="leftHand">Lefthand value</param>
        /// <param name="rightHand">Righthand value</param>
        /// <returns>Division of righthand value, from the lefthand value.</returns>
        public double Divide(double leftHand, double rightHand)
        {
            return leftHand / rightHand;
        }

        /// <summary>
        /// Percentage function.
        /// </summary>
        /// <param name="value">Lefthand value</param>
        /// <returns>Percentage of provided value.</returns>
        public double Percent(double value)
        {
            return value / 100;
        }
    }
}
