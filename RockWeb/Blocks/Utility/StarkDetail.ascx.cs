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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.UI;
using Rock;
using Rock.Attribute;
using Rock.Model;

namespace RockWeb.Blocks.Utility
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Stark Detail" )]
    [Category( "Utility" )]
    [Description( "Template block for developers to use to start a new detail block." )]

    #region Block Attributes

    [BooleanField(
        "Show Email Address",
        Key = AttributeKey.ShowEmailAddress,
        Description = "Should the email address be shown?",
        DefaultBooleanValue = true,
        Order = 1 )]

    [EmailField(
        "Email",
        Key = AttributeKey.Email,
        Description = "The Email address to show.",
        DefaultValue = "ted@rocksolidchurchdemo.com",
        Order = 2 )]

    #endregion Block Attributes
    public partial class StarkDetail : Rock.Web.UI.RockBlock
    {

        #region Attribute Keys

        private static class AttributeKey
        {
            public const string ShowEmailAddress = "ShowEmailAddress";
            public const string Email = "Email";
        }

        #endregion Attribute Keys

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string StarkId = "StarkId";
        }

        #endregion PageParameterKeys

        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );


            if ( !Page.IsPostBack )
            {

                
                /*
                BEFORE
                379.49 per minute, 15810.8759ms total, 500 calls parallel random
                1898.53 per minute, 3160.3374ms total, 100 calls parallel random
                300.27 per minute, 19981.7326ms total, 500 calls parallel random
                1708.15 per minute, 3512.5698ms total, 100 calls parallel random

                USING GetPersonFromMobilePhoneNumber, with || clause
                394.2 per minute, 15220.6857ms total, 500 calls parallel random
                2155.78 per minute, 2783.2121ms total, 100 calls parallel random
                448.27 per minute, 13384.9163ms total, 500 calls parallel random
                2068.55 per minute, 2900.5863ms total, 100 calls parallel random

                USING GetPersonFromMobilePhoneNumber, without || clause
                919.3 per minute, 6526.7253ms total, 500 calls parallel random
                4425.2 per minute, 1355.871ms total, 100 calls parallel random
                999.09 per minute, 6005.4561ms total, 500 calls parallel random
                4489.48 per minute, 1336.4567ms total, 100 calls parallel random

                USING GetPersonFromMobilePhoneNumber, with new indexed PhoneNumber.FullNumber column
                4452.97 per minute, 1347.4146ms total, 500 calls parallel random
                235852.76 per minute, 25.4396ms total, 100 calls parallel random
                55024.32 per minute, 109.0427ms total, 500 calls parallel random
                235392.89 per minute, 25.4893ms total, 100 calls parallel random


                AFTER

                */

                Random random = new Random();
                var personsFound = 0;
                string[] numbers = {"16323331756",
"16238933812",
"16306787214",
"16249299663",
"16313910066",
"16292209272",
"16276251665",
"16230380642",
"16241356394",
"16325117674",
"16323095213",
"16283238737",
"16301925617",
"16244808558",
"16251291896",
"16238594121",
"16279932653",
"16269670744",
"16276546005",
"16306944376",
"16304036557",
"16302322183",
"16324899795",
"16275727737",
"16262527608",
"16306427649",
"16264751700",
"16240618420",
"16263245185",
"16301722749",
"16316035414",
"16254191098",
"16241264870",
"16245282464",
"16273832613",
"16284272552",
"16285370791",
"16238365033",
"16238737781",
"16232592530",
"16322817647",
"16296344234",
"16256625780",
"16272428680",
"16321023611",
"16250719949",
"16322097758",
"16281489893",
"16286732429",
"16310937621",
"16273857616",
"16247225212",
"16308979638",
"16248753357",
"16280432076",
"16295772868",
"16300319888",
"16287116495",
"16236861645",
"16265160654",
"16234488648",
"16256299400",
"16274955891",
"16247216759",
"16291041752",
"16287150126",
"16247734861",
"16233463248",
"16255448967",
"16313987686",
"16241403686",
"16282980662",
"16286770504",
"16296693244",
"16284371222",
"16234859093",
"16296641529",
"16251160567",
"16237245583",
"16287059643",
"16232887577",
"16243117025",
"16307648770",
"16230732434",
"16283812199",
"14658524182",
"14656773295",
"14657930995",
"14655577364",
"14654228323",
"14650568662",
"14656229513",
"14656215369",
"14655175424",
"14659853116",
"14654293093",
"14653619096",
"14653670798",
"14657646895",
"14657909522"};

                /* BEFORE
                using GetPersonFromMobilePhoneNumber, with || clause
407.92 per minute, 14708.8692ms total, 500 calls parallel random, 422 personsFound
2264.97 per minute, 2649.038ms total, 100 calls parallel random
443.89 per minute, 13516.744ms total, 500 calls parallel random, 429 personsFound
2243.39 per minute, 2674.5239ms total, 100 calls parallel random
447.33 per minute, 13412.8006ms total, 500 calls parallel random, 429 personsFound
2121.02 per minute, 2828.828ms total, 100 calls parallel random

                using GetPersonFromMobilePhoneNumber, with new PhoneNumber.FullNumber column (FullNumber IX not includes PersonID)
40142.37 per minute, 149.468ms total, 500 calls parallel random, 416 personsFound
185303 per minute, 32.3794ms total, 100 calls parallel random
41930.39 per minute, 143.0943ms total, 500 calls parallel random, 435 personsFound
197138.21 per minute, 30.4355ms total, 100 calls parallel random
41339.65 per minute, 145.1391ms total, 500 calls parallel random, 411 personsFound
220057.51 per minute, 27.2656ms total, 100 calls parallel random

SECC optimization
36348.13 per minute, 165.0704ms total, 500 calls parallel random, 438 personsFound
207352.01 per minute, 28.9363ms total, 100 calls parallel random
32399.46 per minute, 185.1883ms total, 500 calls parallel random, 420 personsFound
216813.14 per minute, 27.6736ms total, 100 calls parallel random
42651.44 per minute, 140.6752ms total, 500 calls parallel random, 408 personsFound
216093.96 per minute, 27.7657ms total, 100 calls parallel random


                using GetPersonFromMobilePhoneNumber, with new PhoneNumber.FullNumber column (FullNumber IX includes PersonID)
27555.7 per minute, 217.7408ms total, 500 calls parallel random, 427 personsFound
154864.29 per minute, 38.7436ms total, 100 calls parallel random
26401.76 per minute, 227.2576ms total, 500 calls parallel random, 413 personsFound
168868.52 per minute, 35.5306ms total, 100 calls parallel random
32347.8 per minute, 185.484ms total, 500 calls parallel random, 420 personsFound
188967.45 per minute, 31.7515ms total, 100 calls parallel random




                 */
                // Warmup
                string x;
                Rock.Utility.TextToWorkflow.MessageRecieved( "6235551212", "6235551212", "Hello", out x );

                Stopwatch stopwatch = Stopwatch.StartNew();
                Parallel.For( 1, 500, ( a ) =>
                {
                    string response;
                    var phoneNumber = numbers[random.Next( 0, 99 )];
                    Rock.Utility.TextToWorkflow.MessageRecieved( "6235551212", phoneNumber, "Hello", out response );
                    if (response.AsBoolean())
                    {
                        personsFound++;
                    }
                } );

                stopwatch.Stop();
                Debug.WriteLine( "{1} per minute, {0}ms total, 500 calls parallel random, {2} personsFound", stopwatch.Elapsed.TotalMilliseconds, Math.Round( (100 / stopwatch.Elapsed.TotalMinutes) , 2 ), personsFound );
                stopwatch.Restart();

                Parallel.For( 1, 100, ( a ) =>
                {
                    string response;
                    var phoneNumber = numbers[random.Next( 0, 99 )];
                    Rock.Utility.TextToWorkflow.MessageRecieved( "6235551212", phoneNumber, "Hello", out response );
                    if ( response.AsBoolean() )
                    {
                        personsFound++;
                    }
                } );

                stopwatch.Stop();
                Debug.WriteLine( "{1} per minute, {0}ms total, 100 calls parallel random", stopwatch.Elapsed.TotalMilliseconds, Math.Round( ( 100 / stopwatch.Elapsed.TotalMinutes ), 2 ) );
                stopwatch.Restart();

                // added for your convenience

                // to show the created/modified by date time details in the PanelDrawer do something like this:
                // pdAuditDetails.SetEntity( <YOUROBJECT>, ResolveRockUrl( "~" ) );
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        #endregion

        #region Methods

        // helper functional methods (like BindGrid(), etc.)

        #endregion
    }
}