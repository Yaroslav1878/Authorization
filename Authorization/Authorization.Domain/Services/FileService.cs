using System;
using System.Net.Http;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Services.Abstraction;

namespace Authorization.Domain.Services
{/*
    public class FileService : IFileService
    {
        private readonly IFileServiceConfiguration _fileServiceConfiguration;

        public FileService(IFileServiceConfiguration fileServiceConfiguration)
        {
            _fileServiceConfiguration = fileServiceConfiguration;
        }

        public Task DeleteFile(Guid fileKey)
        {
            using (var httpClient = new HttpClient())
            {
                string deleteFileEndpoint = $"{_fileServiceConfiguration.FileServiceUrl}/file/{fileKey}";
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, deleteFileEndpoint))
                {
                    httpClient.SendAsync(requestMessage);
                }
            }

            return Task.CompletedTask;
        }
    }*/
}