using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
namespace Utils {
	public abstract class EncryptedFile : IDisposable {
		protected string filepath;
		//protected string key;
		protected object saveLock = new object();
		protected byte[] key;

		public virtual async Task Initialize(string filename, string encryptionKey) {
			filepath = Path.Combine(Environment.CurrentDirectory, filename);
			key = MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(encryptionKey));
			await Load();
		}
		//public string XorString(string input) {
		//	byte[] inputBytes = Encoding.Unicode.GetBytes(input);
		//	byte[] keyBytes = Encoding.Unicode.GetBytes(key);
		//	byte[] resultBytes = new byte[inputBytes.Length];

		//	for (int i = 0; i < inputBytes.Length; i++) {
		//		resultBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
		//	}
		//	return Encoding.Unicode.GetString(resultBytes);
		//}
		byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key) {
			byte[] encrypted;
			byte[] IV;
			byte[] hmacKey = new byte[Key.Length];
			Array.Copy(Key, hmacKey, Key.Length);

			using (Aes aesAlg = Aes.Create()) {
				aesAlg.Key = Key;
				aesAlg.GenerateIV();
				IV = aesAlg.IV;
				aesAlg.Mode = CipherMode.CBC;
				var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (var msEncrypt = new MemoryStream()) {
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
						using (var swEncrypt = new StreamWriter(csEncrypt)) {
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			var combinedIvCt = new byte[IV.Length + encrypted.Length];
			Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
			Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

			using (HMACSHA256 hmac = new HMACSHA256(hmacKey)) {
				byte[] hash = hmac.ComputeHash(combinedIvCt);
				var result = new byte[combinedIvCt.Length + hash.Length];
				Array.Copy(combinedIvCt, 0, result, 0, combinedIvCt.Length);
				Array.Copy(hash, 0, result, combinedIvCt.Length, hash.Length);
				return result;
			}
		}

		string DecryptStringFromBytes_Aes(byte[] cipherTextCombined, byte[] Key) {
			string plaintext = null;
			byte[] hmacKey = new byte[Key.Length];
			Array.Copy(Key, hmacKey, Key.Length);

			using (HMACSHA256 hmac = new HMACSHA256(hmacKey)) {
				byte[] receivedHash = new byte[hmac.HashSize / 8];
				byte[] actualCipherText = new byte[cipherTextCombined.Length - receivedHash.Length];
				Array.Copy(cipherTextCombined, actualCipherText, actualCipherText.Length);
				Array.Copy(cipherTextCombined, actualCipherText.Length, receivedHash, 0, receivedHash.Length);

				byte[] computedHash = hmac.ComputeHash(actualCipherText);
				for (int i = 0; i < receivedHash.Length; i++) {
					if (computedHash[i] != receivedHash[i]) {
						throw new CryptographicException("HMAC validation failed");
					}
				}

				using (Aes aesAlg = Aes.Create()) {
					aesAlg.Key = Key;
					byte[] IV = new byte[aesAlg.BlockSize / 8];
					byte[] cipherText = new byte[actualCipherText.Length - IV.Length];

					Array.Copy(actualCipherText, IV, IV.Length);
					Array.Copy(actualCipherText, IV.Length, cipherText, 0, cipherText.Length);

					aesAlg.IV = IV;
					aesAlg.Mode = CipherMode.CBC;
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

					using (var msDecrypt = new MemoryStream(cipherText)) {
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
							using (var srDecrypt = new StreamReader(csDecrypt)) {
								plaintext = srDecrypt.ReadToEnd();
							}
						}
					}
				}
			}

			return plaintext;
		}


		protected async Task WriteAllTextAsync(string path, string text) {			
			var encodedText = EncryptStringToBytes_Aes(text, key);
			await File.WriteAllBytesAsync(path, encodedText);
		}

		protected async Task<string> ReadAllTextAsync(string path) {
			var decodedText = await File.ReadAllBytesAsync(path);
			return DecryptStringFromBytes_Aes (decodedText, key);
		}

		protected abstract Task Load();
		protected abstract Task Save();

		public virtual void Dispose() {
			saveLock = null;
		}

	}
}