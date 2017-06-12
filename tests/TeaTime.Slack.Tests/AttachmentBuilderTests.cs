﻿namespace TeaTime.Slack.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Models;
    using Xunit;

    public class AttachmentBuilderTests
    {
        [Fact]
        public void BuildOptions_SixOptions_TwoAttachments()
        {
            var options = new List<Option>
            {
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test1"
                },
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test2"
                },
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test3"
                },
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test4"
                },
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test5"
                },
                new Option
                {
                    Id = Guid.NewGuid(),
                    Name = "test6"
                },
            };

            var attachments = AttachmentBuilder.BuildOptions(options).ToList();

            Assert.Equal(2, attachments.Count);

            var attachment1 = attachments[0];
            var attachment2 = attachments[1];

            Assert.Equal(5, attachment1.Actions.Count);
            Assert.Equal(1, attachment2.Actions.Count);

            Assert.Equal(options[5].Id.ToString(), attachment2.Actions[0].Value);
        }

    }
}