namespace PhishingEmailScanner.Tests
{
    public class AllowedSendersRuleOverrideTests
    {
        private class TestMailItem : IMailItem
        {
            public string Subject { get; set; }
            public string Body { get; set; }
            public string SenderEmail { get; set; }
            public List<string> Links { get; set; }
            public List<string> Attachments { get; set; }
        }

        [Fact]
        public void IsMatch_ReturnsTrue_WhenSenderIsAllowed()
        {
            var rule = new AllowedSendersRuleOverride
            {
                allowed_senders_ = new HashSet<string> { "test@example.com" }
            };
            var mail = new TestMailItem { SenderEmail = "test@example.com" };

            Assert.True(rule.IsMatch(mail));
        }

        [Fact]
        public void IsMatch_ReturnsFalse_WhenSenderIsNotAllowed()
        {
            var rule = new AllowedSendersRuleOverride
            {
                allowed_senders_ = new HashSet<string> { "allowed@example.com" }
            };
            var mail = new TestMailItem { SenderEmail = "notallowed@example.com" };

            Assert.False(rule.IsMatch(mail));
        }

        [Fact]
        public void IsMatch_ReturnsFalse_WhenAllowedSendersIsEmpty()
        {
            var rule = new AllowedSendersRuleOverride
            {
                allowed_senders_ = new HashSet<string>()
            };
            var mail = new TestMailItem { SenderEmail = "anyone@example.com" };

            Assert.False(rule.IsMatch(mail));
        }
    }
}