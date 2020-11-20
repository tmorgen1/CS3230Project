using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.Controller
{
    /// <summary>
    /// Handles holding currently logged in user.
    /// </summary>
    public static class LoginController
    {
        /// <summary>
        /// Gets or sets the current nurse.
        /// </summary>
        /// <value>
        /// The current nurse.
        /// </value>
        public static Nurse CurrentNurse { get; set; }

        /// <summary>
        /// Gets or sets the current administrator.
        /// </summary>
        /// <value>
        /// The current administrator.
        /// </value>
        public static Administrator CurrentAdministrator { get; set; }
    }
}
