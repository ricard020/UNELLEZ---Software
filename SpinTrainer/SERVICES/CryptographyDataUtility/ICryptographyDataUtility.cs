namespace UTILITIES.CryptographyDataUtility
{
    public interface ICryptographyDataUtility
    {
        /// <summary>
        /// Metodo de encriptación de datos usando un formato AES-256
        /// </summary>
        /// <param name="plainText">Texto plano que se quiere cifrar.</param>
        /// <returns>Texto cifrado</returns>
        public string Encrypt(string plainText);

        /// <summary>
        /// Metodo de desencriptación de datos usando un formato AES-256
        /// </summary>
        /// <param name="cipherText">Texto cifrado que se quiere descifrar.</param>
        /// <returns>Texto descifrado</returns>
        public string Decrypt(string cipherText);
    }
}
