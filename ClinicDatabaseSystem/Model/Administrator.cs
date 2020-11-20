using System;

namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding an administrator.
    /// </summary>
    public class Administrator
    {
        /// <summary>
        /// Gets or sets the admin identifier.
        /// </summary>
        /// <value>
        /// The admin identifier.
        /// </value>
        public int AdminId { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the do b.
        /// </summary>
        /// <value>
        /// The do b.
        /// </value>
        public DateTime DoB { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Administrator"/> class.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="address">The address.</param>
        /// <param name="zip">The zip.</param>
        public Administrator(int adminId, string lastName, string firstName, DateTime dob, string phoneNumber, string accountId, string address, string zip)
        {
            this.AdminId = adminId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.DoB = dob;
            this.PhoneNumber = phoneNumber;
            this.AccountId = accountId;
            this.Address = address;
            this.Zip = zip;
        }
    }
}
