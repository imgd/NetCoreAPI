﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dnc.Api.Throttle.Redis.Cache
{
    public class RedisCacheOptions : RedisOptions
    {
        internal bool SameWithStorage { set; get; } = false;

        /// <summary>
        /// Redis Key Prefix
        /// </summary>
        internal string CacheKeyPrefix
        {
            get
            {
                return KeyPrefix + ":cache";
            }
        }
    }
}
