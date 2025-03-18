using System.Net.Http.Headers;
using System.Text;
using Model;
using Newtonsoft.Json;

namespace Service{
    public class ApiService{
        HttpClient httpClient;
        IConfiguration configuration;
        public ApiService(HttpClient httpClient,IConfiguration configuration){
            this.httpClient=httpClient;
            this.configuration=configuration;
            this.httpClient.BaseAddress=new Uri(configuration["ApiSetting:BaseURL"]);
        }
        public async Task<string> LoginAsync(Login login){
            var content=new StringContent(JsonConvert.SerializeObject(login),Encoding.UTF8,"application/json");
            var response=await httpClient.PostAsync("/Login",content);
            if(response.IsSuccessStatusCode){
                var tokenReponse=JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
                return tokenReponse.token;
            }
            return null;
        }
        public async Task<List<Article>> GetArticlesAsync(string token){
            httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);
            var response=await httpClient.GetAsync("/Get");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Article>>(await response.Content.ReadAsStringAsync());
        } 
        public async Task CreateUpdateArticlesAsync(string token,Article article){
            httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);
            var content=new StringContent(JsonConvert.SerializeObject(article),Encoding.UTF8,"application/json");
            var response=await httpClient.PostAsync("/Post",content);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteArticlesAsync(string token,int articleId){
            httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);
            var response=await httpClient.DeleteAsync($"/Delete/?articleId={articleId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
