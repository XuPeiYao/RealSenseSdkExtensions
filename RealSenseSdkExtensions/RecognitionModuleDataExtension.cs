using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PXCMFaceData;

namespace RealSenseSdkExtensions {
    public static class RecognitionModuleDataExtension {
        /// <summary>
        /// 取得臉部辨識資料庫緩衝區序列
        /// </summary>
        /// <param name="moduleData">臉部辨識資料庫</param>
        /// <returns>緩衝區原始資料</returns>
        public static byte[] GetDatabaseBuffer(
            this RecognitionModuleData moduleData
            ) {
            byte[] result = new byte[moduleData.QueryDatabaseSize()];
            moduleData.QueryDatabaseBuffer(result);
            return result;
        }

        /// <summary>
        /// 取得臉部辨識資料庫資訊
        /// </summary>
        /// <param name="moduleData">臉部辨識資料庫</param>
        /// <returns>臉部辨識資料結構陣列</returns>
        public static RecognitionFaceData[] GetDatabase(
            this RecognitionModuleData moduleData
            ) {
            return RecognitionFaceData.FromDatabaseBuffer(
                moduleData.GetDatabaseBuffer());
        }
    }
}
