#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Tls.Crypto;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Tls
{
    /// <summary>Base interface for a class that decrypts TLS secrets.</summary>
    public interface TlsCredentialedDecryptor
        : TlsCredentials
    {
        /// <summary>Decrypt the passed in cipher text using the parameters available.</summary>
        /// <param name="cryptoParams">the parameters to use for the decryption.</param>
        /// <param name="ciphertext">the cipher text containing the secret.</param>
        /// <returns>a TLS secret.</returns>
        /// <exception cref="IOException">on a parsing or decryption error.</exception>
        TlsSecret Decrypt(TlsCryptoParameters cryptoParams, byte[] ciphertext);
    }
}
#pragma warning restore
#endif
