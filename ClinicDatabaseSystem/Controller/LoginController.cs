using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.Controller
{
    /// <summary>
    /// Handles holding currently logged in user.
    /// </summary>
    public static class LoginController
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public static Nurse CurrentUser { get; set; }
    }
}
