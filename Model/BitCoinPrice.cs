using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace GlassGetBitcoinPrice.Model
{
	public class BitCoinPrice
	{
		//	{"high": "989.60", "last": "962.00", "timestamp": "1386733158", "bid": "962.00", "volume": "19739.99168325", "low": "895.76", "ask": "962.49"}
		public string high { get; set; }
		public string last { get; set; }
		public string timestamp { get; set; }
		public string bid  { get; set; }
		public string low  { get; set; }
		public string ask  { get; set; }
		public string volume { get; set; }

		public BitCoinPrice()
		{
		}

		public static async System.Threading.Tasks.Task<BitCoinPrice> Load()
		{
			Console.WriteLine("Requesting data from: https://coinbase.com/api/v1/currencies/exchange_rates");
			HttpWebRequest request = new HttpWebRequest(new Uri("https://www.bitstamp.net/api/ticker/"));//"https://coinbase.com/api/v1/currencies/exchange_rates"));
			request.Method 		= "GET";
			request.ContentType = "application/json";

			string res = null;

			ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;
			try {
				var response = await request.GetResponseAsync().ConfigureAwait(false);
				using (var respStream = response.GetResponseStream()) {
					res = readResponse(respStream);
				}
			} catch (WebException ex) {
				if (ex.Response != null && ex.Status == WebExceptionStatus.ProtocolError)
					using (var respStream = ex.Response.GetResponseStream()) {
						res = readResponse(respStream);
					}
				else {
					throw;
				}
			}
			catch {
				throw;
			}

			var ser = new JsonSerializer<Model.BitCoinPrice>();
			var item = ser.DeserializeFromString(res);//data.ToJson());
			return item;
		}

		private static string readResponse(Stream resultStream)
		{
			string result = null;
			//pump the network stream into a local memory buffer.. 
			using (MemoryStream localMem = new MemoryStream()) {
				byte[] buffer = new byte[4096];
				int read = resultStream.Read (buffer, 0, buffer.Length);
				while (read > 0) {
					localMem.Write (buffer, 0, read);
					read = resultStream.Read(buffer, 0, buffer.Length);
				}

				//convert the full memory stream into UTF8 text
				localMem.Seek (0, SeekOrigin.Begin);
				using (var reader = new StreamReader(localMem, new System.Text.UTF8Encoding(false,false))) {
					result = reader.ReadToEnd();
				}
			}
			return result;
		}

	}
}

