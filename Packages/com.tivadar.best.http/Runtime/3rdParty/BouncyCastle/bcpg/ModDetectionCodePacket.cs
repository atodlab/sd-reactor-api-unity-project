#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Bcpg
{
	/// <remarks>Basic packet for a modification detection code packet.</remarks>
    public class ModDetectionCodePacket
        : ContainedPacket
    {
        private readonly byte[] digest;

		internal ModDetectionCodePacket(
            BcpgInputStream bcpgIn)
        {
			if (bcpgIn == null)
				throw new ArgumentNullException("bcpgIn");

			this.digest = new byte[20];
            bcpgIn.ReadFully(this.digest);
        }

		public ModDetectionCodePacket(
            byte[] digest)
        {
			if (digest == null)
				throw new ArgumentNullException("digest");

			this.digest = (byte[]) digest.Clone();
        }

		public byte[] GetDigest()
        {
			return (byte[]) digest.Clone();
        }

		public override void Encode(
            BcpgOutputStream bcpgOut)
        {
            bcpgOut.WritePacket(PacketTag.ModificationDetectionCode, digest, false);
        }
    }
}
#pragma warning restore
#endif
