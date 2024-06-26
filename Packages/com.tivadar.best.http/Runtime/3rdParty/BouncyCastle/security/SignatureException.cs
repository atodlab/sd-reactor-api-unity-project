#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.Runtime.Serialization;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Security
{
    [Serializable]
    public class SignatureException
		: GeneralSecurityException
	{
		public SignatureException()
			: base()
		{
		}

		public SignatureException(string message)
			: base(message)
		{
		}

		public SignatureException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected SignatureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
#pragma warning restore
#endif
