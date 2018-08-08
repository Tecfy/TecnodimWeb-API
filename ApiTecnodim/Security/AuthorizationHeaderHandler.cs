using DataEF.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tecnodim.Properties;

namespace Tecnodim.Security
{
    /// <summary>
    /// Authorization for web API class.
    /// </summary>
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        #region Send method.

        /// <summary>
        /// Send method.
        /// </summary>
        /// <param name="request">Request parameter</param>
        /// <param name="cancellationToken">Cancellation token parameter</param>
        /// <returns>Return HTTP response.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Initialization
            IEnumerable<string> apiKeyHeaderValues = null;
            IEnumerable<string> apiUserHeaderValues = null;

            // Verification
            if (request.Headers.TryGetValues(Resources.API_KEY_HEADER, out apiKeyHeaderValues) &&
                request.Headers.TryGetValues(Resources.API_USER_HEADER, out apiUserHeaderValues))
            {
                Guid apiKeyHeaderValue = new Guid(apiKeyHeaderValues.First());
                string apiUserHeaderValue = apiUserHeaderValues.First();

                ApiUsers apiUser = new ApiUsers();

                using (var context = new DBContext())
                {
                    apiUser = context.ApiUsers.Where(x => x.Active == true
                                                        && x.DeletedDate == null
                                                        && apiKeyHeaderValue.Equals(x.Hash)
                                                        && apiUserHeaderValue.Equals(x.User)
                                                    ).FirstOrDefault();

                    if (apiUser != null)
                    {
                        apiUser.LastLogin = DateTime.Now;

                        context.Entry(apiUser).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                // Verification
                if (apiUser != null)
                {
                    // Setting
                    var identity = new GenericIdentity(apiKeyHeaderValue.ToString());
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
            }

            // Info
            return base.SendAsync(request, cancellationToken);
        }

        #endregion

        #region Set principal method.

        /// <summary>
        /// Set principal method.
        /// </summary>
        /// <param name="principal">Principal parameter</param>
        private static void SetPrincipal(IPrincipal principal)
        {
            // setting.
            Thread.CurrentPrincipal = principal;

            // Verification.
            if (HttpContext.Current != null)
            {
                // Setting.
                HttpContext.Current.User = principal;
            }
        }

        #endregion
    }
}