namespace Hexalith.Users.Application.ModelViews
{
    using System;

    public record UserDetailedInformations(string Id, string Name, string FirstName, string LastName, DateTime BirthDate)
    {
    }
}