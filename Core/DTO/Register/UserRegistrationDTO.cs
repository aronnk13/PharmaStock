namespace PharmaStock.Core.DTO.Register
{
    /// <summary>
    /// Data Transfer Object for creating or updating a user.
    /// Contains basic user details such as username, role, email, and phone.
    /// </summary>
    public class UpsertUserDTO
    {
        /// <summary>
        /// The ID of the user (Used for updates, send 0 for creation).
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The unique username chosen by the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The role identifier associated with the user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The ID of the admin performing this action.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Whether the user account is active.
        /// </summary>
        public bool StatusId { get; set; } = true;

        /// <summary>
        /// Flag indicating whether this is a new user creation (true) or an update (false).
        /// </summary>
        public bool IsCreate { get; set; }
    }
}