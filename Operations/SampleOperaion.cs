using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ebo.Plugins.Integration;
using Ebo.Plugins.Integration.Domain.OperationRequests;
using Ebo.Plugins.Integration.Domain.OperationResponses;
using Ebo.Plugins.Integration.Domain.OperationResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleIntegration;

namespace Stanleybet.API.Operations
{
    public class SampleOperation : EboBotIntegrationBase<SampleOperation>
    {
        public override async Task<OperationResponse> Operation(IOperationRequest operationRequest)
        {
            var predefinedListString = ApiConfiguration[Constants.PredefinedList];

            List<string> predefinedList = predefinedListString.Split(',').Select(x => x.Trim()).ToList();


            AssertRequiredApiParameters(operationRequest.ApiPropertyBag);
            var response = new OperationResponse();
            var inputString = operationRequest.ApiPropertyBag["input"]?.ToString()?.Trim();
            List<string> inputList = new List<string>();
            List<string> returnList = new List<string>();
            try
            {
                 inputList = JsonConvert.DeserializeObject<List<string>>(inputString);
            }
            catch(Exception e)
            {
                response.OperationResult = new TextResult
                {
                    ReplyText = $"Internal server error",
                    PropertyBag = new Dictionary<string, string>
                            {
                                {"Status", "500"},
                                {"Message", e.Message}
                            }
                };

                return await Task.FromResult(response);
            }

            if(predefinedList.Any(x => inputList.Contains(x)))
            {
                var filteredList = predefinedList.Where(x => inputList.Contains(x)).Distinct().ToList();
                returnList.AddRange(filteredList);
            }







            if (returnList.Count > 0)
            {
   
                
                    response.OperationResult = new TextResult
                    {
                        ReplyText = $"Sample Operation Has Value",
                        PropertyBag = new Dictionary<string, string>
                            {
                                {"Status", "200"},
                                {"ContainsList", string.Join(",", returnList)}
                            }
                    };
                    return await Task.FromResult(response);
                
            }else
            {
                response.OperationResult = new TextResult
                {
                    ReplyText = $"Sample Operation Has No value",
                    PropertyBag = new Dictionary<string, string>
                            {
                                {"Status", "404"},
                                {"Message", "No item found that matches a value in the predefined list"}
                            }
                };
            }

       

            return await Task.FromResult(response);

        }

        public override IOperationResult OperationResultType => new TextResult();
        public override List<string> RequiredApiParameters =>
            new List<string>
            {
                "input"
            };
        public override List<string> OptionalApiParameters { get; }
    }
}
