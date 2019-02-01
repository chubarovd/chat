using System;
using System.Net;
using System.Text;

namespace ProtocolChat {
    public class Protocol {
        public static Encoding commonEncoding = Encoding.GetEncoding (1251);

        public enum PackageType {
            LOGIN,
            CONNECT,
            MESSAGE
        }

        /// <summary>
        /// Преобразует байт в PackageType.
        /// </summary>
        public static PackageType GetPackageType (byte typeInByte) {
            return (PackageType)typeInByte;
        }

        public static byte[] GetPackageTypeByte (PackageType type) {
            return BitConverter.GetBytes ((int)type);
        }

        private static IPAddress _ip = IPAddress.Parse ("172.20.131.19");
        public static IPAddress ip {
            get { return _ip; }
        }

        public const int port = 2016;

        private static IPEndPoint _endPoint = new IPEndPoint (ip, port);
        public static IPEndPoint endPoint {
            get { return _endPoint; }
        }

        private Protocol () { }
    }

    public class Package {
        public Protocol.PackageType type {
            get;
        }
        private byte[] str1;
        private byte[] str2;

        /// <summary>
        /// Создает пустой пакет.
        /// </summary>
        private Package (int str1Size, int str2Size, Protocol.PackageType type) {
            str1 = new byte[str1Size];
            str2 = new byte[str2Size];
            this.type = type;
        }

        /// <summary>
        /// <para>Создает пакет с данными str1 и str2.</para>
        /// <para>Замечание: по умолчанию тип пакета - MESSAGE.</para>
        /// </summary>
        public Package (string str1, string str2 = null, Protocol.PackageType type = Protocol.PackageType.MESSAGE) {
            this.str1 = Protocol.commonEncoding.GetBytes (str1);
            if (str2 == null) this.str2 = Protocol.commonEncoding.GetBytes ("");
            else this.str2 = Protocol.commonEncoding.GetBytes (str2);
            this.type = type;
        }

        public static Package Build (byte[] bytes) {
            ushort str1Size = BitConverter.ToUInt16 (bytes, 1);
            ushort str2Size = BitConverter.ToUInt16 (bytes, 3);

            Package package = new Package (str1Size, str2Size, Protocol.GetPackageType (bytes[0]));

            /*for (int i = 5; i < str1Size; i++) {
                package.str1[i - 5] = bytes[i];
            }*/
            Buffer.BlockCopy (bytes, 5, package.str1, 0, str1Size);
            if (str2Size > 0)
                /*for (int i = 5; i < str2Size; i++) {
                    package.str1[i - 5 - str1Size] = bytes[i];
                }*/
                Buffer.BlockCopy (bytes, 5 + str1Size, package.str2, 0, str2Size);
            else package.str2 = null;

            return package;
        }

        /// <summary>
        /// Извлекает содержимое пакета в str1.
        /// </summary>
        public void Extract (out string str1) {
            str1 = Encoding.GetEncoding (1251).GetString (this.str1);
        }

        /// <summary>
        /// Извлекает содержимое пакета в str1 и str2 соответсвенно.
        /// </summary>
        public void Extract (out string str1, out string str2) {
            str1 = Protocol.commonEncoding.GetString (this.str1);
            if (this.str2 != null) str2 = Protocol.commonEncoding.GetString (this.str2);
            else str2 = null;
        }

        public byte[] ConvertToBytes () {
            byte[] str1Size = BitConverter.GetBytes (str1.Length);
            byte[] str2Size = BitConverter.GetBytes (str2.Length);
            byte[] packageType = Protocol.GetPackageTypeByte (this.type);

            int bytesCount = 5 + str1.Length + str2.Length;
            byte[] bytes = new byte[bytesCount];

            packageType.CopyTo (bytes, 0);
            str1Size.CopyTo (bytes, 1);
            str2Size.CopyTo (bytes, 3);
            str1.CopyTo (bytes, 5);
            str2.CopyTo (bytes, 5 + str1.Length);

            return bytes;
        }
    }
}
