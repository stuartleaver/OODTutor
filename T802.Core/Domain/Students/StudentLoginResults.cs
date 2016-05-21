namespace T802.Core.Domain.Students
{
    /// <summary>
    /// Represents the customer login result enumeration
    /// </summary>
    public enum StudentLoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,
        /// <summary>
        /// Student dies not exist (username)
        /// </summary>
        StudentNotExist = 2,
        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,
        /// <summary>
        /// Student has been deleted 
        /// </summary>
        Deleted = 5,
        /// <summary>
        /// Student not registered 
        /// </summary>
        NotRegistered = 6,
    }
}
