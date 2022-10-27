using System;
using System.IO;
using System.Threading.Tasks;

namespace Aliyun.OSS
{
    /// <summary>
    /// <see cref="OssClient"/> 扩展
    /// </summary>
    public static class OssClientExtensions
    {
        /// <summary>
        /// BeginPutObject EndPutObject TAP
        /// </summary>
        /// <typeparam name="PutObjectResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="putObjectRequest"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task<Aliyun.OSS.PutObjectResult> PutObjectAsync<PutObjectResult>(this OssClient client, PutObjectRequest putObjectRequest, object state = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            return Task<Aliyun.OSS.PutObjectResult>.Factory.FromAsync(client.BeginPutObject, client.EndPutObject, putObjectRequest, state);
        }
        /// <summary>
        /// BeginPutObject EndPutObject TAP
        /// </summary>
        /// <typeparam name="PutObjectResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task<Aliyun.OSS.PutObjectResult> PutObjectAsync<PutObjectResult>(this OssClient client, string bucketName, string key, Stream content, object state = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            return Task<Aliyun.OSS.PutObjectResult>.Factory.FromAsync(client.BeginPutObject, client.EndPutObject, bucketName, key, content, state);
        }
        /// <summary>
        /// BeginPutObject EndPutObject TAP
        /// </summary>
        /// <typeparam name="PutObjectResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="metadata"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<Aliyun.OSS.PutObjectResult> PutObjectAsync<PutObjectResult>(this OssClient client, string bucketName, string key, Stream content, ObjectMetadata metadata, object state = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            var tcs = new TaskCompletionSource<Aliyun.OSS.PutObjectResult>(state);
            client.BeginPutObject(bucketName, key, content, metadata, iar =>
            {
                try
                {
                    tcs.TrySetResult(client.EndPutObject(iar));
                }
                catch (OperationCanceledException)
                {
                    tcs.TrySetCanceled();
                }
                catch (Exception exc)
                {
                    tcs.TrySetException(exc);
                }
            }, state);
            return tcs.Task;
        }
        /// <summary>
        /// BeginGetObject EndGetObject TAP
        /// </summary>
        /// <typeparam name="OssObject"></typeparam>
        /// <param name="client"></param>
        /// <param name="getObjectRequest"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task<Aliyun.OSS.OssObject> GetObjectAsync<OssObject>(this OssClient client, GetObjectRequest getObjectRequest, object state = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            return Task<Aliyun.OSS.OssObject>.Factory.FromAsync(client.BeginGetObject, client.EndGetObject, getObjectRequest, state);
        }
        /// <summary>
        /// BeginGetObject EndGetObject TAP
        /// </summary>
        /// <typeparam name="OssObject"></typeparam>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<Aliyun.OSS.OssObject> GetObjectAsync<OssObject>(this OssClient client, string bucketName, string key, object state = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            return Task<Aliyun.OSS.OssObject>.Factory.FromAsync(client.BeginGetObject, client.EndGetObject, bucketName, key, state);
        }
    }
}
