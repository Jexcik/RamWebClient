using System.Net.Http;
using System.Threading.Tasks;
using System;
using static RAMWebServer.Models.FileSaveDateController;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RAMWebServer.Services
{
    public static class FileService
    {
        /// <summary>
        /// Метод получения даты последнего сохранения файла в случае если файл находится на удаленном сервере
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<DateTime> GetServerFileLastModifiedDateAsync(HttpClient httpClient)
        {
            DateTime fileDate = new DateTime();

            //GET-запрос к серверу для получения даты последнего изменения
            HttpResponseMessage response = await httpClient.GetAsync("Введите API");

            if (response.IsSuccessStatusCode)
            {
                string dateAsString = await response.Content.ReadAsStringAsync();
                if (DateTime.TryParse(dateAsString, out fileDate))
                {
                    return fileDate;
                }
                else
                {
                    throw new InvalidOperationException("Не удалось преобразовать строку в дату!");
                }
            }
            return fileDate;
        }


        /// <summary>
        /// Метод для получения даты последнего сохранения файла в случае если файл находится на локальном сервере
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Task<DateTime> GetLocalFileLastModifiedDateAsync(string filePath)
        {
            try
            {
                return Task.FromResult(File.GetLastWriteTime(filePath));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при чтении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Метод считывающий информацию с XML файла
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static List<FileData> ReadRepositoriesFromXml(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<FileData>));

            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                return (List<FileData>)serializer.Deserialize(fileStream);
            }
        }
    }
}
