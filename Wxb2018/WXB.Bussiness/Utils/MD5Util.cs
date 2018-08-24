using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WXB.Bussiness.Utils
{
	/// <summary>
	/// MD5Util ��ժҪ˵����
	/// </summary>
	public class MD5Util
	{
		public MD5Util()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/** ��ȡ��д��MD5ǩ����� */
        public static string GetMD5(string encypStr, string charset = "GB2312")
		{
			string retStr;
			MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

			//����md5����
			byte[] inputBye;
			byte[] outputBye;

			//ʹ��GB2312���뷽ʽ���ַ���ת��Ϊ�ֽ����飮
			try
			{
				inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
			}
			catch (Exception ex)
			{
				inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
			}
			outputBye = m5.ComputeHash(inputBye);

			retStr = System.BitConverter.ToString(outputBye);
			retStr = retStr.Replace("-", "").ToUpper();
			return retStr;
		}
        /// <summary>
        /// �����ļ�·���õ��ļ���MD5ֵ
        /// </summary>
        /// <param name="FilePath">�ļ���·��</param>
        /// <returns>MD5ֵ</returns>
        public static string GetFileMD5(string FilePath)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return "";
                }
                FileStream get_file = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                string resule = System.BitConverter.ToString(hash_byte);
                resule = resule.Replace("-", "");
                return resule;
            }
            catch (Exception e)
            {
                return "";
            }
        }

	}
}
