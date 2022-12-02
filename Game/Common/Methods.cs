using System;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicTripleTrouble16bit
{
    /// <summary>
    /// Custom extension methods used in this autosplitter
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Perform a signature scan, similarly to how it would achieve with SignatureScanner.Scan()
        /// </summary>
        /// <returns>Address of the signature, if found. Otherwise, an Exception will be thrown</returns>
        public static IntPtr ScanOrThrow(this SignatureScanner ss, SigScanTarget sst)
        {
            IntPtr tempAddr = ss.Scan(sst);
            CheckPtr(tempAddr);
            return tempAddr;
        }

        /// <summary>
        /// Checks whether a provided IntPtr is equal to IntPtr.Zero. If it is, an Exception will be thrown
        /// </summary>
        /// <param name="ptr"></param>
        /// <exception cref="SigscanFailedException"></exception>
        public static void CheckPtr(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new SigscanFailedException();
        }
    }

    public class SigscanFailedException : Exception
    {
        public SigscanFailedException() { }
        public SigscanFailedException(string message) : base(message) { }
    }
}