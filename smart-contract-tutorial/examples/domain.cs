using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Neo.SmartContract
{
    public class Domain : Framework.SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case "Query":
                    return Query((string)args[0]);
                case "Register":
                    return Register((string)args[0], (byte[])args[1]);
                case "Transfer":
                    return Transfer((string)args[0], (byte[])args[1]);
                case "Delete":
                    return Delete((string)args[0]);
                default:
                    return false;
            }
        }

        public static byte[] Query(string domain)
        {
            return Storage.Get(Storage.CurrentContext, domain);
        }

        public static bool Register(string domain, byte[] owner)
        {
            if (!Runtime.CheckWitness(owner)) return false;
            byte[] value = Storage.Get(Storage.CurrentContext, domain);
            if (value != null) return false;
            Storage.Put(Storage.CurrentContext, domain, owner);
            return true;
        }

        public static bool Transfer(string domain, byte[] to)
        {
            byte[] from = Storage.Get(Storage.CurrentContext, domain);
            if (from == null) return false;
            if (!Runtime.CheckWitness(from)) return false;
            Storage.Put(Storage.CurrentContext, domain, to);
            return true;
        }

        public static bool Delete(string domain)
        {
            byte[] owner = Storage.Get(Storage.CurrentContext, domain);
            if (owner == null) return false;
            if (!Runtime.CheckWitness(owner)) return false;
            Storage.Delete(Storage.CurrentContext, domain);
            return true;
        }
    }
}
