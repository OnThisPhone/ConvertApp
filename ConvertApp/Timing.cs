using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***
 * Has some basic timing functions to limit frequency of updates to any function.
 * Useful in async and separate threads.
 */
namespace ConvertApp
{
    internal class Timing
    {
        //Variables to be used
        public static long FREQ_DEFAULT = 800;      //The default frequency
        public long updateTimeNow;                  //Keeps track of last time an update happened
        public long updateTimeElapsed;              //A variable used for checking against the now and frequency
        public long updateTimeFrequency;            //This keeps track of how often an update happens.

        //Constructors
        public Timing()
        {
            this.updateTimeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.updateTimeElapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.updateTimeFrequency = FREQ_DEFAULT;
        }
        public Timing(long updateTimeFrequency)
        {
            this.updateTimeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.updateTimeElapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.updateTimeFrequency = updateTimeFrequency;
        }

        /// <summary>
        /// Checks when if timing has reached the frequency variable value.
        /// </summary>
        /// <returns>Returns true if an update should happen. False otherwise</returns>
        public bool Check()
        {
            updateTimeElapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if(updateTimeElapsed > updateTimeFrequency + updateTimeNow)
            {
                updateTimeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}
