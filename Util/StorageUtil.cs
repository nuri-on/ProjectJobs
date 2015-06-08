using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Runtime.Serialization;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Popups;
using ProjectJobs.Model;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace ProjectJobs.Util
{
    public class StorageUtil
    {
        private static async Task<string[]> getJobIds(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("folderName");
            }

            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            return (from file in await folder.GetFilesAsync() select file.DisplayName).ToArray();
        }


        public static async Task SaveJob<T>(string folderName, T job) where T : Job
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("folderName");
            }

            if (job == null)
            {
                throw new ArgumentException("job");
            }

            try
            {
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(job.Id.ToString(), CreationCollisionOption.ReplaceExisting);
                var accessStream = await file.OpenAsync(FileAccessMode.ReadWrite);

                using (var outputStream = accessStream.GetOutputStreamAt(0))
                {
                    var dataContractJsonSerializer = new DataContractJsonSerializer(typeof (T));

                    dataContractJsonSerializer.WriteObject(outputStream.AsStreamForWrite(), job);

                    await outputStream.FlushAsync();

                    outputStream.Dispose();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public static async Task DeleteJob<T>(string folderName, T job) where T : Job
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("folderName");
            }

            if (job == null)
            {
                throw new ArgumentException("job");
            }

            try
            {
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(job.Id.ToString(), CreationCollisionOption.ReplaceExisting);
                var accessStream = await file.OpenAsync(FileAccessMode.ReadWrite);

                await file.DeleteAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public static async Task<List<Job>> Restore(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("folderName");
            }

            List<Job> jobs = new List<Job>();

            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            var ids = await getJobIds(folderName);

            if (ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    var file = await folder.GetFileAsync(id);

                    try
                    {
                        using (var inputStream = await file.OpenSequentialReadAsync())
                        {
                            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Job));
                            var job = (Job)dataContractJsonSerializer.ReadObject(inputStream.AsStreamForRead());

                            jobs.Add(job);

                            inputStream.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }

                return jobs;
            }
            else
            {
                return null;
            }
        }



        /*
        private static List<object> _data = new List<object>();
        public static List<object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private const string fileName = "pin.xml";


        static async public Task Save<T>()
        {
            try
            {
                await ThreadPool.RunAsync((sender) => StorageUtil.SaveAsync<T>().Wait(), WorkItemPriority.Normal);
            }
            catch (Exception e)
            {
                // TODO
            }
        }


        static async public Task Restore<T>()
        {
            try
            {
                await ThreadPool.RunAsync((sender) => StorageUtil.RestoreAsync<T>().Wait(), WorkItemPriority.Normal);
            }
            catch (Exception e)
            {
                // TODO
            }
        }

        static async private Task SaveAsync<T>()
        {
            try
            {
                
                StorageFile storeageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream randomAccessStream = await storeageFile.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream outputStream = randomAccessStream.GetOutputStreamAt(0);

                var dataContractSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
                dataContractSerializer.WriteObject(outputStream.AsStreamForWrite(), _data);

                await outputStream.FlushAsync();
               

                


                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("vegemilnoid: " + e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        static async private Task RestoreAsync<T>()
        {
            try
            {
                
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

                if (storageFile == null)
                {
                    return;
                }

                IInputStream inputStream = await storageFile.OpenReadAsync();

                var dataContractSerializer = new DataContractSerializer(typeof(List<Object>), new Type[] { typeof(T) });

                _data = (List<object>)dataContractSerializer.ReadObject(inputStream.AsStreamForRead());
                

                // var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(fileName, CreationCollisionOption.OpenIfExists);
                // var file = await folder.CreateFileAsync 

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("vegemilnoid: " + e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
        */
    }
}
