using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Security;

public class NgRandom
{
    public String CreateRandomPassword(int PasswordLength)
    {
        string _allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
        Byte[] randomBytes = new Byte[PasswordLength];
        char[] chars = new char[PasswordLength];
        int allowedCharCount = _allowedChars.Length;

        for (int i = 0; i < PasswordLength; i++)
        {
            Random randomObj = new Random();
            randomObj.NextBytes(randomBytes);
            chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
        }

        return new string(chars);
    }

    public String CreateRandomPassword_Numeros(int PasswordLength)
    {
        string _allowedChars = "0123456789";
        Byte[] randomBytes = new Byte[PasswordLength];
        char[] chars = new char[PasswordLength];
        int allowedCharCount = _allowedChars.Length;

        for (int i = 0; i < PasswordLength; i++)
        {
            Random randomObj = new Random();
            randomObj.NextBytes(randomBytes);
            chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
        }

        return new string(chars);
    }

    public String CreateRandomLetras(int PasswordLength)
    {
        string _allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        Byte[] randomBytes = new Byte[PasswordLength];
        char[] chars = new char[PasswordLength];
        int allowedCharCount = _allowedChars.Length;

        for (int i = 0; i < PasswordLength; i++)
        {
            Random randomObj = new Random();
            randomObj.NextBytes(randomBytes);
            chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
        }

        return new string(chars);
    }
}
