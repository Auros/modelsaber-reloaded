using System;
using GraphQL;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace ModelSaber.API
{
    public static class Utilities
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>
        {
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            {
                ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            },
            {
                ".gif", new List<byte[]>
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38 }
                }
            },
            {
                ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x01, 0x00 }
                }
            }
        };

        public static async Task SaveIFormToFile(IFormFile file, string path)
        {
            using Stream stream = File.Create(path);
            await file.CopyToAsync(stream);
        }

        public static bool IsFileExtensionValid(Stream stream, string extension)
        {
            if (!_fileSignatures.TryGetValue(extension, out List<byte[]> signatures))
                return false;

            using BinaryReader reader = new BinaryReader(stream);
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
            return signatures.Any(sig => headerBytes.Take(sig.Length).SequenceEqual(sig));
        }

        public static bool IsZIP(Stream stream)
        {
            using BinaryReader reader = new BinaryReader(stream);
            var headerBytes = reader.ReadBytes(_fileSignatures[".zip"].Max(m => m.Length));
            return _fileSignatures[".zip"].Any(sig => headerBytes.Take(sig.Length).SequenceEqual(sig));
        }

        public static string ComputeHash(this Stream stream, HashType type = HashType.SHA256)
        {
            if (type == HashType.SHA256)
            {
                using SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(stream);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString().ToLower();
            }
            if (type == HashType.MD5)
            {
                using MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(stream);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString().ToLower();
            }
            return null;
        }

        public static IServiceProvider Resolve(this IResolveFieldContext resolveFieldContext)
        {
            if (resolveFieldContext is null)
                throw new ArgumentNullException(nameof(resolveFieldContext));
            return resolveFieldContext.RequestServices;
        }

        /// <summary>
        /// Get the current <see cref="Microsoft.AspNetCore.Http.HttpContext"/> from a field context.
        /// </summary>
        /// <param name="resolveFieldContext"></param>
        /// <returns></returns>
        public static HttpContext HttpContext(this IResolveFieldContext resolveFieldContext)
        {
            HttpContext? context = resolveFieldContext.Resolve<IHttpContextAccessor>().HttpContext;
            if (context is null)
                throw new NullReferenceException(nameof(context) + "is null");
            return context;
        }

        /// <summary>
        /// Resolve an object from a service container in an <see cref="IResolveFieldContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object being resolved.</typeparam>
        /// <param name="resolveFieldContext">The field context.</param>
        /// <returns></returns>
        public static T Resolve<T>(this IResolveFieldContext resolveFieldContext) => (T)resolveFieldContext.Resolve().GetRequiredService(typeof(T));
    }

    public enum HashType
    {
        MD5,
        SHA256
    }
}
