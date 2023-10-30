using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataDto.Air;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace GoStay.Service.Api.Air.Base
{
	public class ApiAirBase
	{
		public virtual ResultData<T> Get<T>(string url, params KeyValuePair<string,object>[] args)
		{
			try
			{
                string param = "";
                if (args.Length > 0)
                {
                    param = "?";

                    for(int i=0;i<  args.Count();i++)
					{
						if(i==0)
						{
                            param += $"{args[i].Key}={args[i].Value}";
                        }
						else
						{
                            param += $"&{args[i].Key}={args[i].Value}";
                        }

                    }
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(@$"{AppConfigs.ApiAir}/{url}{param}");
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));

				// List data response.
				HttpResponseMessage response = client.GetAsync("").Result;
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var str = response.Content.ReadAsStringAsync().Result;
					var result = JsonConvert.DeserializeObject<ResultData<T>>(str)!;
					if(result != null)
					{
						return result;
					}
				}
                else
                {
                    return new ResultData<T>
                    {
                        Code = response.StatusCode.ToString(),
                        Message = response.StatusCode.GetEnumDescription()
                    };
                }
			}
			catch (Exception ex)
			{
                return new ResultData<T>
                {
                    Code = "99",
                    Message = $"Service base exception :{ex.Message}"
                };
                // Log error
            }

			return default(ResultData<T>);
		}

        public virtual ResultData<T> Delete<T>(string url, params KeyValuePair<string, object>[] args)
        {
            try
            {
                string param = "";
                if (args.Length > 0)
                {
                    param = "?";

                    for (int i = 0; i < args.Count(); i++)
                    {
                        if (i == 0)
                        {
                            param += $"{args[i].Key}={args[i].Value}";
                        }
                        else
                        {
                            param += $"&{args[i].Key}={args[i].Value}";
                        }

                    }
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(@$"{AppConfigs.ApiAir}/{url}{param}");
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.DeleteAsync("").Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ResultData<T>>(str)!;
                    if (result != null)
                    {
                        return result;
                    }
                }
                else
                {
                    return new ResultData<T>
                    {
                        Code = response.StatusCode.ToString(),
                        Message = response.StatusCode.GetEnumDescription()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultData<T>
                {
                    Code = "99",
                    Message = $"Service base exception :{ex.Message}"
                };
                // Log error
            }

            return default(ResultData<T>);
        }

        public virtual ResultData<Tout> Post<Tin,Tout>(string url, Tin body, params string[] args)
		{
			try
			{
                

                string Serialized = JsonConvert.SerializeObject(body);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                //var client = new HttpClient();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpContent content = new StringContent(Serialized, Encoding.Unicode, "application/json");
				var response =  client.PostAsync(@$"{AppConfigs.ApiAir}/{url}", content).Result;

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var str = response.Content.ReadAsStringAsync().Result;
					var result = JsonConvert.DeserializeObject<ResultData<Tout>>(str)!;
					if (result != null)
					{
						return result;
					}
				}
                else
                {
                    return new ResultData<Tout>
                    {
                        Code = response.StatusCode.ToString(),
                        Message = response.StatusCode.GetEnumDescription()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultData<Tout>
                {
                    Code = "99",
                    Message = $"Service base exception :{ex.Message}"
                };
                // Log error
            }

            return default(ResultData<Tout>);
        }

        public virtual ResultData<Tout> Put<Tin, Tout>(string url, Tin body, params string[] args)
        {
            try
            {
                string Serialized = JsonConvert.SerializeObject(body);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(Serialized, Encoding.Unicode, "application/json");
                var response = client.PutAsync(@$"{AppConfigs.ApiAir}/{url}", content).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ResultData<Tout>>(str)!;
                    if (result != null)
                    {
                        return result;
                    }
                }
                else
                {
                    return new ResultData<Tout>
                    {
                        Code = response.StatusCode.ToString(),
                        Message = response.StatusCode.GetEnumDescription()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultData<Tout>
                {
                    Code = "99",
                    Message = $"Service base exception :{ex.Message}"
                };
                // Log error
            }

            return default(ResultData<Tout>);
        }
    }
}