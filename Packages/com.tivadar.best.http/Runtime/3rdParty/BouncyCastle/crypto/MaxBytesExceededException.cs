#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.Runtime.Serialization;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	/// <summary>This exception is thrown whenever a cipher requires a change of key, IV or similar after x amount of
	/// bytes enciphered.
	/// </summary>
    [Serializable]
    public class MaxBytesExceededException
		: CryptoException
	{
		public MaxBytesExceededException()
			: base()
		{
		}

		public MaxBytesExceededException(string message)
			: base(message)
		{
		}

		public MaxBytesExceededException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected MaxBytesExceededException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
#pragma warning restore
#endif
