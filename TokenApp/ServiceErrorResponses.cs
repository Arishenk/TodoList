using System;
using System.Net;
using TodoItems.Client.Errors;

namespace TodoItems.API
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse TodoItemNotFound(string ItemId)
        {
            if (ItemId == null)
            {
                throw new ArgumentNullException(nameof(ItemId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A item with \"{ItemId}\" not found.",
                    Target = "item"
                }
            };

            return error;
        }

        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }
    }
}
