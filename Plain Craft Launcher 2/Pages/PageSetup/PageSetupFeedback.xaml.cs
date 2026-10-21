using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.Json.Serialization;
using PCL.Network;
using PCL.Core.App.Localization;

namespace PCL;

public partial class PageSetupFeedback
{
    public enum TagId : long
    {
        Processing = 6820804544L,
        WaitingProcess = 6820804546L,
        Completed = 6820804547L,
        Decline = 6820804539L,
        Ignored = 8064650117L,
        Duplicate = 6820804541L,
        Wait = 8743070786L,
        Pause = 8558220235L,
        Upnext = 8550609020L
    }

    private bool _isLoaded;

    public ModLoader.LoaderTask<bool, List<Feedback>> Loader;

    private static readonly Dictionary<string, (Func<PageSetupFeedback, StackPanel> Panel, string Icon)> TagMap = new()
    {
        [((long)TagId.Processing).ToString()] = (p => p.PanListProcessing, "Blocks/CommandBlock.png"),
        [((long)TagId.WaitingProcess).ToString()] = (p => p.PanListWaitingProcess, "Blocks/RedstoneBlock.png"),
        [((long)TagId.Wait).ToString()] = (p => p.PanListWait, "Blocks/Anvil.png"),
        [((long)TagId.Pause).ToString()] = (p => p.PanListPause, "Blocks/RedstoneLampOff.png"),
        [((long)TagId.Upnext).ToString()] = (p => p.PanListUpnext, "Blocks/RedstoneLampOn.png"),
        [((long)TagId.Completed).ToString()] = (p => p.PanListCompleted, "Blocks/Grass.png"),
        [((long)TagId.Decline).ToString()] = (p => p.PanListDecline, "Blocks/CobbleStone.png"),
        [((long)TagId.Ignored).ToString()] = (p => p.PanListIgnored, "Blocks/CobbleStone.png"),
        [((long)TagId.Duplicate).ToString()] = (p => p.PanListDuplicate, "Blocks/CobbleStone.png"),
    };

    // 同时按标签名称匹配状态，避免切换仓库后标签 ID 变化导致分类失效
    private static readonly (string Keyword, TagId Tag)[] TagNameMap =
    [
        ("正在处理", TagId.Processing),
        ("等待处理", TagId.WaitingProcess),
        ("等待", TagId.Wait),
        ("暂停", TagId.Pause),
        ("在即", TagId.Upnext),
        ("已完成", TagId.Completed),
        ("已拒绝", TagId.Decline),
        ("已忽略", TagId.Ignored),
        ("重复", TagId.Duplicate),
        ("awaiting processing", TagId.WaitingProcess),
        ("processing", TagId.Processing),
        ("waiting", TagId.Wait),
        ("paused", TagId.Pause),
        ("up next", TagId.Upnext),
        ("completed", TagId.Completed),
        ("declined", TagId.Decline),
        ("ignored", TagId.Ignored),
        ("duplicate", TagId.Duplicate)
    ];

    public PageSetupFeedback()
    {
        InitializeComponent();
        Loader = new ModLoader.LoaderTask<bool, List<Feedback>>("FeedbackList", FeedbackListGet);
        Loaded += PageOtherFeedback_Loaded;
    }

    private void PageOtherFeedback_Loaded(object sender, RoutedEventArgs e)
    {
        PageLoaderInit(Load, PanLoad, PanContent, PanInfo, Loader, _ => RefreshList());
        // 重复加载部分
        PanBack.ScrollToHome();
        // 非重复加载部分
        if (_isLoaded)
            return;
        _isLoaded = true;
    }

