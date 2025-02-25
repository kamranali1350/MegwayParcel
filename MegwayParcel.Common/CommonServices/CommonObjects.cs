using System;
using Microsoft.AspNetCore.Http;

namespace MegwayParcel.Common.CommonServices
{
    public sealed class CommonObjects
    {
        #region Fields

        private static CommonObjects _instance;
        private static readonly object _lock = new object();
        private ApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #endregion Fields

        #region Constructors

        private CommonObjects(IHttpContextAccessor httpContextAccessor, string apiBaseUrl, string apiUser = null, string apiToken = null)
        {
            _httpContextAccessor = httpContextAccessor;
            // Initialize ApiClient with the provided base URL, api-user, and api-token
            _apiClient = new ApiClient(apiBaseUrl, apiUser, apiToken);
        }

        // Private constructor for singleton pattern without DI
        private CommonObjects()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static CommonObjects Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("CommonObjects has not been configured. Call ConfigureInstance first.");
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the API client.
        /// </summary>
        public ApiClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Configures the singleton instance with the provided parameters.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="apiBaseUrl">The base URL for the ApiClient.</param>
        /// <param name="apiUser">The optional API user for the ApiClient.</param>
        /// <param name="apiToken">The optional API token for the ApiClient.</param>
        public static void ConfigureInstance(IHttpContextAccessor httpContextAccessor, string apiBaseUrl, string apiToken = null, string apiUser = null)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CommonObjects(httpContextAccessor, apiBaseUrl, apiToken, apiUser);
                    }
                }
            }
        }

        /// <summary>
        /// Configures the ApiClient with a new base URL, api-user, and optional token.
        /// </summary>
        /// <param name="apiBaseUrl">The base URL for the ApiClient.</param>
        /// <param name="apiUser">The optional API user for the ApiClient.</param>
        /// <param name="apiToken">The optional API token for the ApiClient.</param>
        public void ConfigureApiClient(string apiBaseUrl,  string apiToken = null,string apiUser = null)
        {
            _apiClient = new ApiClient(apiBaseUrl, apiToken,apiUser );
        }

        #endregion Methods
    }
}
