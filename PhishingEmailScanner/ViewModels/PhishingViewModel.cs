using PhishingEmailScanner;
using PhishingEmailScanner.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;

public class PhishingViewModel : INotifyPropertyChanged
{
    private readonly WhitelistCacheManager _whitelistCacheManager;
    public event PropertyChangedEventHandler PropertyChanged;
    private string statusText;
    private PhishingConfidenceLevel risk;
    private string currentSender;
    private string currentDomain;
    public string StatusText
    {
        get => statusText;
        set
        {
            if (statusText != value)
            {
                statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
    }

    public PhishingConfidenceLevel Risk
    {
        get => risk;
        set
        {
            if (risk != value)
            {
                risk = value;
                OnPropertyChanged(nameof(Risk));
            }
        }
    }

    public string CurrentSender
    {
        get => currentSender;
        set
        {
            if (currentSender != value)
            {
                currentSender = value;
                OnPropertyChanged(nameof(CurrentSender));
            }
        }
    }
    public string CurrentDomain
    {
        get => currentDomain;
        set
        {
            if (currentDomain != value)
            {
                currentDomain = value;
                OnPropertyChanged(nameof(CurrentDomain));
            }
        }
    }

    public PhishingViewModel(WhitelistCacheManager whitelistCacheManager)
    {
        _whitelistCacheManager = whitelistCacheManager;
        AddDomainCommand = new RelayCommand(_ => AddDomain());
        AddSenderCommand = new RelayCommand(_ => AddSender());
    }

    public ICommand AddDomainCommand { get; }
    public ICommand AddSenderCommand { get; }

    public ObservableCollection<string> Violations { get; }
        = new ObservableCollection<string>();

    public ObservableCollection<string> Overrides { get; }
        = new ObservableCollection<string>();

    protected void OnPropertyChanged( string propertyName )
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Update( PhishingAnalysisResult result, string sender, string domain)
    {
        Violations.Clear(); 
        Overrides.Clear();

        foreach (var v in result.TriggeredRules)
            Violations.Add(v);

        foreach (var v in result.TriggeredRuleOverrides)
            Overrides.Add(v);

        Risk = GetHighest(result.ConfidenceLevel);

        StatusText = GetStatus(result.ConfidenceLevel);
    }

    private string GetStatus(PhishingConfidenceLevel level)
    {
        if (level.HasFlag(PhishingConfidenceLevel.kCritical))
            return "Wykryto krytyczne ryzyko phishingu!!!";
        if (level.HasFlag(PhishingConfidenceLevel.kHigh))
            return "Wykryto wysokie ryzyko phishingu!";
        if (level.HasFlag(PhishingConfidenceLevel.kMedium))
            return "Wykryto średnie ryzyko phishingu.";
        if (level.HasFlag(PhishingConfidenceLevel.kModerate))
            return "Wykryto umiarkowane ryzyko phishingu.";
        if (level.HasFlag(PhishingConfidenceLevel.kLow))
            return "Wykryto niskie ryzyko phishingu.";
        if (level.HasFlag(PhishingConfidenceLevel.kOverriden))
            return "Wykrywanie phishingu nadpisane przez białą listę.";

        return "E-mail wydaje się być bezpieczny.";
    }

    public static PhishingConfidenceLevel GetHighest( PhishingConfidenceLevel level )
    {
        if (level.HasFlag(PhishingConfidenceLevel.kCritical))
            return PhishingConfidenceLevel.kCritical;
        if (level.HasFlag(PhishingConfidenceLevel.kHigh))
            return PhishingConfidenceLevel.kHigh;
        if (level.HasFlag(PhishingConfidenceLevel.kMedium))
            return PhishingConfidenceLevel.kMedium;
        if (level.HasFlag(PhishingConfidenceLevel.kModerate))
            return PhishingConfidenceLevel.kModerate;
        if (level.HasFlag(PhishingConfidenceLevel.kLow))
            return PhishingConfidenceLevel.kLow;
        if (level.HasFlag(PhishingConfidenceLevel.kOverriden))
            return PhishingConfidenceLevel.kOverriden;

        return PhishingConfidenceLevel.kNone;
    }

    private void AddDomain()
    {
        if (!string.IsNullOrEmpty(CurrentDomain))
        {
            _whitelistCacheManager.AddToWhitelist("Domains", CurrentDomain);
        }
    }

    private void AddSender()
    {
        if (!string.IsNullOrEmpty(CurrentSender))
        {
            _whitelistCacheManager.AddToWhitelist("Senders", CurrentSender);
        }
    }
}