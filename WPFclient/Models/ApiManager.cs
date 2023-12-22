using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        /// Метод для скачивания файла с сервера
        /// </summary>
        /// <param name="httpClient">Экземпляр Http клиента</param>
        /// <param name="serverUrl">ссылка на метод API для запроса</param>
        /// <param name="filePath">Путь к файлу для скачивания</param>
        /// <returns></returns>
        public static async Task DownloadFileAsync(HttpClient httpClient, string serverUrl, string filePath, string localFolderPath)
        {
            using (HttpClient client = new HttpClient())
            {
                string downloadUrl = $"{serverUrl}?file={Uri.EscapeDataString(filePath)}";

                HttpResponseMessage response = await client.GetAsync(downloadUrl);

                if (response.IsSuccessStatusCode)
                {
                    string fileName = Path.GetFileName(filePath);
                    string localFilePath = Path.Combine(localFolderPath, fileName);

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                        fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
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
    }
}
