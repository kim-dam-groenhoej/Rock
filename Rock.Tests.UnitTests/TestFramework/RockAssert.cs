using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rock.Tests.Shared
{
    /// <summary>
    /// Provides a root class to which assertion-style extension methods can be attached.
    /// </summary>
    public class RockAssert
    {
        #region Default MSTest Assertions

        public void AreEqual<T>( T expected, T actual )
        {
            Assert.AreEqual( expected, actual );
        }
        public void AreEqual( System.String expected, System.String actual, System.Boolean ignoreCase )
        {
            Assert.AreEqual( expected, actual, ignoreCase );
        }
        public void AreEqual( System.Single expected, System.Single actual, System.Single delta )
        {
            Assert.AreEqual( expected, actual, delta );
        }
        public void AreEqual( System.Double expected, System.Double actual, System.Double delta, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, delta, message, parameters );
        }
        public void AreEqual( System.Object expected, System.Object actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, message, parameters );
        }
        public void AreEqual( System.Single expected, System.Single actual, System.Single delta, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, delta, message, parameters );
        }
        public void AreEqual<T>( T expected, T actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, message, parameters );
        }
        public void AreEqual( System.Double expected, System.Double actual, System.Double delta )
        {
            Assert.AreEqual( expected, actual, delta );
        }
        public void AreEqual( System.Object expected, System.Object actual )
        {
            Assert.AreEqual( expected, actual );
        }
        public void AreEqual( System.String expected, System.String actual, System.Boolean ignoreCase, System.Globalization.CultureInfo culture )
        {
            Assert.AreEqual( expected, actual, ignoreCase, culture );
        }
        public void AreEqual( System.String expected, System.String actual, System.Boolean ignoreCase, System.Globalization.CultureInfo culture, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, ignoreCase, culture, message, parameters );
        }
        public void AreEqual( System.String expected, System.String actual, System.Boolean ignoreCase, System.String message, params System.Object[] parameters )
        {
            Assert.AreEqual( expected, actual, ignoreCase, message, parameters );
        }
        public void AreNotEqual( System.String notExpected, System.String actual, System.Boolean ignoreCase, System.Globalization.CultureInfo culture )
        {
            Assert.AreNotEqual( notExpected, actual, ignoreCase, culture );
        }
        public void AreNotEqual( System.String notExpected, System.String actual, System.Boolean ignoreCase, System.Globalization.CultureInfo culture, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, ignoreCase, culture, message, parameters );
        }
        public void AreNotEqual( System.Single notExpected, System.Single actual, System.Single delta )
        {
            Assert.AreNotEqual( notExpected, actual, delta );
        }
        public void AreNotEqual( System.String notExpected, System.String actual, System.Boolean ignoreCase )
        {
            Assert.AreNotEqual( notExpected, actual, ignoreCase );
        }
        public void AreNotEqual( System.Double notExpected, System.Double actual, System.Double delta )
        {
            Assert.AreNotEqual( notExpected, actual, delta );
        }
        public void AreNotEqual( System.Double notExpected, System.Double actual, System.Double delta, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, delta, message, parameters );
        }
        public void AreNotEqual( System.Single notExpected, System.Single actual, System.Single delta, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, delta, message, parameters );
        }
        public void AreNotEqual( System.String notExpected, System.String actual, System.Boolean ignoreCase, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, ignoreCase, message, parameters );
        }
        public void AreNotEqual<T>( T notExpected, T actual )
        {
            Assert.AreNotEqual( notExpected, actual );
        }
        public void AreNotEqual<T>( T notExpected, T actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, message, parameters );
        }
        public void AreNotEqual( System.Object notExpected, System.Object actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotEqual( notExpected, actual, message, parameters );
        }
        public void AreNotEqual( System.Object notExpected, System.Object actual )
        {
            Assert.AreNotEqual( notExpected, actual );
        }
        public void AreNotSame( System.Object notExpected, System.Object actual )
        {
            Assert.AreNotSame( notExpected, actual );
        }
        public void AreNotSame( System.Object notExpected, System.Object actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreNotSame( notExpected, actual, message, parameters );
        }
        public void AreSame( System.Object expected, System.Object actual )
        {
            Assert.AreSame( expected, actual );
        }
        public void AreSame( System.Object expected, System.Object actual, System.String message, params System.Object[] parameters )
        {
            Assert.AreSame( expected, actual, message, parameters );
        }
        public void Fail()
        {
            Assert.Fail();
        }
        public void Fail( System.String message, params System.Object[] parameters )
        {
            Assert.Fail( message, parameters );
        }
        public void Inconclusive( System.String message, params System.Object[] parameters )
        {
            Assert.Inconclusive( message, parameters );
        }
        public void Inconclusive()
        {
            Assert.Inconclusive();
        }
        public void IsFalse( System.Boolean condition )
        {
            Assert.IsFalse( condition );
        }
        public void IsFalse( System.Boolean condition, System.String message, params System.Object[] parameters )
        {
            Assert.IsFalse( condition, message, parameters );
        }
        public void IsInstanceOfType( System.Object value, System.Type expectedType )
        {
            Assert.IsInstanceOfType( value, expectedType );
        }
        public void IsInstanceOfType( System.Object value, System.Type expectedType, System.String message, params System.Object[] parameters )
        {
            Assert.IsInstanceOfType( value, expectedType, message, parameters );
        }
        public void IsNotInstanceOfType( System.Object value, System.Type wrongType, System.String message, params System.Object[] parameters )
        {
            Assert.IsNotInstanceOfType( value, wrongType, message, parameters );
        }
        public void IsNotInstanceOfType( System.Object value, System.Type wrongType )
        {
            Assert.IsNotInstanceOfType( value, wrongType );
        }
        public void IsNotNull( System.Object value, System.String message, params System.Object[] parameters )
        {
            Assert.IsNotNull( value, message, parameters );
        }
        public void IsNotNull( System.Object value )
        {
            Assert.IsNotNull( value );
        }
        public void IsNull( System.Object value, System.String message, params System.Object[] parameters )
        {
            Assert.IsNull( value, message, parameters );
        }
        public void IsNull( System.Object value )
        {
            Assert.IsNull( value );
        }
        public void IsTrue( System.Boolean condition, System.String message, params System.Object[] parameters )
        {
            Assert.IsTrue( condition, message, parameters );
        }
        public void IsTrue( System.Boolean condition )
        {
            Assert.IsTrue( condition );
        }
        public void ReplaceNullChars( System.String input )
        {
            Assert.ReplaceNullChars( input );
        }
        public void ThrowsException<T>( Action action )
            where T : Exception
        {
            Assert.ThrowsException<T>( action );
        }

        public void ThrowsException<T>( Action action, string message )
            where T : Exception
        {
            Assert.ThrowsException<T>( action, message );
        }

        public void ThrowsException<T>( Func<object> action )
            where T : Exception
        {
            Assert.ThrowsException<T>( action );
        }
        public void ThrowsException<T>( Func<object> action, string message )
            where T : Exception
        {
            Assert.ThrowsException<T>( action, message );
        }

        public void ThrowsException<T>( Func<object> action, string message, params object[] parameters )
            where T : Exception
        {
            Assert.ThrowsException<T>( action, message, parameters );
        }

        public void ThrowsException<T>( Action action, string message, params object[] parameters )
            where T : Exception
        {
            Assert.ThrowsException<T>( action, message, parameters );
        }

        #endregion

        #region Collection Assertions

        public void AreEqual<T>( IEnumerable<T> expected, IEnumerable<T> actual )
        {
            CollectionAssert.AreEquivalent( expected.ToList(), actual.ToList() );
        }
        public void AreEqual<T>( List<T> expected, List<T> actual )
        {
            CollectionAssert.AreEquivalent( expected, actual );
        }

        public void AreEqual( IEnumerable<dynamic> expected, IEnumerable<dynamic> actual )
        {
            CollectionAssert.AreEquivalent( expected.ToList(), actual.ToList() );
        }

        #endregion

        #region Empty Assertions

        public void IsEmpty( string input )
        {
            if ( string.IsNullOrEmpty( input ) )
            {
                return;
            }

            throw new AssertFailedException( "Expected: (empty), Actual: (value)" );
        }

        public void IsNotEmpty( string input )
        {
            if ( !string.IsNullOrEmpty( input ) )
            {
                return;
            }

            throw new AssertFailedException( "Expected: (value), Actual: (empty)" );
        }

        public void IsNotEmpty<T>( ICollection<T> input )
        {
            if ( input != null && input.Any() )
            {
                return;
            }

            throw new AssertFailedException( "Collection is empty, but a value was expected." );
        }

        #endregion
    }
}
