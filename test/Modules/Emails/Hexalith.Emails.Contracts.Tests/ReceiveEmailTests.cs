namespace Hexalith.Emails.Contracts.Tests
{
    using Hexalith.Emails.Application.Commands;
    using Hexalith.Emails.Contracts.ValueTypes;

    using FluentAssertions;

    using Xunit;

    public class ReceiveEmailTests
    {
        [Fact]
        public void Create_new_check_values()
        {
            var attachments = new Attachment[]
            {
                new (){Name = "File1", Content = "ABCD==" },
                new (){Name = "File2", Content = "FFEE==" },
                new (){Name = "File3", Content = "123456789==" }
            };
            ReceiveEmail command = new()
            {
                Attachments = attachments,
                Body = "Hello world!",
                CopyToRecipients = new[] { "mail1@tot.com", "mail2@titi.com" },
                EmailId = "kllhh544",
                Recipient = "reci@dada.com",
                Subject = "I am testing",
                ToRecipients = new[] { "ggg@hello.com", "ggghh@nan.info", "reci@dada.com" }
            };
            command.Attachments.Should().BeEquivalentTo(attachments);
            command.Body.Should().Be("Hello world!");
            command.CopyToRecipients.Should().BeEquivalentTo(new[] { "mail1@tot.com", "mail2@titi.com" });
            command.EmailId.Should().Be("kllhh544");
            command.Recipient.Should().Be("reci@dada.com");
            command.Subject.Should().Be("I am testing");
            command.ToRecipients.Should().BeEquivalentTo(new[] { "ggg@hello.com", "ggghh@nan.info", "reci@dada.com" });
        }

        [Fact]
        public void Create_new_default_should_be_empty()
        {
            ReceiveEmail command = new();
            command.Attachments.Should().BeEmpty();
            command.Body.Should().BeEmpty();
            command.CopyToRecipients.Should().BeEmpty();
            command.EmailId.Should().BeEmpty();
            command.Recipient.Should().BeEmpty();
            command.Subject.Should().BeEmpty();
            command.ToRecipients.Should().BeEmpty();
        }
    }
}