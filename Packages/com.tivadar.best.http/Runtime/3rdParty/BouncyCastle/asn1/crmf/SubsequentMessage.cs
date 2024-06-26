#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
    public class SubsequentMessage
        : DerInteger
    {
        public static readonly SubsequentMessage encrCert = new SubsequentMessage(0);
        public static readonly SubsequentMessage challengeResp = new SubsequentMessage(1);
    
        private SubsequentMessage(int value)
            : base(value)
        {
        }

        public static SubsequentMessage ValueOf(int value)
        {
            if (value == 0)
                return encrCert;

            if (value == 1)
                return challengeResp;

            throw new ArgumentException("unknown value: " + value, "value");
        }
    }
}
#pragma warning restore
#endif
