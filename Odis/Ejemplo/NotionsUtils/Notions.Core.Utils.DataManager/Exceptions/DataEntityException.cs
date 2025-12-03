using System;

namespace Notions.Core.Utils.DataManager.Exceptions;
[Serializable]
public class DataEntityException : Exception
{
    public DataEntityException() { }
    public DataEntityException(string message) : base(message) { }
    public DataEntityException(string message, Exception inner) : base(message, inner) { }

}
