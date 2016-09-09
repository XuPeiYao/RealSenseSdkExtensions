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
    [Serializable]
    public class RecognitionFaceData {
        public static int Length = 16392;
        /// <summary>
        /// 資料庫主鍵
        /// </summary>
        public int PrimaryKey {
            get {
                return BitConverter.ToInt32(BinaryData, BinaryData.Length - 8);
            }
            set {
                Array.Copy(BitConverter.GetBytes(value), 0, BinaryData, BinaryData.Length - 8, 4);
            }
        }

        /// <summary>
        /// 資料庫外來鍵(使用者ID)
        /// </summary>
        public int ForeignKey {
            get {
                return BitConverter.ToInt32(BinaryData, BinaryData.Length - 4);
            }
            set {
                Array.Copy(BitConverter.GetBytes(value), 0, BinaryData, BinaryData.Length - 4, 4);
            }
        }

        private Bitmap _image;
        /// <summary>
        /// 人臉辨識圖檔
        /// </summary>
        public Bitmap Image {
            get {
                if (_image != null) return _image;
                _image = new Bitmap(128, 128);
                for (int i = 0; i < BinaryData.Length - 8; i++) {
                    var color = Color.FromArgb(BinaryData[i], BinaryData[i], BinaryData[i]);
                    var point = new Point(
                        i % 128,
                        (int)Math.Floor(i / 128.0)
                    );
                    _image.SetPixel(point.X, point.Y, color);
                }
                return _image;
            }
            set {
                if (value.Size.Height != 128 || value.Size.Width != 128) {
                    throw new ArgumentOutOfRangeException("圖片大小錯誤");
                }
                byte[] data = new byte[128 * 128];
                for (int i = 0; i < data.Length; i++) {
                    var point = new Point(
                        i % 128,
                        (int)Math.Floor(i / 128.0)
                    );
                    var color = value.GetPixel(point.X, point.Y);
                    data[i] = (byte)((color.R * 0.3) +
                              (color.G * 0.59) +
                              (color.B * 0.11));
                }
                Array.Copy(data, 0, BinaryData, 0, data.Length);
            }
        }

        /// <summary>
        /// 原始二進制資訊
        /// </summary>
        public byte[] BinaryData { get; private set; }

        public RecognitionFaceData(byte[] binaryData = null) {
            if (binaryData == null) {
                BinaryData = new byte[128 * 128 + 8];
            } else {
                BinaryData = binaryData;
            }
            _image = null;
        }

        public void UpdateDatabaseBuffer(byte[] binary) {
            FaceDatabaseManager.UpdateBuffer(binary, this);
        }

        /// <summary>
        /// 自資料庫緩衝區資訊轉換為臉部辨識資料結構陣列
        /// </summary>
        /// <param name="binaryData">資料庫緩衝區資訊</param>
        /// <returns>臉部辨識資料結構陣列</returns>
        public static RecognitionFaceData[] FromDatabaseBuffer(byte[] binaryData) {
            List<RecognitionFaceData> result = new List<RecognitionFaceData>();

            var resultLength = RecognitionFaceData.Length;
            for (int i = 0; i < binaryData.Length; i += resultLength) {
                byte[] itemRawData = binaryData.Skip(i).Take(resultLength).ToArray();
                var newItem = new RecognitionFaceData(itemRawData);
                if (newItem.PrimaryKey == -1) continue;
                result.Add(newItem);
            }

            return result.ToArray();
        }
    }
}