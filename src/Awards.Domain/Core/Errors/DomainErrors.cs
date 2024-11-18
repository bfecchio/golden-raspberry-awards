using Awards.Domain.Core.Primitives;

namespace Awards.Domain.Core.Errors
{
    public static class DomainErrors
    {
        public static class General
        {
            public static Error UnProcessableRequest => new Error(
                "General.UnProcessableRequest",
                "The server could not process the request.");

            public static Error ServerError => new Error("General.ServerError", "The server encountered an unrecoverable error.");
        }
    }
}
