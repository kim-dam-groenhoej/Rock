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

namespace Rock.Tests.Integration
{
    public static class TestCategories
    {
        // This Test Category contains methods for adding test data to the current database.
        public const string AddData = "Rock.Setup.AddData";
        // This Test Category contains methods for removing test data from the current database.
        public const string RemoveData = "Rock.Setup.RemoveData";
        // This Test Category contains methods for maintaining specific elements of the test data during the development process.
        public const string DeveloperSetup = "Rock.Setup.Dev";
    }

    public static class TestFeatures
    {
        // Tests having this property relate to data setup for integration tests.
        public const string DataSetup = "Data Setup (Basic)";
        public const string DataSetupBulk = "Data Setup (Bulk)";

        // Tests having this property relate to data maintenance that may be needed during the development process.
        public const string DataMaintenance = "Data Maintenance";

        // Tests having this property relate to the Reporting feature of Rock.
        public const string Reporting = "Reporting";

        // Tests having this property relate to the Steps feature of Rock.
        public const string Steps = "Steps";

        public const string Communications = "Communications";
        public const string Groups = "Groups";

    }

    public static class TestPurposes
    {
        // Tests having this purpose are intended to measure the performance of the function being tested.
        public const string Performance = "Performance";
        // Tests having this purpose are intended to validate the function being tested.
        public const string Validation = "Validation";
    }

    public static class TestPeople
    {
        public static Guid TedDeckerPersonGuid = new Guid( "8FEDC6EE-8630-41ED-9FC5-C7157FD1EAA4" );

        public static Guid BillMarblePersonGuid = new Guid( "1EA811BB-3118-42D1-B020-32A82BC8081A" );

        public static Guid AlishaMarblePersonGuid = new Guid( "69DC0FDC-B451-4303-BD91-EF17C0015D23" );

        public static Guid SarahSimmonsPersonGuid = new Guid( "FC6B9819-EF2E-44C9-93DB-05571B39E58F" );
        public static Guid BrianJonesPersonGuid = new Guid( "3D7F6605-3666-4AB5-9F4E-D7FEBF93278E" );

        public static Guid BenJonesPersonGuid = new Guid( "3C402382-3BD2-4337-A996-9E62F1BAB09D" );
        public static Guid BenJonesStepAlphaAttenderGuid = new Guid( "D5DDBBE4-9D62-4EDE-840D-E9DAB8F99430" );

    }

    public static class TestGuids
    {
        public static class Communications
        {
            public static string TestCommunicationSingleEmail1Guid = "{BDCDD3ED-22FF-43E8-9860-65D26DBD5B9B}";
            public static string TestCommunicationBulkEmail1Guid = "{D3EA6513-372D-4192-8E8F-DCA2707AA572}";
            public static string TestCommunicationBulkEmailNoInteractionsGuid = "{42AECB98-76BE-4CD5-A9FA-14FE2CC27887}";
            public static string TestCommunicationBulkSmsGuid = "{79E18AFD-DB14-485A-80D9-CDAC44CA1098}";
            public static string TestTwilioTransportEntityTypeGuid = "{CF9FD146-8623-4D9A-98E6-4BD710F071A4}";

            public static string TestSmsSenderGuid = "{DB8CFE73-4209-4109-8FC5-3C5013AFC290}";
            public static string MobilePhoneTedDecker = "6235553322";
            public static string TedDeckerPersonGuid = "{8FEDC6EE-8630-41ED-9FC5-C7157FD1EAA4}";

            public static string NamelessPerson1MobileNumber = "4807770001";
            public static string NamelessPerson2MobileNumber = "4807770002";

            public static string UnknownPerson1MobileNumber = "4807770101";
        }

        public static class Groups
        {
            // A DataView that returns all of the locations outside the state of Arizona.
            public static Guid DataViewLocationsOutsideArizonaGuid = new Guid( "14B1854D-4F45-4F4D-AFFF-C0A1E06353DF" );
            public static Guid DataViewLocationsInsideArizonaGuid = new Guid( "C39B353E-3E44-42C0-9D85-2107FB5E8C04" );

            public static Guid CategoryGroupsGuid = new Guid( "5CF5224C-F01D-4904-91D0-E58B723F0D2A" );
            public static Guid CategoryLocationsGuid = new Guid( "1D45C0A7-3DE8-428C-94A8-14E5ED5E2E36" );
        }

        public static class Steps
        {
            // Constants
            public static Guid CategoryAdultsGuid = new Guid( "43DC43A8-420B-4012-BAA0-0A0DDF2D4A9A" );
            public static Guid CategoryYouthGuid = new Guid( "EAB10217-F288-4B29-B56F-50BD4BA5FB08" );
            public static Guid CategoryStepsGuid = new Guid( "31A23665-1C2E-4901-9E20-A7A0C7E6DF70" );