    public void FeedbackListGet(ModLoader.LoaderTask<bool, List<Feedback>> task)
    {
        var list = Requester.FetchJson(
            $"{UpdatesGitHubReleaseModel.IssuesApiUrl}?state=all&sort=created&per_page=200",
            new RequestParam
            {
                Retries = 3,
                UseBrowserUserAgent = true
            }) as JsonArray; // 获取近期 200 条数据就够了
        if (list is null)
            throw new Exception(Lang.Text("Setup.Feedback.LoadFailed"));
        var res = new List<Feedback>();
        foreach (var i in list)
        {
            if (i is not JsonObject issue) continue;
            var pullRequestToken = issue["pull_request"];
            if (pullRequestToken is not null && pullRequestToken.GetValueKind() != JsonValueKind.Null) continue;

            var item = issue.Deserialize<Feedback>();
            if (item is null) continue;
            var userLogin = (issue["user"] as JsonObject)?["login"]?.ToString();
            var issueId = issue["number"]?.ToString();
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(issueId))
                continue;
            item.User = userLogin;
            item.Id = issueId;

            var issueType = Lang.Text("Setup.Feedback.Uncategorized");
            var typeToken = issue["type"];
            if (typeToken is not null && typeToken.GetValueKind() == JsonValueKind.Object)
            {
                var typeNameToken = typeToken["name"];
                if (typeNameToken is not null) issueType = typeNameToken.ToString().ToLower();
            }

            item.Type = issueType;
            item.IsOpen = string.Equals(issue["state"]?.ToString(), "open", StringComparison.OrdinalIgnoreCase);

            if (issue["labels"] is JsonArray thisTags)
                foreach (var thisTag in thisTags)
                    if (thisTag is JsonObject tagObj)
                    {
                        var labelId = tagObj["id"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(labelId))
                            item.Tags.Add(labelId);

                        var labelName = tagObj["name"]?.ToString();
                        if (string.IsNullOrWhiteSpace(labelName)) continue;

                        var statusTag = TagNameMap.FirstOrDefault(tag =>
                            labelName.Contains(tag.Keyword, StringComparison.OrdinalIgnoreCase)).Tag;
                        if (statusTag == default) continue;

                        var statusTagId = ((long)statusTag).ToString();
                        if (!item.Tags.Contains(statusTagId))
                            item.Tags.Add(statusTagId);
                    }

            res.Add(item);
        }

        task.output = res;
    }

    private MyListItem CreateFeedbackItem(Feedback item, string logo)
    {
        var li = new MyListItem
        {
            Title = item.Title,
            Type = MyListItem.CheckType.Clickable,
            Info = $"{item.User} | {Lang.Date(item.Time)}",
            Logo = ModBase.pathImage + logo,
            Tags = item.Type
        };

        li.Click += (_, _) => ShowFeedbackDetail(item);

        return li;
    }

    private void ShowFeedbackDetail(Feedback item)
    {
        var timeSpanText = Lang.TimeSpan(item.Time - DateTime.Now);
        switch (ModMain.MyMsgBoxMarkdown(
                    Lang.Text("Setup.Feedback.Item.Submitter", item.User, timeSpanText) + "\n" +
                    Lang.Text("Setup.Feedback.Item.Type", item.Type) + "\n\n" +
                    item.Content,
                    $"#{item.Id} {item.Title}", button2: Lang.Text("Setup.Feedback.Item.ViewDetail")))
        {
            case 2:
            {
                ModBase.OpenWebsite(item.Url);
                break;
            }
        }
    }

    private void SetPanelVisibility(StackPanel panel, MyCard card)
    {
        card.Visibility = panel.Children.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    public void RefreshList()
    {
        PanListProcessing.Children.Clear();
        PanListWaitingProcess.Children.Clear();
        PanListWait.Children.Clear();
        PanListPause.Children.Clear();
        PanListUpnext.Children.Clear();
        PanListCompleted.Children.Clear();
        PanListDecline.Children.Clear();
        PanListIgnored.Children.Clear();
        PanListDuplicate.Children.Clear();

        foreach (var item in Loader.output)
        {
            var tag = item.Tags.Find(TagMap.ContainsKey)
                      ?? ((long)(item.IsOpen ? TagId.WaitingProcess : TagId.Completed)).ToString();
            var (panel, icon) = TagMap[tag];
            panel(this).Children.Add(CreateFeedbackItem(item, icon));
        }

        SetPanelVisibility(PanListProcessing, PanContentProcessing);
        SetPanelVisibility(PanListWaitingProcess, PanContentWaitingProcess);
        SetPanelVisibility(PanListWait, PanContentWait);
        SetPanelVisibility(PanListPause, PanContentPause);
        SetPanelVisibility(PanListUpnext, PanContentUpnext);
        SetPanelVisibility(PanListCompleted, PanContentCompleted);
        SetPanelVisibility(PanListDecline, PanContentDecline);
        SetPanelVisibility(PanListIgnored, PanContentIgnored);
        SetPanelVisibility(PanListDuplicate, PanContentDuplicate);
    }

    private void Feedback_Click(object sender, MouseButtonEventArgs e)
    {
        PageSetupLeft.TryFeedback();
    }

    public class Feedback
    {
        [JsonIgnore]
        public string User { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; init; } = string.Empty;
        [JsonPropertyName("created_at")]
        public DateTime Time { get; init; }
        [JsonPropertyName("body")]
        public string Content { get; init; } = string.Empty;
        [JsonPropertyName("html_url")]
        public string Url { get; init; } = string.Empty;
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsOpen { get; set; } = true;
        public List<string> Tags { get; } = new();
        public string Type { get; set; } = string.Empty;
    }
}