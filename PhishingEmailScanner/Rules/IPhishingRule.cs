namespace PhishingEmailScanner.Rules
{
    public interface IPhishingRule
    {
        string Name { get; }
        PhishingConfidenceLevel IsMatch(IMailItem mail);
    }
}
