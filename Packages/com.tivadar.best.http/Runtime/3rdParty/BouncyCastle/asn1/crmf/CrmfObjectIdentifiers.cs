#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
    public abstract class CrmfObjectIdentifiers
    {
        public static readonly DerObjectIdentifier id_pkix = new DerObjectIdentifier("1.3.6.1.5.5.7");

        // arc for Internet X.509 PKI protocols and their components

        public static readonly DerObjectIdentifier id_pkip  = id_pkix.Branch("5");

        public static readonly DerObjectIdentifier id_regCtrl = id_pkip.Branch("1");
        public static readonly DerObjectIdentifier id_regCtrl_regToken = id_regCtrl.Branch("1");
        public static readonly DerObjectIdentifier id_regCtrl_authenticator = id_regCtrl.Branch("2");
        public static readonly DerObjectIdentifier id_regCtrl_pkiPublicationInfo = id_regCtrl.Branch("3");
        public static readonly DerObjectIdentifier id_regCtrl_pkiArchiveOptions = id_regCtrl.Branch("4");

        public static readonly DerObjectIdentifier id_ct_encKeyWithID = new DerObjectIdentifier(PkcsObjectIdentifiers.IdCT + ".21");
    }
}
#pragma warning restore
#endif
