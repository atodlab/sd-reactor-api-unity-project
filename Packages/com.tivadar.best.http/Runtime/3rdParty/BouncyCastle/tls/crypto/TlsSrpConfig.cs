#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Tls.Crypto
{
    /// <summary>Basic config for SRP.</summary>
    public class TlsSrpConfig
    {
        protected BigInteger[] m_explicitNG;

        /// <summary>Return the (N, g) values used in SRP-6.</summary>
        /// <returns>(N, g) as a BigInteger array (N=[0], g=[1]).</returns>
        public BigInteger[] GetExplicitNG()
        {
            return (BigInteger[])m_explicitNG.Clone();
        }

        /// <summary>Set the (N, g) values used for SRP-6.</summary>
        /// <param name="explicitNG">(N, g) as a BigInteger array (N=[0], g=[1]).</param>
        public void SetExplicitNG(BigInteger[] explicitNG)
        {
            this.m_explicitNG = (BigInteger[])explicitNG.Clone();
        }
    }
}
#pragma warning restore
#endif
