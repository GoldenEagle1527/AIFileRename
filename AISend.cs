using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AiFileNameTranslation
{
    public class AISend
    {
        readonly string ApiKey;
        readonly string BaseUrl;
        readonly string ModelName;
        readonly string SystemPrompt;

        public AISend(string apiKey, string baseUrl, string modelName, string systemPrompt)
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
            ModelName = modelName;
            SystemPrompt = systemPrompt;
        }

        public async Task<string> GetOpenAIAnswer(string prompt, string userInput)
        {
            try
            {
                using var httpClient = new HttpClient();
                var api_key = ApiKey; // 替换为您的 API 密钥
                var base_url = BaseUrl; // 替换为您的 API 基础 URL

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {api_key}");
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // 构造请求数据
                var data = new
                {
                    model = ModelName,
                    messages = new List<dynamic>
                        {
                            new { role = "system", content = $"{SystemPrompt} 请额外遵循以下要求:{prompt}" },
                            new { role = "user", content = userInput }
                        },
                    temperature = 0.3
                };

                // 序列化请求数据
                var jsonRequest = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                // 创建请求内容
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // 发送请求
                var response = await httpClient.PostAsync($"{base_url}/chat/completions", content);

                // 检查响应状态码
                if (response.IsSuccessStatusCode)
                {
                    // 读取响应内容
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic responseData = JsonConvert.DeserializeObject(jsonResponse);
                    string answer = responseData.choices[0].message.content;
                    return answer;
                }
                else
                {
                    // 返回错误信息
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                // 捕获并返回异常信息
                return $"HttpRequestException: {ex.Message}";
            }
            catch (Exception ex)
            {
                // 捕获并返回其他异常信息
                return $"Exception: {ex.Message}";
            }
        }
    }
}