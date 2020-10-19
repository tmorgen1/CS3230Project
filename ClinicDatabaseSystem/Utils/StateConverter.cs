using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Enums;

namespace ClinicDatabaseSystem.Utils
{
    public static class StateConverter
    {
        public static States ConvertToState(string state)
        {
            foreach (States currentState in Enum.GetValues(typeof(States)))
            {
                if (currentState.ToString().Equals(state))
                {
                    return currentState;
                }
            }

            return default;
        }
    }
}
