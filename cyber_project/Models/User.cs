namespace cyber_project.Models
{
    using System;
using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string LamportPublicKey { get; set; }
        public string LamportPrivateKey { get; set; }
        public List<string> HashChains { get; set; }

        // Additional user-related fields can be added as needed

        public static User RegisterUser(string username, string password)
        {
            // Generate Lamport key pairs
            var lamportKeyPair = GenerateLamportKeyPair();
            
            // Generate and store hash chains
            var hashChains = GenerateHashChains("seed", 10); // Adjust the chain length as needed

            var newUser = new User
            {
                Username = username,
                PasswordHash = ComputePasswordHash(password),
                LamportPublicKey = lamportKeyPair.PublicKey,
                LamportPrivateKey = lamportKeyPair.PrivateKey,
                HashChains = hashChains
            };

            // Save the new user to the database
            // ...

            return newUser;
        }

        public static bool AuthenticateUser(User user, string password, string lamportSignature, int chainIndex)
        {
            // Verify Lamport signature
            if (!VerifyLamportSignature(lamportSignature, user.LamportPublicKey, password))
            {
                return false;
            }

            // Check hash chain for authentication
            return CheckHashChain(user.HashChains, ComputePasswordHash(password), chainIndex);
        }

        private static (string PublicKey, string PrivateKey) GenerateLamportKeyPair()
        {
            var random = new Random();
            var publicKey = random.Next().ToString();
            var privateKey = random.Next().ToString();

            return (publicKey, privateKey);
        }

        private static string ComputePasswordHash(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private static bool VerifyLamportSignature(string signature, string publicKey, string message)
        {
            var hashedMessage = ComputePasswordHash(message);
            return signature == hashedMessage;
        }

        private static List<string> GenerateHashChains(string seed, int chainLength)
        {
            var hashChains = new List<string>();
            for (int i = 0; i < chainLength; i++)
            {
                seed = ComputePasswordHash(seed);
                hashChains.Add(seed);
            }
            return hashChains;
        }

        private static bool CheckHashChain(List<string> hashChains, string hashedPassword, int chainIndex)
        {
            return hashChains[chainIndex] == hashedPassword;
        }
    }

}