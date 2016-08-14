using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    static class NativeMethods
    {
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int StrongNameKeyGen(IntPtr wszKeyContainer, uint dwFlags, out IntPtr KeyBlob, out uint KeyBlobSize);

        [DllImport("mscoree.dll", ExactSpelling = true, PreserveSig = false)]
        internal static extern void StrongNameTokenFromPublicKey(byte[] publicKeyBlob, int publicKeyBlobCount, out IntPtr strongNameTokenArray, out int strongNameTokenCount);

        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int StrongNameGetPublicKey(string wszKeyContainer, [In] byte[] KeyBlob, [In] uint KeyBlobSize, out IntPtr PublicKeyBlob, out uint PublicKeyBlobSize);

        [DllImport("mscoree.dll")]
        internal static extern int StrongNameFreeBuffer(IntPtr pbMemory);

        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        internal static extern int StrongNameErrorInfo();

        internal static int HResultFromWin32(int errorCode)
        {
            if (errorCode > 0)
            {
                return (((errorCode & 0xffff) | 0x70000) | -2147483648);
            }
            return errorCode;
        }
    }
}
