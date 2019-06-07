namespace Atomia.Store.Core
{
    /// <summary>
    /// Provides the informations about packages and their configuration
    /// </summary>
    public interface IPackageProvider
    {
        /// <summary>
        /// Checks if multi package is enabled
        /// </summary>
        /// <returns></returns>
        bool IsMultiPackageEnabled();
    }
}
