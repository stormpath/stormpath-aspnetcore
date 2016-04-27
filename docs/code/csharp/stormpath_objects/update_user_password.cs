[FromServices]
public Lazy<IAccount> Account { get; set; }


[HttpPost]
public async Task<IActionResult> UpdatePassword(string newPassword)
{
    if (Account.Value != null)
    {
        var stormpathAccount = Account.Value;
        stormpathAccount.SetPassword(newPassword);
        await stormpathAccount.SaveAsync();
    }

    return RedirectToAction("Index");
}
