namespace SachkovTech.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null, string? name = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"{name ?? "record"} not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length)");
        }
        
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Record already exist");
        }
    }
    
    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Your credentials is invalid");
        }
    }
    
    public static class Files
    {
        public static Error InvalidExtension()
        {
            return Error.Validation("extension.is.invalid", "File extension is invalid");
        }
        
        public static Error InvalidSize()
        {
            return Error.Validation("size.is.invalid", "File size is invalid");
        }
    }
}