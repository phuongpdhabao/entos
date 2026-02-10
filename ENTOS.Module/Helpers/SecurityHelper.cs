using System;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper bảo mật: hash, mã hóa, sinh OTP, ký số, xác thực JWT, kiểm tra file, QR code... Chỉ dùng .NET chuẩn.
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Băm MD5 chuỗi (hash MD5).
        /// </summary>
        public static string HashMD5(string input)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Băm SHA1 chuỗi (hash SHA1).
        /// </summary>
        public static string HashSHA1(string input)
        {
            using var sha = SHA1.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Băm SHA256 chuỗi (hash SHA256).
        /// </summary>
        public static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Băm SHA512 chuỗi (hash SHA512).
        /// </summary>
        public static string HashSHA512(string input)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Mã hóa base64.
        /// </summary>
        public static string ToBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }
        /// <summary>
        /// Giải mã base64.
        /// </summary>
        public static string FromBase64(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
        /// <summary>
        /// Sinh mật khẩu ngẫu nhiên an toàn.
        /// </summary>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=";
            var rnd = new RNGCryptoServiceProvider();
            var data = new byte[length];
            rnd.GetBytes(data);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(chars[data[i] % chars.Length]);
            return sb.ToString();
        }
        /// <summary>
        /// Kiểm tra độ mạnh mật khẩu (có hoa, thường, số, ký tự đặc biệt).
        /// </summary>
        public static bool CheckPasswordStrength(string password, int minLength = 8)
        {
            if (string.IsNullOrEmpty(password) || password.Length < minLength) return false;
            bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
            foreach (var c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else hasSpecial = true;
            }
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
        /// <summary>
        /// Sinh mã OTP ngẫu nhiên.
        /// </summary>
        public static string GenerateOTP(int digits = 6)
        {
            var rnd = new RNGCryptoServiceProvider();
            var data = new byte[4];
            rnd.GetBytes(data);
            int value = BitConverter.ToInt32(data, 0) & int.MaxValue;
            return (value % (int)Math.Pow(10, digits)).ToString($"D{digits}");
        }
        /// <summary>
        /// Kiểm tra mã OTP.
        /// </summary>
        public static bool ValidateOTP(string otp, string expected)
        {
            return otp == expected;
        }
        /// <summary>
        /// Mã hóa AES (chuỗi, key).
        /// </summary>
        public static string EncryptAes(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = new byte[16];
            using var encryptor = aes.CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(encrypted);
        }
        /// <summary>
        /// Giải mã AES (chuỗi, key).
        /// </summary>
        public static string DecryptAes(string cipherText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = new byte[16];
            using var decryptor = aes.CreateDecryptor();
            var bytes = Convert.FromBase64String(cipherText);
            var decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
        /// <summary>
        /// Tạo chữ ký HMAC SHA256.
        /// </summary>
        public static string HmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Tạo chữ ký HMAC SHA512.
        /// </summary>
        public static string HmacSha512(string data, string key)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Sinh salt ngẫu nhiên (dùng cho hash mật khẩu).
        /// </summary>
        public static string GenerateSalt(int size = 16)
        {
            var bytes = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Hash mật khẩu mạnh PBKDF2 (có salt, lặp).
        /// </summary>
        public static string HashPasswordPBKDF2(string password, out string salt, int iterations = 10000)
        {
            salt = GenerateSalt();
            using var derive = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(derive.GetBytes(32));
        }
        /// <summary>
        /// Xác thực mật khẩu PBKDF2.
        /// </summary>
        public static bool VerifyPasswordPBKDF2(string password, string hash, string salt, int iterations = 10000)
        {
            using var derive = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations, HashAlgorithmName.SHA256);
            var test = Convert.ToBase64String(derive.GetBytes(32));
            return hash == test;
        }
        /// <summary>
        /// Sinh cặp khóa RSA (public/private key).
        /// </summary>
        public static (string publicKey, string privateKey) GenerateRsaKeyPair(int keySize = 2048)
        {
            using var rsa = RSA.Create(keySize);
            return (
                Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                Convert.ToBase64String(rsa.ExportRSAPrivateKey())
            );
        }
        /// <summary>
        /// Mã hóa RSA (public key).
        /// </summary>
        public static string RsaEncrypt(string plainText, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encrypted);
        }
        /// <summary>
        /// Giải mã RSA (private key).
        /// </summary>
        public static string RsaDecrypt(string cipherText, string privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            var bytes = Convert.FromBase64String(cipherText);
            var decrypted = rsa.Decrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decrypted);
        }
        /// <summary>
        /// Ký số RSA (private key).
        /// </summary>
        public static string RsaSign(string data, string privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            var bytes = Encoding.UTF8.GetBytes(data);
            var sig = rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(sig);
        }
        /// <summary>
        /// Xác thực chữ ký số RSA (public key).
        /// </summary>
        public static bool RsaVerify(string data, string signature, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            var bytes = Encoding.UTF8.GetBytes(data);
            var sig = Convert.FromBase64String(signature);
            return rsa.VerifyData(bytes, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        /// <summary>
        /// Tạo JWT (header.payload.signature, HMACSHA256).
        /// </summary>
        public static string JwtEncode(Dictionary<string, object> payload, string secret)
        {
            string Base64UrlEncode(byte[] arg)
            {
                return Convert.ToBase64String(arg).TrimEnd('=')
                    .Replace('+', '-').Replace('/', '_');
            }
            var header = Base64UrlEncode(Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
            var body = Base64UrlEncode(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(payload)));
            var toSign = Encoding.UTF8.GetBytes($"{header}.{body}");
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var sig = Base64UrlEncode(hmac.ComputeHash(toSign));
            return $"{header}.{body}.{sig}";
        }
        /// <summary>
        /// Xác thực JWT (HMACSHA256).
        /// </summary>
        public static bool JwtValidate(string token, string secret)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return false;
            string toSign = $"{parts[0]}.{parts[1]}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var sig = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign))).TrimEnd('=')
                .Replace('+', '-').Replace('/', '_');
            return sig == parts[2];
        }
        /// <summary>
        /// Ẩn dữ liệu nhạy cảm (mask).
        /// </summary>
        public static string MaskSensitive(string input, int showStart = 2, int showEnd = 2)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= showStart + showEnd) return input;
            return input.Substring(0, showStart) + new string('*', input.Length - showStart - showEnd) + input.Substring(input.Length - showEnd);
        }
        /// <summary>
        /// Sinh chuỗi ngẫu nhiên an toàn.
        /// </summary>
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(chars[data[i] % chars.Length]);
            return sb.ToString();
        }
        /// <summary>
        /// Sinh mảng byte ngẫu nhiên an toàn.
        /// </summary>
        public static byte[] RandomBytes(int length)
        {
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            return data;
        }
        /// <summary>
        /// Kiểm tra chuỗi có phải hash hợp lệ không.
        /// </summary>
        public static bool IsValidHash(string input, int length = 32)
        {
            return !string.IsNullOrEmpty(input) && input.Length == length && input.All(c => "0123456789abcdefABCDEF".Contains(c));
        }
        /// <summary>
        /// Sinh mã xác thực 2 lớp (TOTP, RFC 6238).
        /// </summary>
        public static string GenerateTotp(string secret, int digits = 6, int step = 30)
        {
            long timestep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / step;
            var key = Encoding.UTF8.GetBytes(secret);
            var msg = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(timestep));
            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(msg);
            int offset = hash[hash.Length - 1] & 0x0F;
            int binary = ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);
            int otp = binary % (int)Math.Pow(10, digits);
            return otp.ToString($"D{digits}");
        }
        /// <summary>
        /// Kiểm tra mã xác thực 2 lớp (TOTP).
        /// </summary>
        public static bool ValidateTotp(string secret, string code, int window = 1, int digits = 6, int step = 30)
        {
            long timestep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / step;
            for (long i = -window; i <= window; i++)
            {
                var test = GenerateTotp(secret, digits, step, timestep + i);
                if (test == code) return true;
            }
            return false;
        }
        private static string GenerateTotp(string secret, int digits, int step, long timestep)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var msg = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(timestep));
            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(msg);
            int offset = hash[hash.Length - 1] & 0x0F;
            int binary = ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);
            int otp = binary % (int)Math.Pow(10, digits);
            return otp.ToString($"D{digits}");
        }
        /// <summary>
        /// Ký số XML (XMLDSig, RSA SHA256).
        /// </summary>
        public static bool IsSafeFile(string filePath, string[] allowedExtensions, Dictionary<string, byte[]> allowedMagicNumbers = null)
        {
            var ext = System.IO.Path.GetExtension(filePath)?.ToLower();
            if (!allowedExtensions.Contains(ext)) return false;
            if (allowedMagicNumbers != null)
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                foreach (var kv in allowedMagicNumbers)
                {
                    var magic = kv.Value;
                    if (fileBytes.Length >= magic.Length && fileBytes.Take(magic.Length).SequenceEqual(magic))
                        return true;
                }
                return false;
            }
            return true;
        }
    }
} 