using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailServer.Hubs
{
    public class ConnectionMapping<T>
    {
        private static Dictionary<T, HashSet<string>> _connections;
        private static object lockObject = new object();
        private static ConnectionMapping<T> _instance;

        public int Count { get; private set; }

        public static ConnectionMapping<T> GetInstance()
        {
            lock (lockObject)
            {
                if (_connections == null)
                {
                    _instance = new ConnectionMapping<T>();
                    _connections = new Dictionary<T, HashSet<string>>();
                }
            }
            return _instance;
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }
                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }
                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }
            return Enumerable.Empty<string>();
        }
    }
}
