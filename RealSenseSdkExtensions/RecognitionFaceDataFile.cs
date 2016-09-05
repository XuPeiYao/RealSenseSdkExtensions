using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSenseSdkExtensions {
    public static class RecognitionFaceDataFile {
        /// <summary>
        /// 將臉部辨識資料結構陣列以二進制儲存至指定位置
        /// </summary>
        /// <param name="dataArray">臉部辨識資料結構陣列</param>
        /// <param name="path">檔案路徑</param>
        public static void Save(
            this RecognitionFaceData[] dataArray,
            string path) {
            var fileStream = new FileStream(path, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fileStream);
            foreach (var item in dataArray) {
                writer.Write(item.BinaryData);
                writer.Flush();
            }
            writer.Close();
            fileStream.Close();
        }

        /// <summary>
        /// 將臉部辨識資料結構陣列轉換為二進制陣列
        /// </summary>
        /// <param name="dataArray">臉部辨識資料結構陣列</param>
        /// <returns>二進制陣列</returns>
        public static byte[] ToBinary(
            this RecognitionFaceData[] dataArray) {
            MemoryStream writer = new MemoryStream();
            foreach (var item in dataArray) {
                writer.Write(item.BinaryData,0,item.BinaryData.Length);
                writer.Flush();
            }
            return writer.ToArray();
        }

        /// <summary>
        /// 自指定檔案讀取臉部辨識資料結構陣列
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <returns>臉部辨識資料結構陣列</returns>
        public static RecognitionFaceData[] Load(
            string path) {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            BinaryReader reader = new BinaryReader(fileStream);
            List<byte> buffer = new List<byte>();
            while(reader.BaseStream.Position != reader.BaseStream.Length) {
                buffer.Add(reader.ReadByte());
            }
            return FromBinary(buffer.ToArray());
        }

        /// <summary>
        /// 自二進制陣列讀取臉部辨識資料結構陣列
        /// </summary>
        /// <param name="binary">二進制陣列</param>
        /// <returns>臉部辨識資料結構陣列</returns>
        public static RecognitionFaceData[] FromBinary(
           byte[] binary) {
            return RecognitionFaceData.FromDatabaseBuffer(binary);
        }
    }
}