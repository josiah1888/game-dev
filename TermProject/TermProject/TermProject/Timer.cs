using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Timer
    {
        private double? WaitTime;

        public Timer()
        {

        }

        /// <summary>
        /// Timer method to execute tasks on the main thread without sleeping or using C#6 features
        /// </summary>
        /// <param name="currentElapsed">current time elapsed</param>
        /// <param name="waitTime">total time to wait for</param>
        /// <returns></returns>
        public bool IsTimeYet(double currentElapsed, double waitTime)
        {
            bool result = false;

            if (!this.WaitTime.HasValue)
            {
                this.WaitTime = currentElapsed + waitTime;
            }
            else if (currentElapsed > this.WaitTime)
            {
                this.WaitTime = null;
                result = true;
            }

            return result;
        }
    }
}
