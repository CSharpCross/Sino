using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.Extensions.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly string _instance;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCache(IOptions<RedisCacheOptions> optionsAccessor)
            : this(optionsAccessor?.Value) { }

        public RedisCache(RedisCacheOptions options)
        {
            _instance = options.InstanceName ?? string.Empty;

            if (string.IsNullOrEmpty(options.Host))
            {
                throw new ArgumentNullException(nameof(options.Host));
            }

            var redisConfig = new ConfigurationOptions()
            {
                AbortOnConnectFail = options.AbortOnConnectFail,
                AllowAdmin = options.AllowAdmin,
                ConnectRetry = options.ConnectRetry,
                ConnectTimeout = options.ConnectTimeout,
                DefaultDatabase = options.DefaultDatabase,
                Ssl = options.Ssl,
                SslHost = options.SslHost,
                User = options.User,
                Password = options.Password
            };

            redisConfig.EndPoints.Add(options.Host, options.Port);

            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfig);
            _database = _connectionMultiplexer.GetDatabase();
        }

        public long Append(string key, string value)
        {
            return _database.StringAppend(key, value);
        }

        public Task<long> AppendAsync(string key, string value)
        {
            return _database.StringAppendAsync(key, value);
        }

        public long BitCount(string key, long start = 0, long end = -1)
        {
            return _database.StringBitCount(key, start, end);
        }

        public Task<long> BitCountAsync(string key, long start = 0, long end = -1)
        {
            return _database.StringBitCountAsync(key, start, end);
        }

        public Tuple<string, string> BLPop(int timeout, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, string>> BLPopAsync(int timeout, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, string> BRPop(int timeout, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, string>> BRPopAsync(int timeout, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public string BRPopLPush(string source, string destination, int timeout)
        {
            throw new NotImplementedException();
        }

        public Task<string> BRPopLPushAsync(string source, string destination, int timeout)
        {
            throw new NotImplementedException();
        }

        public long Decr(string key)
        {
            return _database.StringDecrement(key);
        }

        public Task<long> DecrAsync(string key)
        {
            return _database.StringDecrementAsync(key);
        }

        public long DecrBy(string key, long decrement)
        {
            return _database.StringDecrement(key, decrement);
        }

        public Task<long> DecrByAsync(string key, long decrement)
        {
            return _database.StringDecrementAsync(key, decrement);
        }

        public bool Exists(string key)
        {
            return _database.KeyExists(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return _database.KeyExistsAsync(key);
        }

        public bool Expire(string key, int seconds)
        {
            return _database.KeyExpire(key, TimeSpan.FromSeconds(seconds));
        }

        public Task<bool> ExpireAsync(string key, int seconds)
        {
            return _database.KeyExpireAsync(key, TimeSpan.FromSeconds(seconds));
        }

        public string Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public bool GetBit(string key, uint offset)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetBitAsync(string key, uint offset)
        {
            throw new NotImplementedException();
        }

        public string GetRange(string key, long start, long end)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRangeAsync(string key, long start, long end)
        {
            throw new NotImplementedException();
        }

        public long HDel(string key, params string[] fields)
        {
            throw new NotImplementedException();
        }

        public Task<long> HDelAsync(string key, params string[] fields)
        {
            throw new NotImplementedException();
        }

        public bool HExists(string key, string field)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HExistsAsync(string key, string field)
        {
            throw new NotImplementedException();
        }

        public string HGet(string key, string field)
        {
            throw new NotImplementedException();
        }

        public Task<string> HGetAsync(string key, string field)
        {
            throw new NotImplementedException();
        }

        public long HLen(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> HLenAsync(string key)
        {
            throw new NotImplementedException();
        }

        public bool HSet(string key, string field, string value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HSetAsync(string key, string field, string value)
        {
            throw new NotImplementedException();
        }

        public bool HSetNx(string key, string field, string value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HSetNxAsync(string key, string field, string value)
        {
            throw new NotImplementedException();
        }

        public long Incr(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> IncrAsync(string key)
        {
            throw new NotImplementedException();
        }

        public long IncrBy(string key, long increment)
        {
            throw new NotImplementedException();
        }

        public Task<long> IncrByAsync(string key, long increment)
        {
            throw new NotImplementedException();
        }

        public string LIndex(string key, long index)
        {
            throw new NotImplementedException();
        }

        public Task<string> LIndexAsync(string key, long index)
        {
            throw new NotImplementedException();
        }

        public long LLen(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> LLenAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string LPop(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> LPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public long LPush(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushAsync(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public long LPushX(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushXAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public long LRem(string key, long count, string value)
        {
            throw new NotImplementedException();
        }

        public Task<long> LRemAsync(string key, long count, string value)
        {
            throw new NotImplementedException();
        }

        public bool PExpire(string key, long milliseconds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PExpireAsync(string key, long milliseconds)
        {
            throw new NotImplementedException();
        }

        public void Refresh(string key, int timeout)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, int timeout)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string RPop(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> RPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string RPopLPush(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Task<string> RPopLPushAsync(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public long RPush(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushAsync(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public long RPushX(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushXAsync(string key, params string[] values)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, string value, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, string value, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public bool SetBit(string key, uint offset, bool value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetBitAsync(string key, uint offset, bool value)
        {
            throw new NotImplementedException();
        }

        public bool SetNx(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetNxAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public long StrLen(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> StrLenAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
