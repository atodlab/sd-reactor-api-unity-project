#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.Collections.Generic;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Security
{
    /// <remarks>
    ///  Utility class for creating IWrapper objects from their names/Oids
    /// </remarks>
    public static class WrapperUtilities
    {
        private enum WrapAlgorithm { AESWRAP, CAMELLIAWRAP, DESEDEWRAP, RC2WRAP, SEEDWRAP,
            DESEDERFC3211WRAP, AESRFC3211WRAP, CAMELLIARFC3211WRAP };

        private static readonly IDictionary<string, string> Algorithms =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static WrapperUtilities()
        {
            // Signal to obfuscation tools not to change enum constants
            Enums.GetArbitraryValue<WrapAlgorithm>().ToString();

            Algorithms[NistObjectIdentifiers.IdAes128Wrap.Id] = "AESWRAP";
            Algorithms[NistObjectIdentifiers.IdAes192Wrap.Id] = "AESWRAP";
            Algorithms[NistObjectIdentifiers.IdAes256Wrap.Id] = "AESWRAP";

            Algorithms[NttObjectIdentifiers.IdCamellia128Wrap.Id] = "CAMELLIAWRAP";
            Algorithms[NttObjectIdentifiers.IdCamellia192Wrap.Id] = "CAMELLIAWRAP";
            Algorithms[NttObjectIdentifiers.IdCamellia256Wrap.Id] = "CAMELLIAWRAP";

            Algorithms[PkcsObjectIdentifiers.IdAlgCms3DesWrap.Id] = "DESEDEWRAP";
            Algorithms["TDEAWRAP"] = "DESEDEWRAP";

            Algorithms[PkcsObjectIdentifiers.IdAlgCmsRC2Wrap.Id] = "RC2WRAP";

            Algorithms[KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap.Id] = "SEEDWRAP";
        }

        public static IWrapper GetWrapper(DerObjectIdentifier oid)
        {
            return GetWrapper(oid.Id);
        }

        public static IWrapper GetWrapper(string algorithm)
        {
            string mechanism = CollectionUtilities.GetValueOrKey(Algorithms, algorithm).ToUpperInvariant();

            try
            {
                WrapAlgorithm wrapAlgorithm = Enums.GetEnumValue<WrapAlgorithm>(mechanism);

                switch (wrapAlgorithm)
                {
                case WrapAlgorithm.AESWRAP:				return new AesWrapEngine();
                case WrapAlgorithm.CAMELLIAWRAP:		return new CamelliaWrapEngine();
                case WrapAlgorithm.DESEDEWRAP:			return new DesEdeWrapEngine();
                case WrapAlgorithm.RC2WRAP:				return new RC2WrapEngine();
                case WrapAlgorithm.SEEDWRAP:			return new SeedWrapEngine();
                case WrapAlgorithm.DESEDERFC3211WRAP:	return new Rfc3211WrapEngine(new DesEdeEngine());
                case WrapAlgorithm.AESRFC3211WRAP:		return new Rfc3211WrapEngine(AesUtilities.CreateEngine());
                case WrapAlgorithm.CAMELLIARFC3211WRAP:	return new Rfc3211WrapEngine(new CamelliaEngine());
                }
            }
            catch (ArgumentException)
            {
            }

            // Create an IBufferedCipher and use it as IWrapper (via BufferedCipherWrapper)
            IBufferedCipher blockCipher = CipherUtilities.GetCipher(algorithm);

            if (blockCipher != null)
                return new BufferedCipherWrapper(blockCipher);

            throw new SecurityUtilityException("Wrapper " + algorithm + " not recognised.");
        }

        public static string GetAlgorithmName(DerObjectIdentifier oid)
        {
            return CollectionUtilities.GetValueOrNull(Algorithms, oid.Id);
        }

        private class BufferedCipherWrapper
            : IWrapper
        {
            private readonly IBufferedCipher cipher;
            private bool forWrapping;

            public BufferedCipherWrapper(
                IBufferedCipher cipher)
            {
                this.cipher = cipher;
            }

            public string AlgorithmName
            {
                get { return cipher.AlgorithmName; }
            }

            public void Init(
                bool				forWrapping,
                ICipherParameters	parameters)
            {
                this.forWrapping = forWrapping;

                cipher.Init(forWrapping, parameters);
            }

            public byte[] Wrap(
                byte[]	input,
                int		inOff,
                int		length)
            {
                if (!forWrapping)
                    throw new InvalidOperationException("Not initialised for wrapping");

                return cipher.DoFinal(input, inOff, length);
            }

            public byte[] Unwrap(
                byte[]	input,
                int		inOff,
                int		length)
            {
                if (forWrapping)
                    throw new InvalidOperationException("Not initialised for unwrapping");

                return cipher.DoFinal(input, inOff, length);
            }
        }
    }
}
#pragma warning restore
#endif
