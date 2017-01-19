using GenericWebSample.Application.Extensions;
using Microsoft.Owin.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericWebSample.Application.Handlers
{
    public class HttpLoggingDelegatingHandler : DelegatingHandler
    {
        private readonly bool _disposeInnerHandler;
        private readonly ILogger _logger;

        public HttpLoggingDelegatingHandler(ILogger logger)
        {
            _logger = logger;
            _disposeInnerHandler = true;
        }

        public HttpLoggingDelegatingHandler(ILogger logger, bool disposeInnerHandler)
        {
            _logger = logger;
            _disposeInnerHandler = disposeInnerHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        //-> Option to handle exception here by accessing "task.Exception" or handle outside of the "ContinueWith" method.
                    }

                    //-> Accessing "task.Result" when an exception has occured will cause the wrapped exception which is the just the "task.Exception" instance to be re-thrown at that point.
                    var responseMessage = task.Result;

                    //-> Calling "responseMessage.EnsureSuccessStatusCode" will throw a HttpRequestException if the status code is not a "200 OK" success status.
                    //-> Again, this can be handled internally or allowed to be handled externally by code further up the call stack.
                    responseMessage.EnsureSuccessStatusCode();

                    if (responseMessage.IsSuccessStatusCode || task.IsCanceled)
                        return responseMessage;

                    var sb = new StringBuilder();

                    sb.AppendLine($"Unsuccessful Request:");
                    sb.AppendLine($"--> Request Uri: {request.RequestUri.AbsolutePath}");
                    sb.AppendLine($"--> Request Message: {request.Content.ReadAsStringAsync().Result}");
                    sb.AppendLine($"--> Response Message: {responseMessage.Content.ReadAsStringAsync().Result}");
                    sb.AppendLine($"--> Response StatusCode: {responseMessage.StatusCode}");

                    _logger.WriteInformation(sb.ToString());

                    return responseMessage;

                }, cancellationToken);

                return response;
            }
            catch (WebException webEx)
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Unsuccessful Request:");
                sb.AppendLine($"--> Request Uri: {request.RequestUri.AbsolutePath}");
                sb.AppendLine($"--> Request Message: {request.Content.ReadAsStringAsync().Result}");
                sb.AppendLine($"--> Exception Message: {webEx.Message}");
                sb.AppendLine($"--> Exception Data: {Environment.NewLine} {webEx.Data.ToStringContent()}");
                sb.AppendLine($"--> Status: {webEx.Status}");

                _logger.WriteError(sb.ToString(), webEx);

                throw;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Unsuccessful Request:");
                sb.AppendLine($"--> Request Uri: {request.RequestUri.AbsolutePath}");
                sb.AppendLine($"--> Request Message: {request.Content.ReadAsStringAsync().Result}");
                sb.AppendLine($"--> Exception Message: {ex.Message}");

                _logger.WriteError(sb.ToString(), ex);

                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposeInnerHandler)
                base.Dispose(disposing);
        }
    }
}