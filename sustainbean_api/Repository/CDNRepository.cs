using BunnyCDN.Net.Storage;
using RestSharp;
using sustainbean_api.Models;
using System.Buffers.Text;

namespace sustainbean_api.Repository
{
    public interface ICdnRepository
    {
        public Task<List<string>> UploadOnCdn(List<ImgBase64> imgList);


    }
    public class CDNRepository : ICdnRepository
    {
        private readonly IConfiguration _configuration;

        public CDNRepository(IConfiguration configuration)
        {

            _configuration = configuration;

        }
        public async Task<List<string>> UploadOnCdn(List<ImgBase64> imgList)
        {
            var Password = _configuration["CdnConfig:Password"];
            var Username = _configuration["CdnConfig:Username"];
            var AccessKey = _configuration["CdnConfig:AccessKey"];
            var bunnyCDNStorage = new BunnyCDNStorage(Username, Password);

            try
            {
                // Create a list to store the resulting file paths
                var uploadedFileUrls = new List<string>();

                //string fileExtension = ".jpg";
                var purgeUrlBase = _configuration["CdnConfig:PurgeUrl"] + _configuration["CdnConfig:Url"];

                for (int i = 0; i < imgList.Count; i++)
                {
                    //string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    //var fileName = timestamp + fileExtension;

                    //var FilePath = !string.IsNullOrEmpty(path) ? path : "products/" + fileName;
                    //FilePath = _configuration["CdnConfig:Path"] + FilePath;
                    var FilePath = _configuration["CdnConfig:Path"] + imgList[i].path;
                    var img = LoadImage(imgList[i].img);
                    await bunnyCDNStorage.UploadAsync(img, "/" + Username + FilePath);

                    //var options = new RestClientOptions("https://storage.bunnycdn.com/"+ Username + FilePath);
                    //var client = new RestClient(options);
                    //var request = new RestRequest("");
                    //request.AddHeader("AccessKey", Password);
                    //request.AddStringBody(base64Images[i], "application/octet-stream");
                    //var response = await client.PutAsync(request);



                    var purgeUrl = purgeUrlBase + FilePath;
                    var client = new RestClient(purgeUrl);
                    var request = new RestRequest(purgeUrl, Method.Get);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("AccessKey", AccessKey);
                    var responseP = await client.ExecuteGetAsync(request);

                    // Store the resulting file path
                    var uploadedFileUrl = _configuration["CdnConfig:Url"] + FilePath;
                    uploadedFileUrls.Add(uploadedFileUrl);
                }
                return uploadedFileUrls;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Stream LoadImage(string imgBase64)
        {
            byte[] bytes = Convert.FromBase64String(imgBase64);
            MemoryStream contents = new MemoryStream(bytes, 0, bytes.Length);

            return contents;
        }
    }
}
