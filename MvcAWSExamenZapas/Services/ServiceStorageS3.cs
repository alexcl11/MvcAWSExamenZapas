using Amazon.S3.Model;
using Amazon.S3;
using System.Net;

namespace MvcAWSExamenZapas.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;

        //LA CLASE/INTERFACE PARA TRABAJAR CON LOS BUCKETS
        //SE LLAMA IAmazonS3 Y VAMOS A RECIBIRLA MEDIANTE
        //INYECCION
        private IAmazonS3 ClientS3;

        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clientS3)
        {
            this.BucketName = "bucket-zapatillas-examen-acl";
            this.ClientS3 = clientS3;
        }

        //COMENZAMOS CREANDO UN METODO PARA SUBIR FICHEROS
        public async Task<bool> UploadFileAsync(string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                Key = fileName,
                BucketName = this.BucketName,
                InputStream = stream
            };
            //PARA TRABAJAR SE UTILIZA LA CLASE IAmazonS3 
            //CON UNA PETICION DE PUTOBJECT
            PutObjectResponse response = await this.ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}