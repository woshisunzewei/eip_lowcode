using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EIP.Common.Core.Util
{
	/// <summary>
	/// 公私钥请使用openssl生成  ssh-keygen -t rsa 命令生成的公钥私钥是不行的
	/// http://web.chacuo.net/netrsakeypair/
	/// 1024 PKCS#1
	/// </summary>
	public class RsaUtil
	{
		private readonly RSA _privateKeyRsaProvider;
		private readonly RSA _publicKeyRsaProvider;
		private readonly HashAlgorithmName _hashAlgorithmName;
		private readonly Encoding _encoding;

		/// <summary>
		/// 实例化RSAHelper
		/// </summary>
		/// <param name="rsaType">加密算法类型 RSA SHA1;RSA2 SHA256 密钥长度至少为2048</param>
		/// <param name="encoding">编码类型</param>
		/// <param name="privateKey">私钥</param>
		/// <param name="publicKey">公钥</param>
		public RsaUtil(RSAType rsaType, Encoding encoding, string privateKey, string publicKey = null)
		{
			_encoding = encoding;
			if (!string.IsNullOrEmpty(privateKey))
			{
				_privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
			}

			if (!string.IsNullOrEmpty(publicKey))
			{
				_publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
			}

			_hashAlgorithmName = rsaType == RSAType.RSA ? HashAlgorithmName.SHA1 : HashAlgorithmName.SHA256;
		}

		#region 使用私钥签名

		/// <summary>
		/// 使用私钥签名
		/// </summary>
		/// <param name="data">原始数据</param>
		/// <returns></returns>
		public string Sign(string data)
		{
			byte[] dataBytes = _encoding.GetBytes(data);

			var signatureBytes = _privateKeyRsaProvider.SignData(dataBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);

			return Convert.ToBase64String(signatureBytes);
		}

		#endregion

		#region 使用公钥验证签名

		/// <summary>
		/// 使用公钥验证签名
		/// </summary>
		/// <param name="data">原始数据</param>
		/// <param name="sign">签名</param>
		/// <returns></returns>
		public bool Verify(string data,string sign)
		{
			byte[] dataBytes = _encoding.GetBytes(data);
			byte[] signBytes = Convert.FromBase64String(sign);
			var verify = _publicKeyRsaProvider.VerifyData(dataBytes, signBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);
			return verify;
		}

		#endregion

		#region 解密

		public string Decrypt(string cipherText)
		{
			if (_privateKeyRsaProvider == null)
			{
				throw new Exception("_privateKeyRsaProvider is null");
			}
			return Encoding.UTF8.GetString(_privateKeyRsaProvider.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1));
		}

		#endregion

		#region 加密

		public string Encrypt(string text)
		{
			if (_publicKeyRsaProvider == null)
			{
				throw new Exception("_publicKeyRsaProvider is null");
			}
			return Convert.ToBase64String(_publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
		}

		#endregion

		#region 使用私钥创建RSA实例

		public RSA CreateRsaProviderFromPrivateKey(string privateKey)
		{
			var privateKeyBits = Convert.FromBase64String(privateKey);

			var rsa = RSA.Create();
			var rsaParameters = new RSAParameters();

			using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
			{
				byte bt = 0;
				ushort twobytes = 0;
				twobytes = binr.ReadUInt16();
				if (twobytes == 0x8130)
					binr.ReadByte();
				else if (twobytes == 0x8230)
					binr.ReadInt16();
				else
					throw new Exception("Unexpected value read binr.ReadUInt16()");

				twobytes = binr.ReadUInt16();
				if (twobytes != 0x0102)
					throw new Exception("Unexpected version");

				bt = binr.ReadByte();
				if (bt != 0x00)
					throw new Exception("Unexpected value read binr.ReadByte()");

				rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
			}

			rsa.ImportParameters(rsaParameters);
			return rsa;
		}

		#endregion

		#region 使用公钥创建RSA实例

		public RSA CreateRsaProviderFromPublicKey(string publicKeyString)
		{
			// encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
			byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
			byte[] seq = new byte[15];

			var x509Key = Convert.FromBase64String(publicKeyString);

			// ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
			using (MemoryStream mem = new MemoryStream(x509Key))
			{
				using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
				{
					byte bt = 0;
					ushort twobytes = 0;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					seq = binr.ReadBytes(15);       //read the Sequence OID
					if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8203)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					bt = binr.ReadByte();
					if (bt != 0x00)     //expect null byte next
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					twobytes = binr.ReadUInt16();
					byte lowbyte = 0x00;
					byte highbyte = 0x00;

					if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
						lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
					else if (twobytes == 0x8202)
					{
						highbyte = binr.ReadByte(); //advance 2 bytes
						lowbyte = binr.ReadByte();
					}
					else
						return null;
					byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
					int modsize = BitConverter.ToInt32(modint, 0);

					int firstbyte = binr.PeekChar();
					if (firstbyte == 0x00)
					{   //if first byte (highest order) of modulus is zero, don't include it
						binr.ReadByte();    //skip this null byte
						modsize -= 1;   //reduce modulus buffer size by 1
					}

					byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

					if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
						return null;
					int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
					byte[] exponent = binr.ReadBytes(expbytes);

					// ------- create RSACryptoServiceProvider instance and initialize with public key -----
					var rsa = RSA.Create();
					RSAParameters rsaKeyInfo = new RSAParameters
					{
						Modulus = modulus,
						Exponent = exponent
					};
					rsa.ImportParameters(rsaKeyInfo);

					return rsa;
				}

			}
		}

		#endregion

		#region 导入密钥算法

		private int GetIntegerSize(BinaryReader binr)
		{
			byte bt = 0;
			int count = 0;
			bt = binr.ReadByte();
			if (bt != 0x02)
				return 0;
			bt = binr.ReadByte();

			if (bt == 0x81)
				count = binr.ReadByte();
			else
			if (bt == 0x82)
			{
				var highbyte = binr.ReadByte();
				var lowbyte = binr.ReadByte();
				byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
				count = BitConverter.ToInt32(modint, 0);
			}
			else
			{
				count = bt;
			}

			while (binr.ReadByte() == 0x00)
			{
				count -= 1;
			}
			binr.BaseStream.Seek(-1, SeekOrigin.Current);
			return count;
		}

		private bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			int i = 0;
			foreach (byte c in a)
			{
				if (c != b[i])
					return false;
				i++;
			}
			return true;
		}

		#endregion

	}

	/// <summary>
	/// RSA算法类型
	/// </summary>
	public enum RSAType
	{
		/// <summary>
		/// SHA1
		/// </summary>
		RSA = 0,
		/// <summary>
		/// RSA2 密钥长度至少为2048
		/// SHA256
		/// </summary>
		RSA2
	}


	public class RSACryptoService
	{
		private const string privateKey = @"MIICWwIBAAKBgQDh+uwqFW6N0j2lMcXc9xFwL7M1nFWxwkpVP0ZtcwdWMAoIsXwl
BGD6xMWIGiH+MI1w5rvGTE+DpA4j4ffcwhFk0SQk/iVTdksUTPYYkolfI5+XRM37
QkcJLb9OiOz6XlQYlQECOtT7UTtUp1V1AcOggj+CfdrzkcLtrFBXBIBcSwIDAQAB
AoGAIybnprI3xMrf+Prxluo95bAd1eiQMfsRmgoiN/NNQFxvqyOEtrNkLI4AVigO
gp5l2hqiOfnz4nvaET3c0xPDJZtHd7LQgqtOJ4Wq2IMNlFy6C+0DLWTIHCdW4/ZK
K5ZQOBnETPy0ZKdly+dAsshnla9jmHQnzKR0/LdIO6/6xQ0CQQDt7fdkeEtl1qvC
kVK8pPJV+1Uxxh0ZGzSenFYpS21Y80pQGGcKnhJinSeVVNrDw1s5BgtBOXWsQ6vo
SzN0JUsPAkEA8ySgLWbu1O6heRhXWNwDx2qPd3ytO2CEGldu67mpO/m//lwwPOyI
bv1Q91GZWR/4xY6yy5J3szU5PwFcQq/LBQJAPTATjTz6dugsJ89jravlvoLyN+ix
FwHOGHQwHFKPfm6iz9JWvX5FUCMGSPsXf3y/+vw47L8wKesFTKn6Q4ZLtQJAN7Dk
62FPRQBfheAYaacDSpLSx5iMCTSjLXFnW6DL7YvX+QfemKXI3jsxZ2SOTkavcXis
5UnoFukS8qZ6HmArjQJAIK7BI5bQQBYK48tfm1I+iEbSmVSR57jfa9IpDI0Lr6Hu
XOiVDr1baPLVeQBd8urNLHhgGT+beIFpyJAPXi14gA==";

		private const string publicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDh+uwqFW6N0j2lMcXc9xFwL7M1
nFWxwkpVP0ZtcwdWMAoIsXwlBGD6xMWIGiH+MI1w5rvGTE+DpA4j4ffcwhFk0SQk
/iVTdksUTPYYkolfI5+XRM37QkcJLb9OiOz6XlQYlQECOtT7UTtUp1V1AcOggj+C
fdrzkcLtrFBXBIBcSwIDAQAB";
		private RSACryptoServiceProvider _privateKeyRsaProvider;
		private RSACryptoServiceProvider _publicKeyRsaProvider;

		public RSACryptoService()
		{
			if (!string.IsNullOrEmpty(privateKey))
			{
				_privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
			}

			if (!string.IsNullOrEmpty(publicKey))
			{
				_publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
			}
		}
		public RSACryptoService(string privateKey,string publicKey)
		{
			if (!string.IsNullOrEmpty(privateKey))
			{
				_privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
			}

			if (!string.IsNullOrEmpty(publicKey))
			{
				_publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
			}
		}
		/// <summary>
		/// 补全密文
		/// </summary>
		/// <param name="strCiphertext">密文</param>
		/// <param name="keySize">秘钥长度</param>
		/// <returns>补全后的密文</returns>
		private static string CorrectionCiphertext(string strCiphertext, int keySize = 1024)
		{
			int ciphertextLength = keySize / 8;
			byte[] data = Convert.FromBase64String(strCiphertext);
			var newData = new List<byte>(data);
			while (newData.Count < ciphertextLength)
			{
				newData.Insert(0, 0x00);
			}
			return Convert.ToBase64String(newData.ToArray());
		}
		public string Decrypt(string cipherText)
		{
			cipherText = CorrectionCiphertext(cipherText);
			if (_privateKeyRsaProvider == null)
			{
				throw new Exception("_privateKeyRsaProvider is null");
			}
			return Encoding.UTF8.GetString(_privateKeyRsaProvider.Decrypt(System.Convert.FromBase64String(cipherText), false));
		}

		public string Encrypt(string text)
		{
			if (_publicKeyRsaProvider == null)
			{
				throw new Exception("_publicKeyRsaProvider is null");
			}
			return Convert.ToBase64String(_publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), false));
		}

		private RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string privateKey)
		{
			var privateKeyBits = System.Convert.FromBase64String(privateKey);

			var RSA = new RSACryptoServiceProvider();
			var RSAparams = new RSAParameters();

			using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
			{
				byte bt = 0;
				ushort twobytes = 0;
				twobytes = binr.ReadUInt16();
				if (twobytes == 0x8130)
					binr.ReadByte();
				else if (twobytes == 0x8230)
					binr.ReadInt16();
				else
					throw new Exception("Unexpected value read binr.ReadUInt16()");

				twobytes = binr.ReadUInt16();
				if (twobytes != 0x0102)
					throw new Exception("Unexpected version");

				bt = binr.ReadByte();
				if (bt != 0x00)
					throw new Exception("Unexpected value read binr.ReadByte()");

				RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
				RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
			}

			RSA.ImportParameters(RSAparams);
			return RSA;
		}

		private int GetIntegerSize(BinaryReader binr)
		{
			byte bt = 0;
			byte lowbyte = 0x00;
			byte highbyte = 0x00;
			int count = 0;
			bt = binr.ReadByte();
			if (bt != 0x02)
				return 0;
			bt = binr.ReadByte();

			if (bt == 0x81)
				count = binr.ReadByte();
			else
				if (bt == 0x82)
			{
				highbyte = binr.ReadByte();
				lowbyte = binr.ReadByte();
				byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
				count = BitConverter.ToInt32(modint, 0);
			}
			else
			{
				count = bt;
			}

			while (binr.ReadByte() == 0x00)
			{
				count -= 1;
			}
			binr.BaseStream.Seek(-1, SeekOrigin.Current);
			return count;
		}

		private RSACryptoServiceProvider CreateRsaProviderFromPublicKey(string publicKeyString)
		{
			// encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
			byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
			byte[] x509key;
			byte[] seq = new byte[15];
			int x509size;

			x509key = Convert.FromBase64String(publicKeyString);
			x509size = x509key.Length;

			// ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
			using (MemoryStream mem = new MemoryStream(x509key))
			{
				using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
				{
					byte bt = 0;
					ushort twobytes = 0;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					seq = binr.ReadBytes(15);       //read the Sequence OID
					if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8203)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					bt = binr.ReadByte();
					if (bt != 0x00)     //expect null byte next
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					twobytes = binr.ReadUInt16();
					byte lowbyte = 0x00;
					byte highbyte = 0x00;

					if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
						lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
					else if (twobytes == 0x8202)
					{
						highbyte = binr.ReadByte(); //advance 2 bytes
						lowbyte = binr.ReadByte();
					}
					else
						return null;
					byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
					int modsize = BitConverter.ToInt32(modint, 0);

					int firstbyte = binr.PeekChar();
					if (firstbyte == 0x00)
					{   //if first byte (highest order) of modulus is zero, don't include it
						binr.ReadByte();    //skip this null byte
						modsize -= 1;   //reduce modulus buffer size by 1
					}

					byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

					if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
						return null;
					int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
					byte[] exponent = binr.ReadBytes(expbytes);

					// ------- create RSACryptoServiceProvider instance and initialize with public key -----
					RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
					RSAParameters RSAKeyInfo = new RSAParameters();
					RSAKeyInfo.Modulus = modulus;
					RSAKeyInfo.Exponent = exponent;
					RSA.ImportParameters(RSAKeyInfo);

					return RSA;
				}

			}
		}

		private bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			int i = 0;
			foreach (byte c in a)
			{
				if (c != b[i])
					return false;
				i++;
			}
			return true;
		}
	}

}