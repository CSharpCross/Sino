using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Sino.Extensions.Redis
{
    /// <summary>
    /// 缓存实现
    /// </summary>
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

        private string GetKeyString(string key)
        {
            var newKey = _instance + key;
            return newKey;
        }

        public long Append(string key, string value)
        {
            return _database.StringAppend(GetKeyString(key), value);
        }

        public Task<long> AppendAsync(string key, string value)
        {
            return _database.StringAppendAsync(GetKeyString(key), value);
        }

        public long BitCount(string key, long start = 0, long end = -1)
        {
            return _database.StringBitCount(GetKeyString(key), start, end);
        }

        public Task<long> BitCountAsync(string key, long start = 0, long end = -1)
        {
            return _database.StringBitCountAsync(GetKeyString(key), start, end);
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
            return _database.StringDecrement(GetKeyString(key));
        }

        public Task<long> DecrAsync(string key)
        {
            return _database.StringDecrementAsync(GetKeyString(key));
        }

        public long DecrBy(string key, long decrement)
        {
            return _database.StringDecrement(GetKeyString(key), decrement);
        }

        public Task<long> DecrByAsync(string key, long decrement)
        {
            return _database.StringDecrementAsync(GetKeyString(key), decrement);
        }

        public bool Exists(string key)
        {
            return _database.KeyExists(GetKeyString(key));
        }

        public Task<bool> ExistsAsync(string key)
        {
            return _database.KeyExistsAsync(GetKeyString(key));
        }

        public bool Expire(string key, int seconds)
        {
            return _database.KeyExpire(GetKeyString(key), TimeSpan.FromSeconds(seconds));
        }

        public Task<bool> ExpireAsync(string key, int seconds)
        {
            return _database.KeyExpireAsync(GetKeyString(key), TimeSpan.FromSeconds(seconds));
        }

        public string Get(string key)
        {
            return _database.StringGet(GetKeyString(key));
        }

        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(GetKeyString(key)).ConfigureAwait(false);
        }

        public bool GetBit(string key, uint offset)
        {
            return _database.StringGetBit(GetKeyString(key), offset);
        }

        public Task<bool> GetBitAsync(string key, uint offset)
        {
            return _database.StringGetBitAsync(GetKeyString(key), offset);
        }

        public string GetRange(string key, long start, long end)
        {
            return _database.StringGetRange(GetKeyString(key), start, end);
        }

        public async Task<string> GetRangeAsync(string key, long start, long end)
        {
            return await _database.StringGetRangeAsync(GetKeyString(key), start, end).ConfigureAwait(false);
        }

        public long HDel(string key, params string[] fields)
        {
            return _database.HashDelete(GetKeyString(key), fields.Select(x => (RedisValue)x).ToArray());
        }

        public Task<long> HDelAsync(string key, params string[] fields)
        {
            return _database.HashDeleteAsync(GetKeyString(key), fields.Select(x => (RedisValue)x).ToArray());
        }

        public bool HExists(string key, string field)
        {
            return _database.HashExists(GetKeyString(key), field);
        }

        public Task<bool> HExistsAsync(string key, string field)
        {
            return _database.HashExistsAsync(GetKeyString(key), field);
        }

        public string HGet(string key, string field)
        {
            return _database.HashGet(GetKeyString(key), field);
        }

        public async Task<string> HGetAsync(string key, string field)
        {
            return await _database.HashGetAsync(GetKeyString(key), field).ConfigureAwait(false);
        }

        public long HLen(string key)
        {
            return _database.HashLength(GetKeyString(key));
        }

        public Task<long> HLenAsync(string key)
        {
            return _database.HashLengthAsync(GetKeyString(key));
        }

        public bool HSet(string key, string field, string value)
        {
            return _database.HashSet(GetKeyString(key), field, value);
        }

        public Task<bool> HSetAsync(string key, string field, string value)
        {
            return _database.HashSetAsync(GetKeyString(key), field, value);
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
            return _database.StringIncrement(GetKeyString(key));
        }

        public Task<long> IncrAsync(string key)
        {
            return _database.StringIncrementAsync(GetKeyString(key));
        }

        public long IncrBy(string key, long increment)
        {
            return _database.StringIncrement(GetKeyString(key), increment);
        }

        public Task<long> IncrByAsync(string key, long increment)
        {
            return _database.StringIncrementAsync(GetKeyString(key), increment);
        }

        public string LIndex(string key, long index)
        {
            return _database.ListGetByIndex(GetKeyString(key), index);
        }

        public async Task<string> LIndexAsync(string key, long index)
        {
            return await _database.ListGetByIndexAsync(GetKeyString(key), index).ConfigureAwait(false);
        }

        public long LLen(string key)
        {
            return _database.ListLength(GetKeyString(key));
        }

        public Task<long> LLenAsync(string key)
        {
            return _database.ListLengthAsync(GetKeyString(key));
        }

        public string LPop(string key)
        {
            return _database.ListLeftPop(GetKeyString(key));
        }

        public async Task<string> LPopAsync(string key)
        {
            return await _database.ListLeftPopAsync(GetKeyString(key)).ConfigureAwait(false);
        }

        public long LPush(string key, params string[] values)
        {
            return _database.ListLeftPush(GetKeyString(key), values.Select(x => (RedisValue)x).ToArray());
        }

        public Task<long> LPushAsync(string key, params string[] values)
        {
            return _database.ListLeftPushAsync(GetKeyString(key), values.Select(x => (RedisValue)x).ToArray());
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
            return _database.ListRemove(GetKeyString(key), value, count);
        }

        public Task<long> LRemAsync(string key, long count, string value)
        {
            return _database.ListRemoveAsync(GetKeyString(key), value, count);
        }

        public bool PExpire(string key, long milliseconds)
        {
            return _database.KeyExpire(GetKeyString(key), TimeSpan.FromMilliseconds(milliseconds));
        }

        public Task<bool> PExpireAsync(string key, long milliseconds)
        {
            return _database.KeyExpireAsync(GetKeyString(key), TimeSpan.FromMilliseconds(milliseconds));
        }

        public void Refresh(string key, int timeout)
        {
            _database.KeyExpire(GetKeyString(key), TimeSpan.FromSeconds(timeout));
        }

        public async Task RefreshAsync(string key, int timeout)
        {
            await _database.KeyExpireAsync(GetKeyString(key), TimeSpan.FromSeconds(timeout)).ConfigureAwait(false);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(GetKeyString(key));
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(GetKeyString(key)).ConfigureAwait(false);
        }

        public string RPop(string key)
        {
            return _database.ListRightPop(GetKeyString(key));
        }

        public async Task<string> RPopAsync(string key)
        {
            return await _database.ListRightPopAsync(GetKeyString(key)).ConfigureAwait(false);
        }

        public string RPopLPush(string source, string destination)
        {
            return _database.ListRightPopLeftPush(source, destination);
        }

        public async Task<string> RPopLPushAsync(string source, string destination)
        {
            return await _database.ListRightPopLeftPushAsync(source, destination).ConfigureAwait(false);
        }

        public long RPush(string key, params string[] values)
        {
            return _database.ListRightPush(GetKeyString(key), values.Select(x => (RedisValue)x).ToArray());
        }

        public Task<long> RPushAsync(string key, params string[] values)
        {
            return _database.ListRightPushAsync(GetKeyString(key), values.Select(x => (RedisValue)x).ToArray());
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
            if (timeout == null)
            {
                _database.StringSet(GetKeyString(key), value);
            }
            else
            {
                _database.StringSet(GetKeyString(key), value, TimeSpan.FromSeconds(timeout.Value));
            }
        }

        public async Task SetAsync(string key, string value, int? timeout = null)
        {
            if (timeout == null)
            {
                await _database.StringSetAsync(GetKeyString(key), value).ConfigureAwait(false);
            }
            else
            {
                await _database.StringSetAsync(GetKeyString(key), value, TimeSpan.FromSeconds(timeout.Value)).ConfigureAwait(false);
            }
        }

        public bool SetBit(string key, uint offset, bool value)
        {
            return _database.StringSetBit(GetKeyString(key), offset, value);
        }

        public Task<bool> SetBitAsync(string key, uint offset, bool value)
        {
            return _database.StringSetBitAsync(GetKeyString(key), offset, value);
        }

        public bool SetNx(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSet(GetKeyString(key), value, expiry: expiry, when: When.NotExists);
        }

        public Task<bool> SetNxAsync(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSetAsync(GetKeyString(key), value, expiry: expiry, when: When.NotExists);
        }

        public long StrLen(string key)
        {
            return _database.StringLength(GetKeyString(key));
        }

        public Task<long> StrLenAsync(string key)
        {
            return _database.StringLengthAsync(GetKeyString(key));
        }

        public IDatabase GetDatabase()
        {
            return _database;
        }
    }
}
