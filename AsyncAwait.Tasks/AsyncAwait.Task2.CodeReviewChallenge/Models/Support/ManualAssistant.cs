using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support
{
    public class ManualAssistant : IAssistant
    {
        private readonly ISupportService _supportService;

        public ManualAssistant(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        public async Task<string> RequestAssistanceAsync(string requestInfo)
        {
            try
            {
                await _supportService.RegisterSupportRequestAsync(requestInfo);

                Thread.Sleep(5000); // this is just to be sure that the request is registered

                string supportedInfo = await _supportService.GetSupportInfoAsync(requestInfo);
                return supportedInfo;
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to register assistance request. Please try later. {ex.Message}";
            }
        }
    }
}
