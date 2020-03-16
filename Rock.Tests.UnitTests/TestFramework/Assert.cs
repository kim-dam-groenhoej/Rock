using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rock.Tests.UnitTests
{ internal static class EmptyAsserts
    {
        public static void IsEmpty( this Assert assert, string input )
        {
            if ( string.IsNullOrEmpty( input ) )
            {
                return;
            }

            throw new AssertFailedException( "Expected: (empty), Actual: (value)" );
        }

        public static void IsNotEmpty( this Assert assert, string input )
        {
            if ( !string.IsNullOrEmpty( input ) )
            {
                return;
            }

            throw new AssertFailedException( "Expected: (value), Actual: (empty)" );
        }

        public static void IsNotEmpty<T>( this Assert assert, ICollection<T> input )
        {
            if ( input != null && input.Any() )
            {
                return;
            }

            throw new AssertFailedException( "Collection is empty, but a value was expected." );
        }
        // ...
    }

    internal static class CollectionAsserts
    {
        public static void AreEqual<T>( this Assert assert, IEnumerable<T> expected, IEnumerable<T> output )
        {
            CollectionAssert.AreEquivalent( expected.ToList(), output.ToList() );
        }

        public static void AreEqual( this Assert assert, IEnumerable<dynamic> expected, IEnumerable<dynamic> output )
        {
            CollectionAssert.AreEquivalent( expected.ToList(), output.ToList() );
        }
    }
}
