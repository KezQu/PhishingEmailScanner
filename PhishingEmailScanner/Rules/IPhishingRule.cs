namespace PhishingEmailScanner
{
    public interface IPhishingRule
    {
        string Name { get; }
        int Score { get; }

        bool IsMatch(IMailItem mail);
    }
}
