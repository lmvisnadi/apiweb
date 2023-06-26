namespace Infrastructure
{
    public enum ResponseStatus
    {
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        Unprocessable = 422,
        NoDefined = 999
    }
}
