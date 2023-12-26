using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace WPFclient.Models
{
    public static class ApiManager
    {
        /// <summary>
        /// Метод для получения информации о файле на сервере
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Возвращает информацию о файле на сервере</returns>
        public static async Task<List<FileData>> GetServerFilesLastModifiedDateAsync(HttpClient httpClient, string serverUrl)
        {
            List<FileData> saveDates = new List<FileData>();
            try
            {
                //Выполняем асинхронный GET-запрос к серверу для получения списка дат сохранения файлов
                HttpResponseMessage response = await httpClient.GetAsync(serverUrl);

                if (response.IsSuccessStatusCode)
                {
                    //Парсим ответ сервера и возвращаем дату сохранения файлов
                    saveDates = await response.Content.ReadAsAsync<List<FileData>>();
                }
                else 
                {
                    MessageBox.Show($"Не удалось получить список дат сохранений файлов!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке запроса {ex.Message}").ToString();
            }
            return saveDates;
        }

        /// <summary>
        /// Метод для скачивания обновлений с сервера
        /// </summary>
        /// <param name="httpClient">Экземпляр Http клиента</param>
        /// <param name="serverUrl">ссылка на метод API для запроса</param>
        /// <param name="fileName">Путь к файлу для скачивания</param>
        /// <returns></returns>
        public static async Task DownloadFileAsync(string fileName, string localFolderPath)
        {
            string serverUrl = "http://a22946-8c78.g.d-f.pw/api/file";
            try
            {
                string downloadUrl = $"{serverUrl}?fileName={fileName}";

                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.GetAsync(downloadUrl))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        string localFilePath = Path.Combine(localFolderPath, $"{fileName}.dll");

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                        MessageBox.Show($"Файл {fileName} успешно скачан");
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при скачивании: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке/сохранении файла: {ex.Message}");
            }

        }
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
