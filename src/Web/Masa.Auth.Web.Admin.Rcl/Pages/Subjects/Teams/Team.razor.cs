﻿namespace Masa.Auth.Web.Admin.Rcl.Pages.Subjects.Teams;

public partial class Team
{
    private string? _search;
    private bool _enabled;
    private int _pageIndex = 1;
    private int _pageSize = 10;

    public string Search
    {
        get { return _search ?? ""; }
        set
        {
            _search = value;
            GetTeamItemsAsync().ContinueWith(_ => InvokeAsync(StateHasChanged));
        }
    }

    public bool Enabled
    {
        get { return _enabled; }
        set
        {
            _enabled = value;
            GetTeamItemsAsync().ContinueWith(_ => InvokeAsync(StateHasChanged));
        }
    }

    public int PageIndex
    {
        get { return _pageIndex; }
        set
        {
            _pageIndex = value;
            GetTeamItemsAsync().ContinueWith(_ => InvokeAsync(StateHasChanged));
        }
    }

    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            _pageSize = value;
            GetTeamItemsAsync().ContinueWith(_ => InvokeAsync(StateHasChanged));
        }
    }

    public int PageCount { get; set; }

    public long TotalCount { get; set; }

    public List<int> PageSizes = new() { 10, 25, 50, 100 };

    public List<TeamItemResponse> Teams { get; set; } = new();

    public Guid CurrentTeamId { get; set; }

    public bool TeamDialogVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetTeamItemsAsync();
    }

    public async Task GetTeamItemsAsync()
    {
        Loading = true;
        var response = await AuthClient.GetTeamItemsAsync();
        if (response.Success)
        {
            Teams = response.Data;
        }
        else OpenErrorMessage(T("Failed to query teamList data:") + response.Message);
        Loading = false;
    }

    public void OpenAddTeamDialog()
    {
        CurrentTeamId = Guid.Empty;
        TeamDialogVisible = true;
    }

    public void OpenEditUserDialog(UserItemResponse user)
    {
        CurrentTeamId = user.UserId;
        TeamDialogVisible = true;
    }

    public void OpenAuthorizeDialog(UserItemResponse user)
    {
        CurrentTeamId = user.UserId;
        TeamDialogVisible = true;
    }
}

