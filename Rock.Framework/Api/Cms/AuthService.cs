//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using Rock.Cms.Security;

namespace Rock.Api.Cms
{
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class AuthService : IAuthService
    {
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Cms.Auth Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Cms.AuthService AuthService = new Rock.Services.Cms.AuthService();
                Rock.Models.Cms.Auth Auth = AuthService.Get( int.Parse( id ) );
                if ( Auth.Authorized( "View", currentUser ) )
                    return Auth;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateAuth( string id, Rock.Models.Cms.Auth Auth )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.AuthService AuthService = new Rock.Services.Cms.AuthService();
                Rock.Models.Cms.Auth existingAuth = AuthService.Get( int.Parse( id ) );
                if ( existingAuth.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingAuth).CurrentValues.SetValues(Auth);
                    AuthService.Save( existingAuth, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateAuth( Rock.Models.Cms.Auth Auth )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.AuthService AuthService = new Rock.Services.Cms.AuthService();
                AuthService.Add( Auth, currentUser.PersonId() );
                AuthService.Save( Auth, currentUser.PersonId() );
            }
        }

		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteAuth( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.AuthService AuthService = new Rock.Services.Cms.AuthService();
                Rock.Models.Cms.Auth Auth = AuthService.Get( int.Parse( id ) );
                if ( Auth.Authorized( "Edit", currentUser ) )
                {
                    AuthService.Delete( Auth, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
