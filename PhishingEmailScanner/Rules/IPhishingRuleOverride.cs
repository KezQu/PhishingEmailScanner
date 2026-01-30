namespace PhishingEmailScanner.Rules
{
    public interface IPhishingRuleOverride
    {
        string Name { get; }
        bool IsMatch(IMailItem mail);
    }
}