            public static Guid ProgramSacramentsGuid = new Guid( "2CAFBB12-901F-4880-A3E4-848F25CAF1A6" );
            public static Guid StepTypeBaptismGuid = new Guid( "23E73F78-587A-483F-99EF-855FD6AD1B11" );
            public static Guid StepTypeEucharistGuid = new Guid( "5EA01E79-5D17-4E87-A94F-8C4DD22131B5" );
            public static Guid StepTypeAnnointingGuid = new Guid( "48141631-38F6-40C0-8470-5253208CEA9A" );
            public static Guid StepTypeConfessionGuid = new Guid( "5F754006-7F8C-4BED-94A8-FC61CEEC6B43" );
            public static Guid StepTypeMarriageGuid = new Guid( "D03B3C65-C128-4918-A300-509B94B90175" );
            public static Guid StepTypeHolyOrdersGuid = new Guid( "0099C701-6C1E-418E-A94F-C247A2FE4BA5" );
            public static Guid StepTypeConfirmationGuid = new Guid( "F109169F-C1F6-46ED-9091-274540E3F3E2" );
            public static Guid StatusSacramentsSuccessGuid = new Guid( "A5C2A14F-9ED9-4DF4-A1C8-8ADF75E18833" );
            public static Guid StatusSacramentsPendingGuid = new Guid( "B591240C-4D4D-49DA-82E3-F8C1738B8EC6" );
            public static Guid StatusSacramentsIncompleteGuid = new Guid( "D0CDE46C-46D6-4F8D-B48D-55D7ACE6C3BA" );
            public static Guid PrerequisiteHolyOrdersGuid = new Guid( "D96A2C67-2D76-4697-838F-3514CA11485E" );

            public static Guid ProgramAlphaGuid = new Guid( "F7C2BA07-579B-4800-BBE1-B14B73E21E12" );
            public static Guid StepTypeAttenderGuid = new Guid( "24A70E5C-9871-452E-9403-3B43C003AA87" );
            public static Guid StepTypeVolunteerGuid = new Guid( "31DB3478-840B-4541-972D-059690AD623E" );
            public static Guid StepTypeLeaderGuid = new Guid( "95DF3AC6-AFBF-4BEA-AF86-805335A1C4CB" );
            public static Guid StatusAlphaStartedGuid = new Guid( "F29CF8A1-76A9-4436-9726-810DF0BC95C7" );
            public static Guid StatusAlphaCompletedGuid = new Guid( "7BA5F14D-BB38-4AAB-AB69-3A7F49494A55" );

            // Known Step Achievements
            // These steps are specifically identified to enable verification of query and filtering operations.
            public static Guid TedDeckerPersonGuid = new Guid( "8FEDC6EE-8630-41ED-9FC5-C7157FD1EAA4" );
            public static Guid TedDeckerStepBaptismGuid = new Guid( "02BB71C9-5FE9-45B8-B230-51C7A8475B6B" );
            public static Guid TedDeckerStepConfirmationGuid = new Guid( "15412166-1DE0-41C0-9F8D-F07160513CAE" );
            public static Guid TedDeckerStepMarriage1Guid = new Guid( "414EBE88-2CD2-40E1-893B-216DEA2CB25E" );
            public static Guid TedDeckerStepMarriage2Guid = new Guid( "314F5303-C803-442B-AE39-DAA7BC30CCEE" );

            public static Guid BillMarblePersonGuid = new Guid( "1EA811BB-3118-42D1-B020-32A82BC8081A" );
            public static Guid BillMarbleStepBaptismGuid = new Guid( "CBD25E24-FD14-4B69-884B-F78DA8BF8FD8" );
            public static Guid BillMarbleStepConfirmationGuid = new Guid( "FE6B8F0D-450B-4737-80EE-E2BC4FA4EAB5" );
            public static Guid BillMarbleStepMarriage1Guid = new Guid( "E6F37E9D-177F-4CEA-91EA-2C2B45C4B1FD" );
            public static Guid BillMarbleStepAlphaAttenderGuid = new Guid( "680F64F1-C579-4216-B08C-68AF3FC762EC" );

            public static Guid AlishaMarblePersonGuid = new Guid( "69DC0FDC-B451-4303-BD91-EF17C0015D23" );
            public static Guid AlishaMarbleStepBaptismGuid = new Guid( "12DF97EE-52E8-49CE-B804-2568B8837B81" );
            public static Guid AlishaMarbleStepConfirmationGuid = new Guid( "6B3037D8-23AC-478C-BCC9-7483B86976AA" );
            public static Guid AlishaMarbleStepMarriage1Guid = new Guid( "FB2BA405-4233-4E7E-83AB-8A94B5E2A80A" );
            public static Guid AlishaMarbleStepAlphaAttenderGuid = new Guid( "87976C94-C9E4-4839-9EFF-1E0B3F4EF5A0" );

            public static Guid SarahSimmonsPersonGuid = new Guid( "FC6B9819-EF2E-44C9-93DB-05571B39E58F" );
            public static Guid SarahSimmonsStepConfirmationGuid = new Guid( "E41A74DE-72C7-40B5-B05C-C1A72E09F5C0" );
            public static Guid SarahSimmonsStepMarriage1Guid = new Guid( "94342ACA-744B-41A0-9076-0BCB808514CF" );
            public static Guid SarahSimmonsStepAlphaAttenderGuid = new Guid( "B5907E4F-6197-4A9F-A1DB-046DE7BB2035" );

            public static Guid BrianJonesPersonGuid = new Guid( "3D7F6605-3666-4AB5-9F4E-D7FEBF93278E" );
            public static Guid BrianJonesStepBaptismGuid = new Guid( "92171B1E-25C8-408B-AA93-AB7263D02B5A" );

            public static Guid BenJonesPersonGuid = new Guid( "3C402382-3BD2-4337-A996-9E62F1BAB09D" );
            public static Guid BenJonesStepAlphaAttenderGuid = new Guid( "D5DDBBE4-9D62-4EDE-840D-E9DAB8F99430" );

            public static string ColorCodeRed = "#f00";
            public static string ColorCodeGreen = "#0f0";
            public static string ColorCodeBlue = "#00f";

            // A DataView that returns all of the people who have been baptised in 2001.
            public static Guid DataViewStepsCompleted2001Guid = new Guid( "E52D89CD-19A0-44B3-8052-D4FD206AB499" );
        }
    }
}
