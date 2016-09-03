using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSenseSdkExtensions {
    /// <summary>
    /// 臉部辨識資料庫原始資料結構
    /// </summary>
    public struct RecognitionFaceData {
        /// <summary>
        /// 唯一識別號
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 人臉辨識圖檔
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// 未知欄位
        /// </summary>
        public byte[] UnknowField { get; private set; }

        /// <summary>
        /// 原始二進制資訊
        /// </summary>
        public byte[] BinaryData { get; private set; }

        public RecognitionFaceData(byte[] binaryData) {
            Id = BitConverter.ToInt32(binaryData, binaryData.Length - 5);
            UnknowField = binaryData.Skip(128 * 128).Take(4).ToArray();
            Image = new Bitmap(128, 128);
            for (int i = 0; i < binaryData.Length - 8; i++) {
                var color = Color.FromArgb(binaryData[i], binaryData[i], binaryData[i]);
                var point = new Point(
                    i % 128,
                    (int)Math.Floor(i / 128.0)
                );
                Image.SetPixel(point.X, point.Y, color);
            }
            
            BinaryData = binaryData;
        }

        /// <summary>
        /// 自資料庫緩衝區資訊轉換為臉部辨識資料結構陣列
        /// </summary>
        /// <param name="binaryData">資料庫緩衝區資訊</param>
        /// <returns>臉部辨識資料結構陣列</returns>
        public static RecognitionFaceData[] FromDatabaseBuffer(byte[] binaryData) {
            List<RecognitionFaceData> result = new List<RecognitionFaceData>();

            var resultLength = 16392;
            for (int i = 0; i < binaryData.Length; i += resultLength) {
                byte[] itemRawData = binaryData.Skip(i).Take(resultLength).ToArray();
                result.Add(new RecognitionFaceData(itemRawData));
            }

            return result.ToArray();
        }
    }
}
