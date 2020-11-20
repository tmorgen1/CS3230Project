using ClinicDatabaseSystem.Enums;
using System;

namespace ClinicDatabaseSystem.Utils
{
    /// <summary>
    /// Converts states strings to enum.
    /// </summary>
    public static class StateConverter
    {
        /// <summary>
        /// Converts to state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>State enum from string</returns>
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
