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
    public partial class HtmlContentService : IHtmlContentService
    {
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Cms.HtmlContent Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Cms.HtmlContentService HtmlContentService = new Rock.Services.Cms.HtmlContentService();
                Rock.Models.Cms.HtmlContent HtmlContent = HtmlContentService.Get( int.Parse( id ) );
                if ( HtmlContent.Authorized( "View", currentUser ) )
                    return HtmlContent;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateHtmlContent( string id, Rock.Models.Cms.HtmlContent HtmlContent )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.HtmlContentService HtmlContentService = new Rock.Services.Cms.HtmlContentService();
                Rock.Models.Cms.HtmlContent existingHtmlContent = HtmlContentService.Get( int.Parse( id ) );
                if ( existingHtmlContent.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingHtmlContent).CurrentValues.SetValues(HtmlContent);
                    HtmlContentService.Save( existingHtmlContent, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateHtmlContent( Rock.Models.Cms.HtmlContent HtmlContent )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.HtmlContentService HtmlContentService = new Rock.Services.Cms.HtmlContentService();
                HtmlContentService.Add( HtmlContent, currentUser.PersonId() );
                HtmlContentService.Save( HtmlContent, currentUser.PersonId() );
            }
        }

		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteHtmlContent( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.HtmlContentService HtmlContentService = new Rock.Services.Cms.HtmlContentService();
                Rock.Models.Cms.HtmlContent HtmlContent = HtmlContentService.Get( int.Parse( id ) );
                if ( HtmlContent.Authorized( "Edit", currentUser ) )
                {
                    HtmlContentService.Delete( HtmlContent, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
