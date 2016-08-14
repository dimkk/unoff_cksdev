using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    internal sealed class StrongNameKey
    {
        byte[] _keyBuffer;
        string _keyContainer;

        StrongNameKey(byte[] buffer)
        {
            this._keyBuffer = buffer;
        }

        StrongNameKey(string keyContainer)
        {
            this._keyContainer = keyContainer;
        }

        internal static StrongNameKey CreateNewKeyPair()
        {
            byte[] buffer;
            IntPtr zero = IntPtr.Zero;
            try
            {
                uint num;
                if (NativeMethods.StrongNameKeyGen(IntPtr.Zero, 0, out zero, out num) == 0)
                {
                    Marshal.ThrowExceptionForHR(NativeMethods.StrongNameErrorInfo());
                }
                if (zero == IntPtr.Zero)
                {
                    throw new InvalidOperationException("StrongNameKeyGenerationFailed");
                }
                buffer = new byte[num];
                Marshal.Copy(zero, buffer, 0, (int)num);
            }
            finally
            {
                NativeMethods.StrongNameFreeBuffer(zero);
            }
            return new StrongNameKey(buffer);
        }

        private byte[] GetPublicKey()
        {
            byte[] buffer;
            IntPtr zero = IntPtr.Zero;
            uint publicKeyBlobSize = 0;
            try
            {
                uint keyBlobSize = 0;
                if (this._keyBuffer != null)
                {
                    keyBlobSize = (uint)this._keyBuffer.Length;
                }
                if (NativeMethods.StrongNameGetPublicKey(this._keyContainer, this._keyBuffer, keyBlobSize, out zero, out publicKeyBlobSize) == 0)
                {
                    throw Marshal.GetExceptionForHR(NativeMethods.StrongNameErrorInfo());
                }
                if (publicKeyBlobSize == 0)
                {
                    throw new InvalidOperationException("StrongNameKeyGettingPublicKeyFailed");
                }
                buffer = new byte[publicKeyBlobSize];
                Marshal.Copy(zero, buffer, 0, (int)publicKeyBlobSize);
            }
            finally
            {
                NativeMethods.StrongNameFreeBuffer(zero);
            }
            return buffer;
        }

        internal string GetPublicKeyToken()
        {
            IntPtr zero = IntPtr.Zero;
            int strongNameTokenCount = 0;
            StringBuilder builder = new StringBuilder();
            try
            {
                byte[] publicKey = this.GetPublicKey();
                NativeMethods.StrongNameTokenFromPublicKey(publicKey, publicKey.Length, out zero, out strongNameTokenCount);
                if (strongNameTokenCount == 0)
                {
                    throw new InvalidOperationException("StrongNameKeyExtractingPublicKeyFailed");
                }
                for (int i = 0; i < strongNameTokenCount; i++)
                {
                    builder.Append(Marshal.ReadByte(zero, i).ToString("x02", CultureInfo.InvariantCulture));
                }
            }
            finally
            {
                NativeMethods.StrongNameFreeBuffer(zero);
            }
            return builder.ToString();
        }

        internal static StrongNameKey Load(string path)
        {
            return new StrongNameKey(File.ReadAllBytes(path));
        }

        internal static StrongNameKey LoadContainer(string keyContainer)
        {
            return new StrongNameKey(keyContainer);
        }

        internal void SaveTo(string destinationFileName)
        {
            File.WriteAllBytes(destinationFileName, this._keyBuffer);
        }
    }
}

