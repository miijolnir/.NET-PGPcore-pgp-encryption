using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using PgpCore;
using System.Net.WebSockets;

namespace MyPgpFunctions
{
    public static class FunctionPgp
    {
        [FunctionName("FunctionPgp")]
        public static void Run(
            [BlobTrigger("input/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
            [Blob("output/{name}.pgp", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outBlob,
            string name,
            ILogger log)
        {

            //bool asciiArmor = true;

            // Retrieve the public key from Azure Key Vault
            string keyVaultUri = "key-vault-uri"; // Replace with your key-vault-uri
            string publicKeySecretName = "secret-name"; // Replace with your secret name
            var keyVaultClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            KeyVaultSecret publicKeySecret = keyVaultClient.GetSecret(publicKeySecretName);
            string inlinePublicKey = publicKeySecret.Value;

            inlinePublicKey = inlinePublicKey.Replace("\\r\\n", Environment.NewLine);

            PGP pgp = new PGP(new EncryptionKeys(inlinePublicKey));
            pgp.EncryptStream(myBlob, outBlob);
        }
    }
}


