using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rock.Tests.Rock.Utility.NcoaApi
{
    /// <summary>
    /// Test if TrueNCOA columns changed.
    /// </summary>
    /// <seealso cref="Xunit.IClassFixture{Rock.Tests.Rock.Utility.NcoaApi.TrueNcoaApiFixture}" />
    [TestClass]
    public class TrueNcoaApiTests
    {
        #region Initialization

        /// <summary>
        /// Gets or sets the TrueNOCA response columns.
        /// </summary>
        /// <value>
        /// The TrueNOCA response columns.
        /// </value>
        private static List<string> _ResponseColumns { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueNcoaApiFixture"/> class and download an export from TrueNCOA.
        /// </summary>
        [ClassInitialize]
        public static void InitializeTrueNcoaApiTests( TestContext context )
        {
            _ResponseColumns = new List<string>();

            string NCOA_SERVER = "https://app.testing.truencoa.com";
            string exportfileid = "4c58fba3-8ee9-4644-893a-e3903ad0f91b";

            var _client = new RestClient( NCOA_SERVER );
            _client.AddDefaultHeader( "user_name", "gerhard@sparkdevnetwork.org" );
            _client.AddDefaultHeader( "password", "TrueNCOA_password" );
            _client.AddDefaultHeader( "Content-Type", "application/x-www-form-urlencoded" );

            var request = new RestRequest( $"api/files/{exportfileid}/records", Method.GET );
            request.AddParameter( "application/x-www-form-urlencoded", "status=submit", ParameterType.RequestBody );
            IRestResponse response = _client.Execute( request );
            if ( response.StatusCode != HttpStatusCode.OK )
            {
                return;
            }

            dynamic obj = null;
            try
            {
                obj = JObject.Parse( response.Content );
                foreach ( var o in obj.Records[0] )
                {
                    _ResponseColumns.Add( o.Name );
                }

                return;
            }
            catch
            {
                return;
            }
        }

        #endregion

        //public TrueNcoaApiTests( TrueNcoaApiFixture trueNcoaApiFixture )
        //{
        //    this._trueNcoaApiFixture = trueNcoaApiFixture;
        //}

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_individual_id()
        {
            bool output = _ResponseColumns.Contains( "individual_id" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_individual_first_name()
        {
            bool output = _ResponseColumns.Contains( "individual_first_name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_individual_last_name()
        {
            bool output = _ResponseColumns.Contains( "individual_last_name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_line_1()
        {
            bool output = _ResponseColumns.Contains( "address_line_1" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_line_2()
        {
            bool output = _ResponseColumns.Contains( "address_line_2" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_city_name()
        {
            bool output = _ResponseColumns.Contains( "address_city_name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_state_code()
        {
            bool output = _ResponseColumns.Contains( "address_state_code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_postal_code()
        {
            bool output = _ResponseColumns.Contains( "address_postal_code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_address_country_code()
        {
            bool output = _ResponseColumns.Contains( "address_country_code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Household_Position()
        {
            bool output = _ResponseColumns.Contains( "Household Position" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Name_ID()
        {
            bool output = _ResponseColumns.Contains( "Name ID" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Street_Suffix()
        {
            bool output = _ResponseColumns.Contains( "Street Suffix" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Unit_Type()
        {
            bool output = _ResponseColumns.Contains( "Unit Type" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Unit_Number()
        {
            bool output = _ResponseColumns.Contains( "Unit Number" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Box_Number()
        {
            bool output = _ResponseColumns.Contains( "Box Number" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_City_Name()
        {
            bool output = _ResponseColumns.Contains( "City Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_State_Code()
        {
            bool output = _ResponseColumns.Contains( "State Code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Postal_Code()
        {
            bool output = _ResponseColumns.Contains( "Postal Code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Postal_Code_Extension()
        {
            bool output = _ResponseColumns.Contains( "Postal Code Extension" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Carrier_Route()
        {
            bool output = _ResponseColumns.Contains( "Carrier Route" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Address_Status()
        {
            bool output = _ResponseColumns.Contains( "Address Status" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Individual_Record_ID()
        {
            bool output = _ResponseColumns.Contains( "Individual Record ID" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Error_Number()
        {
            bool output = _ResponseColumns.Contains( "Error Number" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Address_Type()
        {
            bool output = _ResponseColumns.Contains( "Address Type" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Delivery_Point()
        {
            bool output = _ResponseColumns.Contains( "Delivery Point" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Check_Digit()
        {
            bool output = _ResponseColumns.Contains( "Check Digit" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Delivery_Point_Verification()
        {
            bool output = _ResponseColumns.Contains( "Delivery Point Verification" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Delivery_Point_Verification_Notes()
        {
            bool output = _ResponseColumns.Contains( "Delivery Point Verification Notes" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Vacant()
        {
            bool output = _ResponseColumns.Contains( "Vacant" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Congressional_District_Code()
        {
            bool output = _ResponseColumns.Contains( "Congressional District Code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Area_Code()
        {
            bool output = _ResponseColumns.Contains( "Area Code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Latitude()
        {
            bool output = _ResponseColumns.Contains( "Latitude" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_First_Name()
        {
            bool output = _ResponseColumns.Contains( "First Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Longitude()
        {
            bool output = _ResponseColumns.Contains( "Longitude" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Time_Zone()
        {
            bool output = _ResponseColumns.Contains( "Time Zone" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_County_Name()
        {
            bool output = _ResponseColumns.Contains( "County Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_County_FIPS()
        {
            bool output = _ResponseColumns.Contains( "County FIPS" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_State_FIPS()
        {
            bool output = _ResponseColumns.Contains( "State FIPS" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Barcode()
        {
            bool output = _ResponseColumns.Contains( "Barcode" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Locatable_Address_Conversion_System()
        {
            bool output = _ResponseColumns.Contains( "Locatable Address Conversion System" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Line_of_Travel()
        {
            bool output = _ResponseColumns.Contains( "Line of Travel" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Ascending_Descending()
        {
            bool output = _ResponseColumns.Contains( "Ascending/Descending" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Move_Applied()
        {
            bool output = _ResponseColumns.Contains( "Move Applied" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Last_Name()
        {
            bool output = _ResponseColumns.Contains( "Last Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Move_Type()
        {
            bool output = _ResponseColumns.Contains( "Move Type" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Move_Date()
        {
            bool output = _ResponseColumns.Contains( "Move Date" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Move_Distance()
        {
            bool output = _ResponseColumns.Contains( "Move Distance" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Match_Flag()
        {
            bool output = _ResponseColumns.Contains( "Match Flag" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_NXI()
        {
            bool output = _ResponseColumns.Contains( "NXI" );
            Assert.IsTrue( output );
        }

        [TestMethod] //[Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_ANK()
        {
            bool output = _ResponseColumns.Contains( "ANK" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Residential_Delivery_Indicator()
        {
            bool output = _ResponseColumns.Contains( "Residential Delivery Indicator" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Record_Type()
        {
            bool output = _ResponseColumns.Contains( "Record Type" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Country_Code()
        {
            bool output = _ResponseColumns.Contains( "Country Code" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Company_Name()
        {
            bool output = _ResponseColumns.Contains( "Company Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Address_Line_1()
        {
            bool output = _ResponseColumns.Contains( "Address Line 1" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Address_Line_2()
        {
            bool output = _ResponseColumns.Contains( "Address Line 2" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Address_Id()
        {
            bool output = _ResponseColumns.Contains( "Address Id" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Household_Id()
        {
            bool output = _ResponseColumns.Contains( "Household Id" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Individual_space_Id()
        {
            bool output = _ResponseColumns.Contains( "Individual Id" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Street_Number()
        {
            bool output = _ResponseColumns.Contains( "Street Number" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Street_Pre_Direction()
        {
            bool output = _ResponseColumns.Contains( "Street Pre Direction" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Street_Name()
        {
            bool output = _ResponseColumns.Contains( "Street Name" );
            Assert.IsTrue( output );
        }

        [TestMethod] [Ignore( "Needs seeded data in TrueNcoa" )]
        public void Records_Contains_Street_Post_Direction()
        {
            bool output = _ResponseColumns.Contains( "Street Post Direction" );
            Assert.IsTrue( output );
        }
    }
}
