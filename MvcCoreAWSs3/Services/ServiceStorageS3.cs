using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace MvcCoreAWSs3.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;

        private IAmazonS3 clientS3;

        public ServiceStorageS3 (IConfiguration configuration, IAmazonS3 clientS3)
        {
            this.BucketName = configuration.GetValue<string>("AWS:BucketName");
            this.clientS3 = clientS3;
        }

        // METODO PARA SUBIR FICHEROS
        public async Task<int> UploadFileAsync(string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                Key = fileName,
                BucketName = this.BucketName,
                InputStream = stream,
            };
            PutObjectResponse response = await this.clientS3.PutObjectAsync(request);
            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                // AQUI HARIAMOS LO QUE FUERA..
            }
            int code = (int)response.HttpStatusCode;
            return code;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            DeleteObjectResponse response = await this.clientS3.DeleteObjectAsync(this.BucketName, fileName);
        }

        /* METODO PARA RECUPERAR TODOS LOS FICHEROS
         * DEBEMOS INDICAR LA VERION AUNQ NO TENGAMOS */
        public async Task<List<string>> GetFilesAsync()
        {
            ListVersionsResponse response = await this.clientS3.ListVersionsAsync(this.BucketName);
            if(response.Versions == null)
            {
                return null;
            }
            /* EXTRAEMOS LAS KEYS (FILENAME) POR DEFECTO NOS DEVUELVE LA ULTIMA VERSION */
            return response.Versions.Select(f => f.Key).ToList();
        }
    }
}
