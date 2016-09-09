using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealSenseSdkExtensions {
    public class FaceDatabaseManager {
        private PXCMSenseManager manager;
        private byte[] buffer = null;
        public FaceDatabaseManager(PXCMSenseManager manager) {
            this.manager = manager;
        }

        public void Add(RecognitionFaceData data) {
            using (var faceModule = manager.QueryFace())
            using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                buffer = faceModule.CreateOutput()
                    .QueryRecognitionModule()
                    .GetDatabaseBuffer();

                FaceDatabaseManager.AddBuffer(
                    buffer, data);

                moduleConfiguration
                    .QueryRecognition()
                    .SetDatabaseBuffer(buffer);
            }
        }

        public void Update(RecognitionFaceData data) {
            using (var faceModule = manager.QueryFace())
            using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                buffer = faceModule.CreateOutput()
                    .QueryRecognitionModule()
                    .GetDatabaseBuffer();

                FaceDatabaseManager.UpdateBuffer(
                    buffer, data);

                moduleConfiguration
                    .QueryRecognition()
                    .SetDatabaseBuffer(buffer);
            }
        }

        public void Delete(RecognitionFaceData data) {
            using (var faceModule = manager.QueryFace())
            using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                buffer = faceModule.CreateOutput()
                    .QueryRecognitionModule()
                    .GetDatabaseBuffer();

                FaceDatabaseManager.SoftDeleteBuffer(
                    buffer, data);

                moduleConfiguration
                    .QueryRecognition()
                    .SetDatabaseBuffer(buffer);
            }
        }                

        /// <summary>
        /// 更新資料庫緩衝區
        /// </summary>
        /// <param name="buffer">緩衝區</param>
        /// <param name="data">更新後的資料</param>
        public static void UpdateBuffer(byte[] buffer, RecognitionFaceData data) {
            Array.Copy(data.BinaryData, 0, buffer,
                data.PrimaryKey * RecognitionFaceData.Length,
                RecognitionFaceData.Length);
        }

        /// <summary>
        /// 自資料庫緩衝區完全刪除
        /// </summary>
        /// <param name="buffer">緩衝區</param>
        /// <param name="data">刪除目標</param>
        public static void HardDeleteBuffer(byte[] buffer, RecognitionFaceData data) {
            int DataCount = buffer.Length / RecognitionFaceData.Length;
            if (data.PrimaryKey != DataCount - 1) {
                Array.Copy(buffer,
                    data.PrimaryKey * (RecognitionFaceData.Length + 1),
                    buffer, data.PrimaryKey * RecognitionFaceData.Length, 
                    buffer.Length - data.PrimaryKey * RecognitionFaceData.Length);
            }
            Array.Resize(ref buffer, buffer.Length - RecognitionFaceData.Length);
        }

        /// <summary>
        /// 自資料庫緩衝區標示刪除
        /// </summary>
        /// <param name="buffer">緩衝區</param>
        /// <param name="data">刪除目標</param>
        public static void SoftDeleteBuffer(byte[] buffer, RecognitionFaceData data) {
            data.ForeignKey = -1;
            Array.Copy(BitConverter.GetBytes(-1), 0, buffer,
                data.PrimaryKey * RecognitionFaceData.Length + RecognitionFaceData.Length - 4,
                4);
        }

        /// <summary>
        /// 自資料庫緩衝區清除已經標示刪除的項目
        /// </summary>
        /// <param name="buffer">清除後的結果</param>
        public static void ClearRemovedBuffer(byte[] buffer) {
            var items = RecognitionFaceData.FromDatabaseBuffer(buffer);
            buffer = items.Where(x => x.ForeignKey != -1).Select((x, i) => {
                x.PrimaryKey = i;
                return x;
            }).ToArray().ToBinary();
        }

        /// <summary>
        /// 在資料庫緩衝區加入新資料
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="data"></param>
        public static void AddBuffer(byte[] buffer, RecognitionFaceData data) {
            if (data.PrimaryKey != buffer.Length / RecognitionFaceData.Length) {
                throw new ArgumentOutOfRangeException("PrimaryKey Error");
            }
            Array.Resize(ref buffer, buffer.Length + RecognitionFaceData.Length);
            UpdateBuffer(buffer, data);
        }
    }
}
