using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PXCMFaceConfiguration;

namespace RealSenseSdkExtensions {
    public static class RecognitionConfigurationExtension {
        /// <summary>
        /// 設定臉部辨識資料庫緩衝區序列
        /// </summary>
        /// <param name="config">臉部辨識資料庫</param>
        /// <param name="dataArray">緩衝區序列</param>
        public static void SetDatabase(
            this RecognitionConfiguration config,
            RecognitionFaceData[] dataArray) {
            List<byte> buffer = new List<byte>();
            foreach(var item in dataArray) {
                buffer.AddRange(item.BinaryData);
            }
            config.SetDatabaseBuffer(buffer.ToArray());
        }
    }
}
