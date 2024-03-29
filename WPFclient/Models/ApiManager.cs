﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public static async Task<List<FileData>> GetServerFilesLastModifiedDateAsync()
        {
            string serverUrl = "http://a22946-8c78.g.d-f.pw/api/saveDates";

            List<FileData> saveDates = new List<FileData>();
            try
            {
                //Выполняем асинхронный GET-запрос к серверу для получения списка дат сохранения файлов
                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.GetAsync(serverUrl))
                {
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

            string username = GetLocalUserName();

            localFolderPath = localFolderPath.Replace("%username%", username);
            try
            {
                if (!Directory.Exists($@"{localFolderPath}\data"))
                {
                    Directory.CreateDirectory($@"{localFolderPath}\data");
                }

                string downloadUrl = $"{serverUrl}?fileName={fileName}";

                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.GetAsync(downloadUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string localFilePath = "default";
                        if (fileName.Contains("dll") || fileName.Contains("addin"))
                        {
                            localFilePath = Path.Combine(localFolderPath, fileName);
                        }
                        else
                        {
                            localFilePath = Path.Combine($@"{localFolderPath}\data\", fileName);
                        }

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
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

        /// <summary>
        /// Метод для получения имени пользователя на локальном компьютере
        /// </summary>
        /// <returns>Возвращает имя пользователя локльного компьютера</returns>
        public static string GetLocalUserName()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Split('\\').Last();
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
