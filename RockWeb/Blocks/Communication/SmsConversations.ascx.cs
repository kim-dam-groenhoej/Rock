﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Communication
{
    [DisplayName( "SMS Conversations" )]
    [Category( "Communication" )]
    [Description( "Block for having SMS Conversations between an SMS enabled phone and a Rock SMS Phone number that has 'Enable Mobile Conversations' set to false." )]
    [DefinedValueField( definedTypeGuid: Rock.SystemGuid.DefinedType.COMMUNICATION_SMS_FROM,
        name: "Allowed SMS Numbers",
        description: "Set the allowed FROM numbers to appear when in SMS mode (if none are selected all numbers will be included). ",
        required: false,
        allowMultiple: true,
        order: 1,
        key: "AllowedSMSNumbers" )]
    [BooleanField( "Show only personal SMS number",
        description: "Only SMS Numbers tied to the current individual will be shown. Those with ADMIN rights will see all SMS Numbers.",
        defaultValue: false,
        order: 2,
        key: "ShowOnlyPersonalSmsNumber" )]
    [BooleanField( "Hide personal SMS numbers",
        description: "Only SMS Numbers that are not associated with a person. The numbers without a 'ResponseRecipient' attribute value.",
        defaultValue: false,
        order: 3,
        key: "HidePersonalSmsNumbers" )]
    [BooleanField( "Enable SMS Send",
        description: "Allow SMS messages to be sent from the block.",
        defaultValue: true,
        order: 4,
        key: "EnableSmsSend" )]
    [IntegerField( name: "Show Conversations From Months Ago",
        description: "Limits the conversations shown in the left pane to those of X months ago or newer. This does not affect the actual messages shown on the right.",
        defaultValue: 6,
        order: 5,
        key: "ShowConversationsFromMonthsAgo" )]
    [CodeEditorField( "Person Info Lava Template",
        description: "A Lava template to display person information about the selected Communication Recipient.",
        defaultValue: "{{ Person.FullName }}",
        mode: CodeEditorMode.Lava,
        theme: CodeEditorTheme.Rock,
        height: 300,
        required: false,
        order: 6,
        key: "PersonInfoLavaTemplate" )]

    // Start here to build the person description lit field after selecting recipient.
    public partial class SmsConversations : RockBlock
    {
        #region Control Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            HtmlMeta preventPhoneMetaTag = new HtmlMeta
            {
                Name = "format-detection",
                Content = "telephone=no"
            };

            RockPage.AddMetaTag( this.Page, preventPhoneMetaTag );

            this.BlockUpdated += Block_BlockUpdated;

            btnCreateNewMessage.Visible = this.GetAttributeValue( "EnableSmsSend" ).AsBoolean();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            string postbackArgs = Request.Params["__EVENTARGUMENT"] ?? string.Empty;

            nbAddPerson.Visible = false;

            if ( !IsPostBack )
            {
                if ( LoadPhoneNumbers() )
                {
                    nbNoNumbers.Visible = false;
                    divMain.Visible = true;
                    LoadResponseListing();
                }
                else
                {
                    nbNoNumbers.Visible = true;
                    divMain.Visible = false;
                }
            }
        }

        #endregion Control Overrides

        #region private/protected Methods

        /// <summary>
        /// Loads the phone numbers.
        /// </summary>
        /// <returns></returns>
        private bool LoadPhoneNumbers()
        {
            // First load up all of the available numbers
            var smsNumbers = DefinedTypeCache.Get( Rock.SystemGuid.DefinedType.COMMUNICATION_SMS_FROM.AsGuid() ).DefinedValues;

            var selectedNumberGuids = GetAttributeValue( "AllowedSMSNumbers" ).SplitDelimitedValues( true ).AsGuidList();
            if ( selectedNumberGuids.Any() )
            {
                smsNumbers = smsNumbers.Where( v => selectedNumberGuids.Contains( v.Guid ) ).ToList();
            }

            // filter personal numbers (any that have a response recipient) if the hide personal option is enabled
            if ( GetAttributeValue( "HidePersonalSmsNumbers" ).AsBoolean() )
            {
                smsNumbers = smsNumbers.Where( v => v.GetAttributeValue( "ResponseRecipient" ).IsNullOrWhiteSpace() ).ToList();
            }

            // Show only numbers 'tied to the current' individual...unless they have 'Admin rights'.
            if ( GetAttributeValue( "ShowOnlyPersonalSmsNumber" ).AsBoolean() && !IsUserAuthorized( Authorization.ADMINISTRATE ) )
            {
                smsNumbers = smsNumbers.Where( v => CurrentPerson.Aliases.Any( a => a.Guid == v.GetAttributeValue( "ResponseRecipient" ).AsGuid() ) ).ToList();
            }

            if ( smsNumbers.Any() )
            {
                var smsDetails = smsNumbers.Select( v => new
                {
                    v.Id,
                    Description = string.IsNullOrWhiteSpace( v.Description )
                    ? PhoneNumber.FormattedNumber( string.Empty, v.Value.Replace( "+", string.Empty ) )
                    : v.Description.LeftWithEllipsis( 25 ),
                } );

                ddlSmsNumbers.DataSource = smsDetails;
                ddlSmsNumbers.Visible = smsNumbers.Count() > 1;
                ddlSmsNumbers.DataValueField = "Id";
                ddlSmsNumbers.DataTextField = "Description";
                ddlSmsNumbers.DataBind();

                string keyPrefix = string.Format( "sms-conversations-{0}-", this.BlockId );

                string smsNumberUserPref = this.GetUserPreference( keyPrefix + "smsNumber" ) ?? string.Empty;

                if ( smsNumberUserPref.IsNotNullOrWhiteSpace() )
                {
                    // Don't try to set the selected value unless you are sure it's in the list of items.
                    if ( ddlSmsNumbers.Items.FindByValue( smsNumberUserPref ) != null )
                    {
                        ddlSmsNumbers.SelectedValue = smsNumberUserPref;
                    }
                }

                hlSmsNumber.Visible = smsNumbers.Count() == 1;
                hlSmsNumber.Text = smsDetails.Select( v => v.Description ).FirstOrDefault();
                hfSmsNumber.SetValue( smsNumbers.Count() > 1 ? ddlSmsNumbers.SelectedValue.AsInteger() : smsDetails.Select( v => v.Id ).FirstOrDefault() );

                tglShowRead.Checked = this.GetUserPreference( keyPrefix + "showRead" ).AsBooleanOrNull() ?? true;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the response listing.
        /// </summary>
        private void LoadResponseListing()
        {
            // NOTE: The FromPersonAliasId is the person who sent a text from a mobile device to Rock.
            // This person is also referred to as the Recipient because they are responding to a
            // communication from Rock. Restated the response is from the recipient of a communication.

            // This is the person lava field, we want to clear it because reloading this list will deselect the user.
            litSelectedRecipientDescription.Text = string.Empty;
            hfSelectedRecipientPersonAliasId.Value = string.Empty;
            hfSelectedMessageKey.Value = string.Empty;
            tbNewMessage.Visible = false;
            btnSend.Visible = false;

            int? smsPhoneDefinedValueId = hfSmsNumber.ValueAsInt();
            if ( smsPhoneDefinedValueId == default( int ) )
            {
                return;
            }

            using ( var rockContext = new RockContext() )
            {
                var communicationResponseService = new CommunicationResponseService( rockContext );

                DataSet responses = null;
                int months = GetAttributeValue( "ShowConversationsFromMonthsAgo" ).AsInteger();

                if ( tglShowRead.Checked )
                {
                    responses = communicationResponseService.GetCommunicationsAndResponseRecipients( smsPhoneDefinedValueId.Value, months );
                }
                else
                {
                    // Since communications sent from Rock are always considered "Read" we don't need them included in the list if we are not showing "Read" messages.
                    responses = communicationResponseService.GetResponseRecipients( smsPhoneDefinedValueId.Value, false, months );
                }

                var responseListItems = responses.Tables[0].AsEnumerable()
                    .Select( r => new ResponseListItem
                    {
                        RecipientPersonAliasId = r.Field<int?>( "FromPersonAliasId" ),
                        MessageKey = r.Field<string>( "MessageKey" ),
                        FullName = r.Field<string>( "FullName" ),
                        CreatedDateTime = r.Field<DateTime>( "CreatedDateTime" ),
                        HumanizedCreatedDateTime = HumanizeDateTime( r.Field<DateTime>( "CreatedDateTime" ) ),
                        SMSMessage = r.Field<string>( "SMSMessage" ),
                        IsRead = r.Field<bool>( "IsRead" )
                    } )
                    .ToList();

                // don't display conversations if we're rebinding the recipient list
                rptConversation.Visible = false;
                gRecipients.DataSource = responseListItems;
                gRecipients.DataBind();
            }
        }

        /// <summary>
        /// Loads the responses for recipient.
        /// </summary>
        /// <param name="recipientId">The recipient identifier.</param>
        /// <returns></returns>
        private string LoadResponsesForRecipient( int recipientId )
        {
            int? smsPhoneDefinedValueId = hfSmsNumber.ValueAsInt();

            if ( smsPhoneDefinedValueId == default( int ) )
            {
                return string.Empty;
            }

            var communicationResponseService = new CommunicationResponseService( new RockContext() );
            var responses = communicationResponseService.GetConversation( recipientId, smsPhoneDefinedValueId.Value );

            BindConversationRepeater( responses );

            var list = responses.Tables[0].AsEnumerable();
            if ( list != null && list.Count() > 0 )
            {
                DataRow row = list.Last();
                return row["SMSMessage"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Loads the responses for recipient.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        /// <returns></returns>
        private string LoadResponsesForRecipient( string messageKey )
        {
            int? smsPhoneDefinedValueId = hfSmsNumber.ValueAsInt();

            if ( smsPhoneDefinedValueId == default( int ) )
            {
                return string.Empty;
            }

            var communicationResponseService = new CommunicationResponseService( new RockContext() );
            var responses = communicationResponseService.GetConversation( messageKey, smsPhoneDefinedValueId.Value );

            BindConversationRepeater( responses );

            DataRow row = responses.Tables[0].AsEnumerable().Last();
            return row["SMSMessage"].ToString();
        }

        /// <summary>
        /// Binds the conversation repeater.
        /// </summary>
        /// <param name="responses">The responses.</param>
        private void BindConversationRepeater( DataSet responses )
        {
            var communicationItems = responses.Tables[0].AsEnumerable()
                .Select( r => new ResponseListItem
                {
                    RecipientPersonAliasId = r.Field<int?>( "FromPersonAliasId" ),
                    MessageKey = r.Field<string>( "MessageKey" ),
                    FullName = r.Field<string>( "FullName" ),
                    CreatedDateTime = r.Field<DateTime>( "CreatedDateTime" ),
                    HumanizedCreatedDateTime = HumanizeDateTime( r.Field<DateTime>( "CreatedDateTime" ) ),
                    SMSMessage = r.Field<string>( "SMSMessage" ),
                    IsRead = r.Field<bool>( "IsRead" )
                } )
                .ToList();

            rptConversation.Visible = true;
            rptConversation.DataSource = communicationItems;
            rptConversation.DataBind();
        }

        /// <summary>
        /// Humanizes the date time to relative if not on the same day and short time if it is.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        private string HumanizeDateTime( DateTime? dateTime )
        {
            if ( dateTime == null )
            {
                return string.Empty;
            }

            DateTime dtCompare = RockDateTime.Now;

            if ( dtCompare.Date == dateTime.Value.Date )
            {
                return dateTime.Value.ToShortTimeString();
            }

            // Method Name "Truncate" collision between Humanizer and Rock ExtensionMethods so have to call as a static with full name.
            return Humanizer.DateHumanizeExtensions.Humanize( dateTime, true, dtCompare, null );
        }

        /// <summary>
        /// Populates the person lava.
        /// </summary>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        private void PopulatePersonLava( RowEventArgs e )
        {
            var hfRecipientPersonAliasId = ( HiddenField ) e.Row.FindControl( "hfRecipientPersonAliasId" );
            int? recipientPersonAliasId = hfSelectedRecipientPersonAliasId.Value.AsIntegerOrNull();

            var hfMessageKey = ( HiddenField ) e.Row.FindControl( "hfMessageKey" );
            var lblName = ( Label ) e.Row.FindControl( "lblName" );
            string html = lblName.Text;
            string unknownPerson = " (Unknown Person)";
            var lava = GetAttributeValue( "PersonInfoLavaTemplate" );

            if ( !recipientPersonAliasId.HasValue || recipientPersonAliasId.Value == -1 )
            {
                // We don't have a person to do the lava merge so just display the formatted phone number
                html = PhoneNumber.FormattedNumber( string.Empty, hfMessageKey.Value ) + unknownPerson;
                litSelectedRecipientDescription.Text = html;
            }
            else
            {
                // Merge the person and lava
                using ( var rockContext = new RockContext() )
                {
                    var personAliasService = new PersonAliasService( rockContext );
                    var recipientPerson = personAliasService.GetPerson( recipientPersonAliasId.Value );
                    var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
                    mergeFields.Add( "Person", recipientPerson );

                    html = lava.ResolveMergeFields( mergeFields );
                }
            }

            litSelectedRecipientDescription.Text = string.Format( "<div class='header-lava pull-left'>{0}</div>", html );
        }

        /// <summary>
        /// Updates the read property.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        private void UpdateReadProperty( string messageKey )
        {
            int? smsPhoneDefinedValueId = hfSmsNumber.ValueAsInt();

            if ( smsPhoneDefinedValueId == default( int ) )
            {
                return;
            }

            new CommunicationResponseService( new RockContext() ).UpdateReadPropertyByMessageKey( messageKey, smsPhoneDefinedValueId.Value );
        }

        /// <summary>
        /// POCO to store communication info
        /// </summary>
        protected class ResponseListItem
        {
            public int? RecipientPersonAliasId { get; set; }

            public string MessageKey { get; set; }

            public string FullName { get; set; }

            public DateTime? CreatedDateTime { get; set; }

            public string HumanizedCreatedDateTime { get; set; }

            public string SMSMessage { get; set; }

            public bool IsRead { get; set; }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            string keyPrefix = string.Format( "sms-conversations-{0}-", this.BlockId );

            if ( ddlSmsNumbers.Visible )
            {
                this.SetUserPreference( keyPrefix + "smsNumber", ddlSmsNumbers.SelectedValue.ToString(), false );
                hfSmsNumber.SetValue( ddlSmsNumbers.SelectedValue.AsInteger() );
            }
            else
            {
                this.SetUserPreference( keyPrefix + "smsNumber", hfSmsNumber.Value.ToString(), false );
            }
            this.SetUserPreference( keyPrefix + "showRead", tglShowRead.Checked.ToString(), false );
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="toPersonAliasId">To person alias identifier.</param>
        /// <param name="message">The message.</param>
        private void SendMessage( int toPersonAliasId, string message )
        {
            using ( var rockContext = new RockContext() )
            {
                // The sender is the logged in user.
                int fromPersonAliasId = CurrentUser.Person.PrimaryAliasId.Value;
                string fromPersonName = CurrentUser.Person.FullName;

                // The sending phone is the selected one
                DefinedValueCache fromPhone = DefinedValueCache.Get( hfSmsNumber.ValueAsInt() );

                string responseCode = Rock.Communication.Medium.Sms.GenerateResponseCode( rockContext );

                // Create and enqueue the communication
                Rock.Communication.Medium.Sms.CreateCommunicationMobile( CurrentUser.Person, toPersonAliasId, message, fromPhone, responseCode, rockContext );
            }
        }

        /// <summary>
        /// Updates the message part for the grid row with the selected message key with the provided message string.
        /// </summary>
        /// <param name="message">The message.</param>
        private void UpdateMessagePart( string message )
        {
            foreach ( GridViewRow row in gRecipients.Rows )
            {
                if ( row.RowType != DataControlRowType.DataRow )
                {
                    continue;
                }

                var messageKeyHiddenField = ( HiddenFieldWithClass ) row.FindControl( "hfMessageKey" );
                if ( messageKeyHiddenField.Value == hfSelectedMessageKey.Value )
                {
                    // This is our row, update the lit
                    Literal literal = ( Literal ) row.FindControl( "litMessagePart" );
                    literal.Text = message;
                    break;
                }
            }
        }

        #endregion private/protected Methods

        #region Control Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            if ( LoadPhoneNumbers() )
            {
                nbNoNumbers.Visible = false;
                divMain.Visible = true;
                LoadResponseListing();
            }
            else
            {
                nbNoNumbers.Visible = true;
                divMain.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the lbLinkConversation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbLinkConversation_Click( object sender, EventArgs e )
        {
            mdLinkToPerson.Show();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSmsNumbers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlSmsNumbers_SelectedIndexChanged( object sender, EventArgs e )
        {
            SaveSettings();
            LoadResponseListing();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglShowRead control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tglShowRead_CheckedChanged( object sender, EventArgs e )
        {
            SaveSettings();
            LoadResponseListing();
        }

        /// <summary>
        /// Handles the Click event of the btnCreateNewMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCreateNewMessage_Click( object sender, EventArgs e )
        {
            mdNewMessage.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSend_Click( object sender, EventArgs e )
        {
            string message = tbNewMessage.Text.Trim();

            if ( message.Length == 0 || hfSelectedRecipientPersonAliasId.Value == string.Empty )
            {
                return;
            }

            int toPersonAliasId = hfSelectedRecipientPersonAliasId.ValueAsInt();
            SendMessage( toPersonAliasId, message );
            tbNewMessage.Text = string.Empty;
            LoadResponsesForRecipient( toPersonAliasId );
            UpdateMessagePart( message );
        }

        /// <summary>
        /// Handles the SaveClick event of the mdNewMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdNewMessage_SaveClick( object sender, EventArgs e )
        {
            string message = tbSMSTextMessage.Text.Trim();
            if ( message.IsNullOrWhiteSpace() )
            {
                return;
            }

            nbNoSms.Visible = false;

            int toPersonAliasId = ppRecipient.PersonAliasId.Value;
            var personAliasService = new PersonAliasService( new RockContext() );
            var toPerson = personAliasService.GetPerson( toPersonAliasId );
            if ( !toPerson.PhoneNumbers.Where( p => p.IsMessagingEnabled ).Any() )
            {
                nbNoSms.Visible = true;
                return;
            }

            SendMessage( toPersonAliasId, message );

            mdNewMessage.Hide();
            LoadResponseListing();
        }

        protected void ppRecipient_SelectPerson( object sender, EventArgs e )
        {
            nbNoSms.Visible = false;

            int toPersonAliasId = ppRecipient.PersonAliasId.Value;
            var personAliasService = new PersonAliasService( new RockContext() );
            var toPerson = personAliasService.GetPerson( toPersonAliasId );
            if ( !toPerson.PhoneNumbers.Where( p => p.IsMessagingEnabled ).Any() )
            {
                nbNoSms.Visible = true;
            }
        }

        /// <summary>
        /// Handles the RowSelected event of the gRecipients control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gRecipients_RowSelected( object sender, RowEventArgs e )
        {
            if ( e.Row.RowType != DataControlRowType.DataRow )
            {
                return;
            }

            var hfRecipientPersonAliasId = ( HiddenField ) e.Row.FindControl( "hfRecipientPersonAliasId" );
            var hfMessageKey = ( HiddenField ) e.Row.FindControl( "hfMessageKey" );

            // Since we can get newer messages when a selected let's also update the message part on the response recipients grid.
            var litMessagePart = ( Literal ) e.Row.FindControl( "litMessagePart" );

            int? recipientPersonAliasId = hfRecipientPersonAliasId.Value.AsIntegerOrNull();
            string messageKey = hfMessageKey.Value;

            hfSelectedRecipientPersonAliasId.Value = recipientPersonAliasId.ToString();
            hfSelectedMessageKey.Value = hfMessageKey.Value;

            Person recipientPerson = null;
            if ( recipientPersonAliasId.HasValue )
            {
                recipientPerson = new PersonAliasService( new RockContext() ).GetPerson( recipientPersonAliasId.Value );
            }

            if ( recipientPerson == null )
            {
                litMessagePart.Text = LoadResponsesForRecipient( messageKey );
            }
            else
            {
                litMessagePart.Text = LoadResponsesForRecipient( recipientPersonAliasId.Value );
            }

            UpdateReadProperty( messageKey );
            tbNewMessage.Visible = true;
            btnSend.Visible = true;

            upConversation.Attributes.Add( "class", "conversation-panel has-focus" );

            foreach ( GridViewRow row in gRecipients.Rows )
            {
                row.RemoveCssClass( "selected" );
            }

            e.Row.AddCssClass( "selected" );
            e.Row.RemoveCssClass( "unread" );

            var recordTypeValueIdNameless = DefinedValueCache.GetId( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_NAMELESS.AsGuid() );

            if ( recipientPerson == null || ( recipientPerson.RecordTypeValueId == recordTypeValueIdNameless ) )
            {
                lbLinkConversation.Visible = true;
            }
            else
            {
                lbLinkConversation.Visible = false;
            }

            PopulatePersonLava( e );
        }

        /// <summary>
        /// Handles the RowDataBound event of the gRecipients control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gRecipients_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( e.Row.RowType != DataControlRowType.DataRow )
            {
                return;
            }

            var responseListItem = e.Row.DataItem as ResponseListItem;
            if ( !responseListItem.IsRead )
            {
                e.Row.AddCssClass( "unread" );
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptConversation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptConversation_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var messageKey = ( HiddenFieldWithClass ) e.Item.FindControl( "hfCommunicationMessageKey" );
                if ( messageKey.Value != string.Empty )
                {
                    var divCommunication = ( HtmlGenericControl ) e.Item.FindControl( "divCommunication" );
                    divCommunication.RemoveCssClass( "outbound" );
                    divCommunication.AddCssClass( "inbound" );
                }
            }
        }

        #endregion Control Events

        #region Link Conversation Modal

        /// <summary>
        /// Handles the SaveClick event of the mdLinkConversation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdLinkToPerson_SaveClick( object sender, EventArgs e )
        {
            // Do some validation on entering a new person/family first
            if ( pnlLinkToNewPerson.Visible )
            {
                var validationMessages = new List<string>();
                bool isValid = true;

                DateTime? birthdate = newPersonEditor.PersonBirthDate;
                if ( !newPersonEditor.PersonBirthDateIsValid )
                {
                    validationMessages.Add( "Birthdate is not valid." );
                    isValid = false;
                }

                if ( !isValid )
                {
                    if ( validationMessages.Any() )
                    {
                        nbAddPerson.Text = "<ul><li>" + validationMessages.AsDelimited( "</li><li>" ) + "</li></lu>";
                        nbAddPerson.Visible = true;
                    }

                    return;
                }
            }

            using ( var rockContext = new RockContext() )
            {
                // Get the Person Record from the selected conversation. (It should be a 'NamelessPerson' record type)
                int? selectedRecipientPersonAliasId = hfSelectedRecipientPersonAliasId.Value.AsIntegerOrNull();
                Person selectedRecipient = null;
                Person person;
                var personAliasService = new PersonAliasService( rockContext );
                var personService = new PersonService( rockContext );
                var phoneNumberService = new PhoneNumberService( rockContext );

                if ( selectedRecipientPersonAliasId.HasValue )
                {
                    selectedRecipient = personAliasService.GetPerson( selectedRecipientPersonAliasId.Value );
                }

                int mobilePhoneTypeId = DefinedValueCache.Get( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE ).Id;
                PhoneNumber mobilePhoneNumber;

                if ( pnlLinkToExistingPerson.Visible )
                {
                    if ( !ppPerson.PersonId.HasValue )
                    {
                        return;
                    }

                    // All we need to do here is add/edit the SMS enabled phone number and save
                    person = personService.Get( ppPerson.PersonId.Value );
                    mobilePhoneNumber = person.PhoneNumbers.FirstOrDefault( n => n.NumberTypeValueId == mobilePhoneTypeId );

                    if ( mobilePhoneNumber == null )
                    {
                        // the person we are linking the phone number to doesn't have a SMS Messaging Number, so add a new one
                        mobilePhoneNumber = new PhoneNumber
                        {
                            NumberTypeValueId = mobilePhoneTypeId,
                            IsMessagingEnabled = true,
                            Number = hfSelectedMessageKey.Value
                        };

                        person.PhoneNumbers.Add( mobilePhoneNumber );
                    }
                    else
                    {
                        // A person should only have one Mobile Phone Number, and no more than one phone with Messaging enabled. (Rock enforces that in the Person Profile UI)
                        // So, if they already have a Messaging Enabled Mobile Number, change it to the new linked number
                        mobilePhoneNumber.Number = hfSelectedMessageKey.Value;
                        mobilePhoneNumber.IsMessagingEnabled = true;
                    }

                    // ensure they only have one SMS Number
                    var otherSMSPhones = person.PhoneNumbers.Where( a => a != mobilePhoneNumber && a.IsMessagingEnabled == true ).ToList();
                    foreach ( var otherSMSPhone in otherSMSPhones )
                    {
                        otherSMSPhone.IsMessagingEnabled = false;
                    }

                    rockContext.SaveChanges();
                    hfSelectedRecipientPersonAliasId.Value = person.PrimaryAliasId.ToString();
                }
                else
                {
                    // new Person and new family
                    person = new Person();

                    person.TitleValueId = newPersonEditor.PersonTitleValueId;
                    person.FirstName = newPersonEditor.FirstName;
                    person.NickName = newPersonEditor.FirstName;
                    person.LastName = newPersonEditor.LastName;
                    person.SuffixValueId = newPersonEditor.PersonSuffixValueId;
                    person.Gender = newPersonEditor.PersonGender;
                    person.MaritalStatusValueId = newPersonEditor.PersonMaritalStatusValueId;

                    person.PhoneNumbers = new List<PhoneNumber>();
                    mobilePhoneNumber = new PhoneNumber
                    {
                        NumberTypeValueId = mobilePhoneTypeId,
                        IsMessagingEnabled = true,
                        Number = hfSelectedMessageKey.Value
                    };

                    person.PhoneNumbers.Add( mobilePhoneNumber );

                    var birthMonth = person.BirthMonth;
                    var birthDay = person.BirthDay;
                    var birthYear = person.BirthYear;

                    var birthday = newPersonEditor.PersonBirthDate;
                    if ( birthday.HasValue )
                    {
                        person.BirthMonth = birthday.Value.Month;
                        person.BirthDay = birthday.Value.Day;
                        if ( birthday.Value.Year != DateTime.MinValue.Year )
                        {
                            person.BirthYear = birthday.Value.Year;
                        }
                        else
                        {
                            person.BirthYear = null;
                        }
                    }
                    else
                    {
                        person.SetBirthDate( null );
                    }

                    person.GradeOffset = newPersonEditor.PersonGradeOffset;
                    person.ConnectionStatusValueId = newPersonEditor.PersonConnectionStatusValueId;

                    var groupMember = new GroupMember();
                    groupMember.GroupRoleId = newPersonEditor.PersonGroupRoleId;
                    groupMember.Person = person;

                    var groupMembers = new List<GroupMember>();
                    groupMembers.Add( groupMember );

                    Group group = GroupService.SaveNewFamily( rockContext, groupMembers, null, true );
                    hfSelectedRecipientPersonAliasId.Value = person.PrimaryAliasId.ToString();
                }

                new CommunicationResponseService( rockContext ).UpdatePersonAliasByMessageKey( hfSelectedRecipientPersonAliasId.ValueAsInt(), hfSelectedMessageKey.Value, PersonAliasType.FromPersonAlias );

                // now that we've linked to a new person, get rid of the NameLess Person record
                if ( selectedRecipient != null )
                {
                    var selectedConversationPhoneNumber = selectedRecipient.PhoneNumbers.Where( a => a.Number == mobilePhoneNumber.Number ).FirstOrDefault();
                    if ( selectedConversationPhoneNumber != null )
                    {
                        phoneNumberService.Delete( selectedConversationPhoneNumber );
                    }
                    
                    var recordTypeIdNameLessPerson =  DefinedValueCache.GetId( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_NAMELESS.AsGuid() );

                    if ( selectedRecipient.RecordTypeValueId == recordTypeIdNameLessPerson )
                    {
                        // point any personAliases for the nameless person to the linked person
                        foreach ( var alias in selectedRecipient.Aliases.ToList() )
                        {
                            alias.PersonId = person.Id;
                            alias.AliasPersonId = selectedRecipient.Id;
                            alias.AliasPersonGuid = selectedRecipient.Guid;
                        }

                        personService.Delete( selectedRecipient );
                    }

                    rockContext.SaveChanges();
                }
            }

            mdLinkToPerson.Hide();
            LoadResponseListing();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglLinkPersonMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tglLinkPersonMode_CheckedChanged( object sender, EventArgs e )
        {
            pnlLinkToExistingPerson.Visible = tglLinkPersonMode.Checked;
            pnlLinkToNewPerson.Visible = !tglLinkPersonMode.Checked;
        }

        #endregion Link Conversation Modal
    }
}