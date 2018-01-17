﻿using System;

namespace TeeGonSdk
{
    /// <summary>
    /// TOP客户端。
    /// </summary>
    public interface ITopClient
    {
        /// <summary>
        /// 等待请求开始返回的超时时间
        /// </summary>
        void SetTimeout(int timeout);

        /// <summary>
        /// 是否禁用解析
        /// </summary>
        void SetDisableParser(bool disableParser);

        /// <summary>
        /// 执行TOP公开API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的TOP API请求</param>
        /// <returns>领域对象</returns>
        T Execute<T>(ITopRequest<T> request) where T : TopResponse;

        /// <summary>
        /// 执行TOP隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的TOP API请求</param>
        /// <param name="session">用户会话码</param>
        /// <returns>领域对象</returns>
        T Execute<T>(ITopRequest<T> request, string session) where T : TopResponse;

        /// <summary>
        /// 执行TOP隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的TOP API请求</param>
        /// <param name="session">用户会话码</param>
        /// <param name="timestamp">请求时间戳</param>
        /// <returns>领域对象</returns>
        T Execute<T>(ITopRequest<T> request, string session, DateTime timestamp) where T : TopResponse;
    }
}
