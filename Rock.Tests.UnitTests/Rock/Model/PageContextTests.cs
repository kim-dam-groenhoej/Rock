﻿using Rock.Model;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Tests.Shared;

namespace Rock.Tests.Rock.Model
{
    [TestClass]
    public class PageContextTests : TestClassBase
    {
        /// <summary>
        /// Should perform a shallow copy of a PageContext object, resulting in a new PageContext.
        /// </summary>
        [TestMethod]
        public void ShallowClone()
        {
            var pageContext = new PageContext { Guid = Guid.NewGuid() };
            var result = pageContext.Clone( false );
            Assert.AreEqual( result.Guid, pageContext.Guid );
        }

        /// <summary>
        /// Should serialize a PageContext into a non-empty string.
        /// </summary>
        [TestMethod]
        public void ToJson()
        {
            var pageContext = new PageContext { Guid = Guid.NewGuid() };
            var result = pageContext.ToJson();
            Assert.IsNotEmpty( result );
        }

        /// <summary>
        /// Shoulds serialize a PageContext into a JSON string.
        /// </summary>
        [TestMethod]
        public void ExportJson()
        {
            var guid = Guid.NewGuid();
            var pageContext = new PageContext
            {
                Guid = guid
            };

            var result = pageContext.ToJson();
            var key = string.Format( "\"Guid\":\"{0}\"", guid );
            Assert.AreNotEqual( result.IndexOf( key ), -1 );
        }

        /// <summary>
        /// Should take a JSON string and copy its contents to a Rock.Model.PageContext instance
        /// </summary>
        [TestMethod]
        public void ImportJson()
        {
            var obj = new PageContext
            {
                Guid = Guid.NewGuid(),
                IsSystem = false
            };

            var json = obj.ToJson();
            var pageContext = PageContext.FromJson( json );
            Assert.AreEqual( obj.Guid, pageContext.Guid );
            Assert.AreEqual( obj.IsSystem, pageContext.IsSystem );
        }
    }
}