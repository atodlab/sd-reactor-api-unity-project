#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
    public sealed class Ed25519PublicKeyParameters
        : AsymmetricKeyParameter
    {
        public static readonly int KeySize = Ed25519.PublicKeySize;

        private readonly byte[] data = new byte[KeySize];

        public Ed25519PublicKeyParameters(byte[] buf)
            : this(Validate(buf), 0)
        {
        }

        public Ed25519PublicKeyParameters(byte[] buf, int off)
            : base(false)
        {
            Array.Copy(buf, off, data, 0, KeySize);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        public Ed25519PublicKeyParameters(ReadOnlySpan<byte> buf)
            : base(false)
        {
            if (buf.Length != KeySize)
                throw new ArgumentException("must have length " + KeySize, nameof(buf));

            buf.CopyTo(data);
        }
#endif

        public Ed25519PublicKeyParameters(Stream input)
            : base(false)
        {
            if (KeySize != Streams.ReadFully(input, data))
                throw new EndOfStreamException("EOF encountered in middle of Ed25519 public key");
        }

        public void Encode(byte[] buf, int off)
        {
            Array.Copy(data, 0, buf, off, KeySize);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        public void Encode(Span<byte> buf)
        {
            data.CopyTo(buf);
        }
#endif

        public byte[] GetEncoded()
        {
            return Arrays.Clone(data);
        }

        private static byte[] Validate(byte[] buf)
        {
            if (buf.Length != KeySize)
                throw new ArgumentException("must have length " + KeySize, nameof(buf));

            return buf;
        }
    }
}
#pragma warning restore
#endif
