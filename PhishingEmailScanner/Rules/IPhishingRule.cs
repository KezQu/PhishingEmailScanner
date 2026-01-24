namespace PhishingEmailScanner
{
    public interface IPhishingRule
    {
        string Name { get; }
        bool IsMatch(IMailItem mail);
    }
}
