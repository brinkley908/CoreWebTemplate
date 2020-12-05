using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace CoreWebTemplate
{
    public static class StringExtensions
    {

        private const string BlankString = "";

      
        public static string ToBlogDate(this DateTime value )
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract( value );

            if ( timeSpan <= TimeSpan.FromSeconds( 60 ) )
            {
                result = string.Format( "{0} seconds ago", timeSpan.Seconds );
            }
            else if ( timeSpan <= TimeSpan.FromMinutes( 60 ) )
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format( "about {0} minutes ago", timeSpan.Minutes ) :
                    "about a minute ago";
            }
            else if ( timeSpan <= TimeSpan.FromHours( 24 ) )
            {
                result = timeSpan.Hours > 1 ?
                    String.Format( "about {0} hours ago", timeSpan.Hours ) :
                    "about an hour ago";
            }
            else if ( timeSpan <= TimeSpan.FromDays( 30 ) )
            {
                result = timeSpan.Days > 1 ?
                    String.Format( "about {0} days ago", timeSpan.Days ) :
                    "yesterday";
            }
            else if ( timeSpan <= TimeSpan.FromDays( 365 ) )
            {
                result = timeSpan.Days > 30 ?
                    String.Format( "about {0} months ago", timeSpan.Days / 30 ) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format( "about {0} years ago", timeSpan.Days / 365 ) :
                    "about a year ago";
            }

            return result;
        }

        public static string Encrypt( this string value, string key )
        {
            byte[] iv = { 121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62 };
            var keyLen = key.Length;

            if ( keyLen >= 32 )
                key = key.Substring( 0, 32 );
            else
                key = key.PadRight( keyLen - 32, 'X' );

            return Convert.ToBase64String( EncryptStringToBytes( value, Encoding.ASCII.GetBytes( key ), iv ) );
        }

        public static string Decrypt( this string value, string key )
        {
            byte[] iv = { 121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62 };
            var keyLen = key.Length;

            if ( keyLen >= 32 )
                key = key.Substring( 0, 32 );
            else
                key = key.PadRight( keyLen - 32, 'X' );
            return DecryptStringFromBytes( Convert.FromBase64String( value ), Encoding.ASCII.GetBytes( key ), iv );
        }

        public static byte[] EncryptStringToBytes( string plainText, byte[] Key, byte[] IV )
        {
            if ( plainText == null || plainText.Length <= 0 )
                throw new ArgumentNullException( "plainText" );

            if ( Key == null || Key.Length <= 0 )
                throw new ArgumentNullException( "Key" );

            if ( IV == null || IV.Length <= 0 )
                throw new ArgumentNullException( "Key" );

            byte[] encrypted;

            using ( RijndaelManaged rijAlg = new RijndaelManaged() )
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform encryptor = rijAlg.CreateEncryptor( rijAlg.Key,
                                             rijAlg.IV );
                using MemoryStream msEncrypt = new MemoryStream();

                using CryptoStream csEncrypt = new CryptoStream( msEncrypt, encryptor, CryptoStreamMode.Write );

                using ( StreamWriter swEncrypt = new StreamWriter( csEncrypt ) )
                {
                    swEncrypt.Write( plainText );
                }

                encrypted = msEncrypt.ToArray();
            }

            return encrypted;
        }

        public static string DecryptStringFromBytes( byte[] cipherText, byte[] Key, byte[] IV )
        {
            if ( cipherText == null || cipherText.Length <= 0 )
                throw new ArgumentNullException( "cipherText" );
            if ( Key == null || Key.Length <= 0 )
                throw new ArgumentNullException( "Key" );
            if ( IV == null || IV.Length <= 0 )
                throw new ArgumentNullException( "Key" );
            string plaintext = null;
            using ( RijndaelManaged rijAlg = new RijndaelManaged() )
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor( rijAlg.Key,
                                             rijAlg.IV );
                using MemoryStream msDecrypt = new MemoryStream( cipherText );

                using CryptoStream csDecrypt = new CryptoStream( msDecrypt, decryptor, CryptoStreamMode.Read );

                using StreamReader srDecrypt = new StreamReader( csDecrypt );
                plaintext = srDecrypt.ReadToEnd();

            }

            return plaintext;
        }
        public static string ToBase64( this string value ) => Convert.ToBase64String( Encoding.ASCII.GetBytes( value ) );

        public static string FromBase64( this string value ) => Encoding.Default.GetString( Convert.FromBase64String( value ) );

        public static bool Empty( this string value ) => string.IsNullOrEmpty( value ) || value.Trim() == BlankString;

        public static bool EmptyOrWhiteSpace( this string value ) => value.Empty() || string.IsNullOrWhiteSpace( value );

        public static bool Empty<T>( this T[] value ) => value == null || !value.Any();

        public static bool Empty<T>( this List<T> value ) => value == null || !value.Any();

        public static List<T> EmptyIfNull<T>( this List<T> value ) => value ?? Activator.CreateInstance<List<T>>();

        public static T[] EmptyIfNull<T>( this T[] value ) => value ?? Activator.CreateInstance<T[]>();

        public static string Zip( this string uncompressedString )
        {
            byte[] compressedBytes;

            using var uncompressedStream = new MemoryStream( Encoding.UTF8.GetBytes( uncompressedString ) );

            using var compressedStream = new MemoryStream();

            // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
            // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
            // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change

            using var compressorStream = new DeflateStream( compressedStream, CompressionLevel.Fastest, true );

            uncompressedStream.CopyTo( compressorStream );

            // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
            compressedBytes = compressedStream.ToArray();

            return Convert.ToBase64String( compressedBytes );
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string UnZip( this string compressedString )
        {
            byte[] decompressedBytes;

            using var compressedStream = new MemoryStream( Convert.FromBase64String( compressedString ) );

            using var decompressorStream = new DeflateStream( compressedStream, CompressionMode.Decompress );

            using var decompressedStream = new MemoryStream();

            decompressorStream.CopyTo( decompressedStream );

            decompressedBytes = decompressedStream.ToArray();

            return Encoding.UTF8.GetString( decompressedBytes );
        }

        public static string Xml_Serializer( this object obj )
        {
            try
            {
                using System.IO.MemoryStream memstrm = new System.IO.MemoryStream();

                var xs = new System.Xml.Serialization.XmlSerializer( obj.GetType() );

                xs.Serialize( memstrm, obj );

                return Encoding.UTF8.GetString( memstrm.GetBuffer() );
            }
            catch
            {
                return string.Empty;
            }
        }

        public static object Xml_Deserializer( this string xml, object obj )
        {
            if ( xml.Empty() )
                return null;

            xml = xml.Trim();

            try
            {
                byte[] bytes = new byte[xml.Length];

                Encoding.UTF8.GetBytes( xml, 0, xml.Length, bytes, 0 );

                using System.IO.MemoryStream ms = new System.IO.MemoryStream( bytes, true );

                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer( obj.GetType() );

                obj = xs.Deserialize( ms );

                return obj;
            }
            catch
            {
                return null;
            }
        }

        public static long TryLong( this string value, long defaultValue = 0 )
        {
            var stripped = new string( value.Where( c => char.IsDigit( c ) ).ToArray() );

            if ( long.TryParse( stripped, out long result ) )
                return result;
            else
                return defaultValue;
        }

        public static int TryInt( this string value, int defaultValue = 0 )
        {
            var stripped = new string( value.Where( c => char.IsDigit( c ) ).ToArray() );

            if ( int.TryParse( stripped, out int result ) )
                return result;
            else
                return defaultValue;
        }

        public static string ToCamelCase( this string input )
        {
            if ( string.IsNullOrEmpty( input ) )
                return input;

            var first = input.Substring( 0, 1 ).ToLower();
            if ( input.Length == 1 )
                return first;

            return first + input.Substring( 1 );
        }

    }
}
