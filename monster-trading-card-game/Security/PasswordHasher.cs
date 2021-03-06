using System;
using System.Security.Cryptography;

namespace monster_trading_card_game.Security {
	public class PasswordHasher {
		private const int SaltSize = 16;
		private const int HashSize = 20;

		/// <summary>
		/// Create Hash from Password
		/// </summary>
		/// <param name="password"> Password in clear text </param>
		/// <param name="iterations"> Iterations, default: 10000 </param>
		/// <returns> hashed password </returns>
		public string Hash(string password, int iterations) {
			// Create salt
			using (var rng = new RNGCryptoServiceProvider()) {
				byte[] salt;
				rng.GetBytes(salt = new byte[SaltSize]);
				using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations)) {
					var hash = pbkdf2.GetBytes(HashSize);
					// Combine salt and hash
					var hashBytes = new byte[SaltSize + HashSize];
					Array.Copy(salt, 0, hashBytes, 0, SaltSize);
					Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
					// Convert to base64
					var base64Hash = Convert.ToBase64String(hashBytes);

					// Format hash with extra information
					return $"$MTCGHASH${iterations}${base64Hash}";
				}
			}
		}

		public string Hash(string password) {
			return Hash(password, 10000);
		}

		public bool IsHashSupported(string hashString) {
			return hashString.Contains("MTCGHASH$");
		}

		/// <summary>
		/// Verify Password Hash
		/// </summary>
		/// <param name="password"> Clear text Password </param>
		/// <param name="hashedPassword"> Hashed Password from Database </param>
		/// <returns> true if password is verified </returns>
		/// <returns> false if password cannot be verified </returns>
		public bool Verify(string password, string hashedPassword) {
			// Check hash
			if (!IsHashSupported(hashedPassword)) {
				throw new NotSupportedException("The hashtype is not supported");
			}

			// Extract iteration and Base64 string
			var splittedHashString = hashedPassword.Replace("$MTCGHASH$", "").Split('$');
			var iterations = int.Parse(splittedHashString[0]);
			var base64Hash = splittedHashString[1];

			// Get hash bytes
			var hashBytes = Convert.FromBase64String(base64Hash);

			// Get salt
			var salt = new byte[SaltSize];
			Array.Copy(hashBytes, 0, salt, 0, SaltSize);

			// Create hash with given salt
			using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations)) {
				byte[] hash = pbkdf2.GetBytes(HashSize);

				// Get result
				for (var i = 0; i < HashSize; i++) {
					if (hashBytes[i + SaltSize] != hash[i]) {
						return false;
					}
				}
				return true;
			}
		}
	}
}