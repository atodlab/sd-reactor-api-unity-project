#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
    /// <summary>
    /// Base interface for operators that serve as stream-based signature calculators.
    /// </summary>
    public interface ISignatureFactory
	{
        /// <summary>The algorithm details object for this calculator.</summary>
        object AlgorithmDetails { get; }

        /// <summary>
        /// Create a stream calculator for this signature calculator. The stream
        /// calculator is used for the actual operation of entering the data to be signed
        /// and producing the signature block.
        /// </summary>
        /// <returns>A calculator producing an IBlockResult with a signature in it.</returns>
        IStreamCalculator<IBlockResult> CreateCalculator();
    }
}
#pragma warning restore
#endif
