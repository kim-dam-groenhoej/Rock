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
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class StepTerm : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DropIndex("dbo.StepProgram", new[] { "CategoryId" });
            AddColumn("dbo.StepProgram", "StepTerm", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.StepProgram", "CategoryId", c => c.Int());
            CreateIndex("dbo.StepProgram", "CategoryId");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.StepProgram", new[] { "CategoryId" });
            AlterColumn("dbo.StepProgram", "CategoryId", c => c.Int(nullable: false));
            DropColumn("dbo.StepProgram", "StepTerm");
            CreateIndex("dbo.StepProgram", "CategoryId");
        }
    }
}
