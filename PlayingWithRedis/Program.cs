using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingWithRedis
{
    class Program
    {
        private const string ConnectionString = "...";

        static void Main(string[] args)
        {
            var connection = ConnectionMultiplexer.Connect(ConnectionString);
            var database = connection.GetDatabase();
        }
    }
}
