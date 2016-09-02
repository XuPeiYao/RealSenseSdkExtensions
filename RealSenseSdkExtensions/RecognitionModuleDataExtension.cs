using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PXCMFaceData;

namespace RealSenseSdkExtensions {
    public static class RecognitionModuleDataExtension {
        public static byte[] GetDatabaseBuffer(
            this RecognitionModuleData moduleData
            ) {
            byte[] result = new byte[moduleData.QueryDatabaseSize()];
            moduleData.QueryDatabaseBuffer(result);
            return result;
        }

        public static RecognitionFaceData[] GetDatabase(
            this RecognitionModuleData moduleData
            ) {
            return RecognitionFaceData.FromDatabaseBuffer(
                moduleData.GetDatabaseBuffer());
        }
    }
}
