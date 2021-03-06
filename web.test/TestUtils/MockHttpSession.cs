using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace web.test.TestUtils
{
    /*
     * How to Mock Session variables in ASP.net core unit testing project?
     * https://stackoverflow.com/a/42292876
     * ここのコードを元にちょっといじったもの
     */

    internal class MockHttpSession : ISession
    {
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public object this[string name]
        {
            get { return sessionStorage[name]; }
            set { sessionStorage[name] = value; }
        }

        public IEnumerable<string> Keys => throw new NotImplementedException();

        string ISession.Id
        {
            get => throw new NotImplementedException();
        }

        bool ISession.IsAvailable
        {
            get => throw new NotImplementedException();
        }

        IEnumerable<string> ISession.Keys
        {
            get => sessionStorage.Keys;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task LoadAsync(CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        void ISession.Clear() => sessionStorage.Clear();

        public void Remove(string key) => sessionStorage.Remove(key);

        void ISession.Set(string key, byte[] value)
            => sessionStorage[key] = value;

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (sessionStorage.ContainsKey(key))
            {
                value = (byte[])sessionStorage[key]; //Encoding.UTF8.GetBytes(sessionStorage[key].ToString())
                //value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
