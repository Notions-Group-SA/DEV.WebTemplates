using System;
using System.Configuration;

namespace Notions.Core.Utils.DataManager;


public class ConnectionString
{
    private static string _Value;
    private static string _ConnName;

    public ConnectionString(string connection)
    {
        _Value = connection;
    }

    public static string Value
    {
        get
        {
            return _Value;
        }
    }

    public static string ConnName
    {
        get
        {
            return _ConnName;
        }
        set
        {
            _ConnName = value;
        }
    }
}