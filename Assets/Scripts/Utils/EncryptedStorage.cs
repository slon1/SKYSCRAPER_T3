using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Utils {
	public class EncryptedStorage : EncryptedFile {
		private static readonly Lazy<EncryptedStorage> instance = new Lazy<EncryptedStorage>(() => new EncryptedStorage());
		public static EncryptedStorage Instance => instance.Value;

		private Dictionary<string, string> storage;

		private EncryptedStorage() {
			storage = new Dictionary<string, string>();
		}

		protected override async Task Load() {
			if (File.Exists(filepath)) {
				string encryptedText = await ReadAllTextAsync(filepath);
				string[] lines = encryptedText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string line in lines) {
					string[] parts = line.Split(new[] { ',' }, 2);
					if (parts.Length == 2) {
						storage[parts[0]] = parts[1];
					}
				}
			}
		}

		public override Task Initialize(string filename, string encryptionKey) {
			return base.Initialize(filename, encryptionKey);
		}

		protected override async Task Save() {
			string plaintext;
			lock (saveLock) {
				List<string> lines = new List<string>();
				foreach (var kvp in storage) {
					lines.Add($"{kvp.Key},{kvp.Value}");
				}
				plaintext = string.Join(Environment.NewLine, lines);


			}
			await WriteAllTextAsync(filepath, plaintext);
		}

		public async Task SetString(string name, string value) {
			storage[name] = value;
			await Save();
		}

		public string GetString(string name) {
			return storage.ContainsKey(name) ? storage[name] : null;
		}

		public async Task SetInt(string name, int value) {
			await SetString(name, value.ToString());
		}

		public int GetInt(string name, int defaultValue = 0) {
			string value = GetString(name);
			return int.TryParse(value, out int result) ? result : defaultValue;
		}

		public override void Dispose() {
			storage.Clear();
		}

		internal void Clear() {
			File.Delete(filepath);
		}
	}
}