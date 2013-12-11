using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Speech.Tts;
using Com.Google.Android.Glass.App;

namespace GlassGetBitcoinPrice
{

	[Activity(Label = "BitCoin", MainLauncher = true, Icon = "@drawable/icon")]
	[IntentFilter(new[] { "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trigger_start")]
	public class BitCoinMainActivity : Activity, TextToSpeech.IOnInitListener
    {
		private TextToSpeech 	tts;
		private int 			result = -1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			tts = new TextToSpeech(this, this);

			var card = new Card(this);
			card.SetText("Fetching Bitcoin price");
			card.SetInfo("Source: bitstamp.net");
            SetContentView(card.ToView());

			GetPrice();
        }

		/// <summary>
		/// Initialize the Text to Speech Engine
		/// </summary>
		/// <param name="status">Status.</param>
		public void OnInit(OperationResult status)
		{
			if (status == OperationResult.Success) {
				var r = tts.SetLanguage(Java.Util.Locale.Us);
				if (r == LanguageAvailableResult.MissingData || r == LanguageAvailableResult.NotSupported) {
					Console.WriteLine("TTS Unsupported Language");
					result = -1;
				} else {
					result = 0;
				}
			}
			else {
				Console.WriteLine("TTS Init Failed");
				result = -1;
			}
		}

		public async Task<int> GetPrice()
		{
			Model.BitCoinPrice bitcoin = await Model.BitCoinPrice.Load();

			SayBTCPrice(bitcoin.last);
				
			return 0;
		}

		public void SayBTCPrice(string price) 
		{
			try 
			{
				this.RunOnUiThread(() => {
					int spaceIndex = price.IndexOf(".");
					if (spaceIndex != -1)
						price = price.Substring(0, spaceIndex);

					Card card = new Card(this.ApplicationContext);
					card.SetText("1x Bitcoin = $" + price);
					SetContentView(card.ToView());

					string text = "The current bit coin price is " + price + " dollars";
					if(result >= 0) {
							tts.Speak(text, QueueMode.Flush, new Dictionary<string, string>());
					}
				});
			}
			catch (Exception e) {
				Console.WriteLine("Error in SayBTCPrice:" + e.Message);
			}
		}
    }
}

