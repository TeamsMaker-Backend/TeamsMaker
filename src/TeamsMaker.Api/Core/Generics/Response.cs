using TeamsMaker.Api.Core.ResultMessages;

namespace Core.Generics;

public class Response<T> where T : class
{
    /// <summary>
    ///    Unified response object, Use it in case of returning success response
    /// </summary>
    /// <param name="successObject">object: object that will be returned into "ReturnObject" field</param>
    /// <param name="engMessage">strig: English message that describe what happened</param>
    /// <returns>ResultMessage Object</returns>
    public ResultMessage SuccessResponse(object? successObject, string engMessage = "Successful operation")
        => ResponseObject(successObject, null, true, engMessage);


    /// <summary>
    ///     Unified response object, Use it in case of returning failed response
    /// </summary>
    /// <param name="exceptionString">string: exception stack or any error message that will be returned into "exception" field</param>
    /// <param name="engMessage">string: English message that describe what happened</param>
    /// <returns>ResultMessage Object</returns>
    public ResultMessage FailureResponse(string exceptionString, string engMessage = "Failed operation")
        => ResponseObject(null, exceptionString, false, engMessage);

    /// <summary>
    ///    Unified response object, Use it in case of returning paginated success response
    /// </summary>
    /// <param name="successObject">object: object that will be returned into "ReturnObject" field</param>
    /// <param name="engMessage">strig: English message that describe what happened</param>
    /// <returns>ResultMessageWithPagination Object</returns>
    public ResultMessageWithPagination SuccessResponseWithPagination(PagedList<T> successObject, string engMessage = "Successful operation", string locMessage = "تم بنجاح")
        => ResponseWithPagination(successObject, null, true, engMessage, locMessage);

    public ResultMessageWithPagination SuccessResponseWithPagination<TType>(PagedList<TType> successObject, string engMessage = "Successful operation", string locMessage = "تم بنجاح") where TType : class
        => ResponseWithPagination(successObject, null, true, engMessage, locMessage);


    /// <summary>
    ///     Generic, Private Method - don't use it without wrapping it within another method
    ///     It meant to be a central place that holds a pagenated response object 
    /// </summary>
    /// <param name="successObject">object: object that will be returned into "ReturnObject" field</param>
    /// <param name="exceptionString">string: exception stack or any error message that will be returned into "exception" field</param>
    /// <param name="isSuccessed">boolean: represent request is success or not</param>
    /// <param name="engMessage">string: English message that describe what happened</param>
    /// <returns>ResultMessage Object</returns>
    private ResultMessage ResponseObject(object? successObject, string? exceptionString, bool isSuccessed, string engMessage)
    {
        return new ResultMessage
        {
            EngMsg = engMessage,
            Success = isSuccessed,
            Exception = exceptionString,
            ReturnObject = successObject
        };
    }


    /// <summary>
    ///     Generic, Private Method - don't use it without wrapping it within another method
    ///     It meant to be a central place that holds a pagenated response object 
    /// </summary>
    /// <param name="successObject">object: object that will be returned into "ReturnObject" field</param>
    /// <param name="exceptionString">string: exception stack or any error message that will be returned into "exception" field</param>
    /// <param name="isSuccessed">boolean: represent request is success or not</param>
    /// <param name="engMessage">string: English message that describe what happened</param>
    /// <returns>ResultMessageWithPagination Object</returns>
    private ResultMessageWithPagination ResponseWithPagination<TType>(PagedList<TType> successObject, string? exceptionString, bool isSuccessed, string engMessage, string locMessage) where TType : class
    {
        return new ResultMessageWithPagination
        {
            EngMsg = engMessage,
            Success = isSuccessed,
            Exception = exceptionString,
            ReturnObject = successObject,
            pagination = new PaginationData
            {
                CurrentPage = successObject.CurrentPage,
                TotalPages = successObject.TotalPages,
                PageSize = successObject.PageSize,
                TotalCount = successObject.TotalCount,
                HasPrevious = successObject.HasPrevious,
                HasNext = successObject.HasNext
            }
        };
    }
}

// private ResultMessage FailureResponseObject(object? successObject,
//     string? exceptionString,
//     bool isSuccessed,
//     string engMessage,
//     int? status = null,
//     string? instance = null)
// {
//     return new FailureResultMessage
//     {
//         EngMsg = engMessage,
//         Success = isSuccessed,
//         exception = exceptionString,
//         ReturnObject = successObject,
//         Status = status,
//         Instance = instance
//     };
// }