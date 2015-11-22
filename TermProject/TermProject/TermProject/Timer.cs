using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public static class Timer
    {
        private static Dictionary<object, double> Times = new Dictionary<object, double>();

        /// <summary>
        /// Timer method to execute tasks on the main thread without sleeping or using C#6 features
        /// </summary>
        /// <param name="timeLock">object to bind to</param>
        /// <param name="currentTime">current time elapsed</param>
        /// <param name="waitTime">total time to wait for</param>
        /// <returns></returns>
        public static bool IsTimeYet(object timeLock, double currentTime, double? waitTime = null)
        {
            bool result = false;
            if (Times.ContainsKey(timeLock))
            {
                result = Times[timeLock] < currentTime;
            }
            else if (waitTime.HasValue)
            {
                Times.Add(timeLock, currentTime + waitTime.Value);
            }

            if (result)
            {
                Times.Remove(timeLock);
            }
            return result;
        }
    }
}
