//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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
//
using System;
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for AnalyticsDimFamilyCurrent that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class AnalyticsDimFamilyCurrentEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int AdultCount { get; set; }

        /// <summary />
        public int? CampusId { get; set; }

        /// <summary />
        public string CampusName { get; set; }

        /// <summary />
        public string CampusShortCode { get; set; }

        /// <summary />
        public int ChildCount { get; set; }

        /// <summary />
        public string ConnectionStatus { get; set; }

        /// <summary />
        public int Count { get; set; }

        /// <summary />
        public bool CurrentRowIndicator { get; set; }

        /// <summary />
        public DateTime EffectiveDate { get; set; }

        /// <summary />
        public DateTime ExpireDate { get; set; }

        /// <summary />
        public int FamilyId { get; set; }

        /// <summary />
        public string FamilyTitle { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public int? HeadOfHouseholdPersonKey { get; set; }

        /// <summary />
        public bool IsEra { get; set; }

        /// <summary />
        public bool IsFamilyActive { get; set; }

        /// <summary />
        public string MailingAddressCity { get; set; }

        /// <summary />
        public string MailingAddressCountry { get; set; }

        /// <summary />
        public string MailingAddressCounty { get; set; }

        /// <summary />
        public object MailingAddressGeoFence { get; set; }

        /// <summary />
        public object MailingAddressGeoPoint { get; set; }

        /// <summary />
        public double? MailingAddressLatitude { get; set; }

        /// <summary />
        public int? MailingAddressLocationId { get; set; }

        /// <summary />
        public double? MailingAddressLongitude { get; set; }

        /// <summary />
        public string MailingAddressPostalCode { get; set; }

        /// <summary />
        public string MailingAddressState { get; set; }

        /// <summary />
        public string MailingAddressStreet1 { get; set; }

        /// <summary />
        public string MailingAddressStreet2 { get; set; }

        /// <summary />
        public string MappedAddressCity { get; set; }

        /// <summary />
        public string MappedAddressCountry { get; set; }

        /// <summary />
        public string MappedAddressCounty { get; set; }

        /// <summary />
        public object MappedAddressGeoFence { get; set; }

        /// <summary />
        public object MappedAddressGeoPoint { get; set; }

        /// <summary />
        public double? MappedAddressLatitude { get; set; }

        /// <summary />
        public int? MappedAddressLocationId { get; set; }

        /// <summary />
        public double? MappedAddressLongitude { get; set; }

        /// <summary />
        public string MappedAddressPostalCode { get; set; }

        /// <summary />
        public string MappedAddressState { get; set; }

        /// <summary />
        public string MappedAddressStreet1 { get; set; }

        /// <summary />
        public string MappedAddressStreet2 { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source AnalyticsDimFamilyCurrent object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( AnalyticsDimFamilyCurrent source )
        {
            this.Id = source.Id;
            this.AdultCount = source.AdultCount;
            this.CampusId = source.CampusId;
            this.CampusName = source.CampusName;
            this.CampusShortCode = source.CampusShortCode;
            this.ChildCount = source.ChildCount;
            this.ConnectionStatus = source.ConnectionStatus;
            this.Count = source.Count;
            this.CurrentRowIndicator = source.CurrentRowIndicator;
            this.EffectiveDate = source.EffectiveDate;
            this.ExpireDate = source.ExpireDate;
            this.FamilyId = source.FamilyId;
            this.FamilyTitle = source.FamilyTitle;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.HeadOfHouseholdPersonKey = source.HeadOfHouseholdPersonKey;
            this.IsEra = source.IsEra;
            this.IsFamilyActive = source.IsFamilyActive;
            this.MailingAddressCity = source.MailingAddressCity;
            this.MailingAddressCountry = source.MailingAddressCountry;
            this.MailingAddressCounty = source.MailingAddressCounty;
            this.MailingAddressGeoFence = source.MailingAddressGeoFence;
            this.MailingAddressGeoPoint = source.MailingAddressGeoPoint;
            this.MailingAddressLatitude = source.MailingAddressLatitude;
            this.MailingAddressLocationId = source.MailingAddressLocationId;
            this.MailingAddressLongitude = source.MailingAddressLongitude;
            this.MailingAddressPostalCode = source.MailingAddressPostalCode;
            this.MailingAddressState = source.MailingAddressState;
            this.MailingAddressStreet1 = source.MailingAddressStreet1;
            this.MailingAddressStreet2 = source.MailingAddressStreet2;
            this.MappedAddressCity = source.MappedAddressCity;
            this.MappedAddressCountry = source.MappedAddressCountry;
            this.MappedAddressCounty = source.MappedAddressCounty;
            this.MappedAddressGeoFence = source.MappedAddressGeoFence;
            this.MappedAddressGeoPoint = source.MappedAddressGeoPoint;
            this.MappedAddressLatitude = source.MappedAddressLatitude;
            this.MappedAddressLocationId = source.MappedAddressLocationId;
            this.MappedAddressLongitude = source.MappedAddressLongitude;
            this.MappedAddressPostalCode = source.MappedAddressPostalCode;
            this.MappedAddressState = source.MappedAddressState;
            this.MappedAddressStreet1 = source.MappedAddressStreet1;
            this.MappedAddressStreet2 = source.MappedAddressStreet2;
            this.Name = source.Name;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for AnalyticsDimFamilyCurrent that includes all the fields that are available for GETs. Use this for GETs (use AnalyticsDimFamilyCurrentEntity for POST/PUTs)
    /// </summary>
    public partial class AnalyticsDimFamilyCurrent : AnalyticsDimFamilyCurrentEntity
    {
    }
}
