using System;
using System.Security.Cryptography;

namespace Developer.Service.Utils;

public static class Cryptography
{
    public static string GetKeyBase64String(int keyLength) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(keyLength));

    public static string GetKeyHexString(int keyLength) =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(keyLength));

    public static char[] GetKeyBase64CharArray(int keyLength, int offset, int length)
    {
        char[] result = new char[length];

        Convert.ToBase64CharArray(RandomNumberGenerator.GetBytes(keyLength), offset, length, result, 0);

        return result;
    }
        
}
