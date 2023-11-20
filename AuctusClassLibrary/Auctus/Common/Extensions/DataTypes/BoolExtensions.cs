namespace Auctus.DataMiner.Library.Common.Type
{
    /// <summary>
    ///   Extension methods for the bool type.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>Converts the boolean representation to an integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        ///   0 if <c>false</c>.
        ///   <br></br>
        ///   1 if <c>true</c>.
        /// </returns>
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}