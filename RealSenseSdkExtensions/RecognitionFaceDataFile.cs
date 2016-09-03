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
        /// <param name="dataArray">臉部辨識資料結構</param>
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
            return RecognitionFaceData.FromDatabaseBuffer(buffer.ToArray());
        }
    }
}