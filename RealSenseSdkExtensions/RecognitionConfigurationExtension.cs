using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PXCMFaceConfiguration;

namespace RealSenseSdkExtensions {
    public static class RecognitionConfigurationExtension {
        public static void SetDatabase(
            this RecognitionConfiguration config,
            RecognitionFaceData[] dataArray) {
            foreach(var item in dataArray) {
                config.SetDatabaseBuffer(item.BinaryData);
            }
        }
    }
}
