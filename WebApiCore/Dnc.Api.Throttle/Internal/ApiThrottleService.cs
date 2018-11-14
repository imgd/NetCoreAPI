﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dnc.Api.Throttle.Internal
{
    internal class ApiThrottleService : IApiThrottleService
    {
        private readonly ICacheProvider _cache;
        private readonly IStorageProvider _storage;
        private readonly ApiThrottleOptions _options;

        public ApiThrottleService(ICacheProvider cache, IOptions<ApiThrottleOptions> options, IStorageProvider storage)
        {
            _cache = cache;
            _options = options.Value;
            _storage = storage;
        }

        #region 黑名单 & 白名单

        /// <summary>
        /// 添加名单
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="item">项目</param>
        /// <remarks>因为要保存过期时间，所以名单通过Redis 有序集合(sorted set)来存储，score来存储过期时间Ticks</remarks>
        public async Task AddRosterAsync(RosterType rosterType, string api, Policy policy, string policyKey, TimeSpan? expiry, params string[] item)
        {
            //保存名单
            await _storage.AddRosterAsync(rosterType, api, policy, policyKey, expiry, item);

            //从反名单中移除
            await _storage.RemoveRosterAsync(rosterType == RosterType.BlackList ? RosterType.WhiteList : RosterType.BlackList, api, policy, policyKey, item);

            //清除缓存
            await _cache.ClearRosterListCacheAsync(rosterType, api, policy, policyKey);
            await _cache.ClearRosterListCacheAsync(rosterType == RosterType.BlackList ? RosterType.WhiteList : RosterType.BlackList, api, policy, policyKey);
        }

        /// <summary>
        /// 添加全局名单
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="item">项目</param>
        public async Task AddGlobalRosterAsync(RosterType rosterType, Policy policy, string policyKey, TimeSpan? expiry, params string[] item)
        {
            await AddRosterAsync(rosterType, Common.GlobalApiKey, policy, policyKey, expiry, item);
        }

        /// <summary>
        /// 移除名单
        /// </summary>
        /// <param name="policy">策略</param>
        /// <param name="item">项目</param>
        public async Task RemoveRosterAsync(RosterType rosterType, string api, Policy policy, string policyKey, params string[] item)
        {
            //从名单中移除
            await _storage.RemoveRosterAsync(rosterType, api, policy, policyKey, item);
            //清除缓存
            await _cache.ClearRosterListCacheAsync(rosterType, api, policy, policyKey);
        }

        /// <summary>
        /// 移除全局名单
        /// </summary>
        /// <param name="policy">策略</param>
        /// <param name="item">项目</param>
        public async Task RemoveGlobalRosterAsync(RosterType rosterType, Policy policy, string policyKey, params string[] item)
        {
            await RemoveRosterAsync(rosterType, Common.GlobalApiKey, policy, policyKey, item);
        }

        /// <summary>
        /// 取得名单列表（分页）
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="api">API</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<(long count, IEnumerable<ListItem> items)> GetRosterListAsync(RosterType rosterType, string api, Policy policy, string policyKey, long skip, long take)
        {
            return await _storage.GetRosterListAsync(rosterType, api, policy, policyKey, skip, take);
        }

        /// <summary>
        /// 取得全局名单列表（分页）
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<(long count, IEnumerable<ListItem> items)> GetGlobalRosterListAsync(RosterType rosterType, Policy policy, string policyKey, long skip, long take)
        {
            return await GetRosterListAsync(rosterType, Common.GlobalApiKey, policy, policyKey, skip, take);
        }

        /// <summary>
        /// 取得名单列表
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="api">API</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<IEnumerable<ListItem>> GetRosterListAsync(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            return await _storage.GetRosterListAsync(rosterType, api, policy, policyKey);
        }

        /// <summary>
        /// 取得全局名单列表
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<IEnumerable<ListItem>> GetGlobalRosterListAsync(RosterType rosterType, Policy policy, string policyKey)
        {
            return await GetRosterListAsync(rosterType, Common.GlobalApiKey, policy, policyKey);
        }

        #endregion

    }
}
